﻿using FEZAnalyzer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FEZAnalyzer.SkillCount
{
    internal class SkillUseAlgorithm : ISkillUseRecognizer
    {
        /// <summary>
        /// フレーム毎に回復する可能性のある値の閾値
        /// </summary>
        /// <remarks>
        /// Pow回復中に、パワブレまたはスキル使用によるPow減少が起きると、
        /// 本来 20 消費するはずのところが、回復によって 15 といったことになりうる。
        /// フレーム取得タイミングが一定でないため、回復量も一定ではない。
        /// そのためフレーム毎で回復しうる閾値を設ける。
        /// </remarks>
        private const int PowRegenerateThreashold = 5;

        public class DebuffData
        {
            public int DebuffId { get; set; }
            public long TimeStamp { get; set; }
            public int[] Pow { get; set; }
        }

        private long _previousTimeStamp;
        private int _previousPow;
        private PowDebuff[] _previousPowDebuff;
        private Skill _previousActiveSkill;     // 1フレーム以上前に選択していたスキル(必ずIsEmpty()=falseになる)
        private Skill _previousSelectedSkill;   // 1つ前に選択していたスキル(必ずIsEmpty()=falseになる)

        private List<DebuffData> _debuffList = new List<DebuffData>();
        public IReadOnlyList<DebuffData> DebuffList
        {
            get
            {
                return _debuffList;
            }
        }

        public SkillUseAlgorithm()
        {
            Reset();
        }

        public void Reset()
        {
            _previousTimeStamp     = long.MinValue;
            _previousPow           = int.MinValue;
            _previousPowDebuff     = null;
            _previousActiveSkill   = null;
            _previousSelectedSkill = null;
            _debuffList.Clear();
        }

        public Skill RecognizeUsedSkill(long timeStamp, int pow, Skill[] skills, PowDebuff[] powDebuff)
        {
            if (timeStamp < 0 || pow < 0)
            {
                throw new ArgumentException($"{nameof(timeStamp)}:{timeStamp} {nameof(pow)}:{pow}");
            }

            /* パワーブレイク判定アルゴリズムについて
             *
             *  ■概要
             *  スキルの使用はPowの減少によって判断する。
             *  しかし、Powの減少はスキルの使用以外にパワーブレイク(以下パワブレ)によっても減少する。
             *  そこで、Powの減少がパワブレによるものかどうか判定し、
             *  パワブレによるPow減少でなければスキル使用と判断する。
             *
             *  ■前提
             *  1. 複数Pow減少デバフが存在しないものとする
             *       → Pow減少デバフは現状スカウトのパワブレのみのため
             *  2. Pow回復は徐々に増加する
             *       → 50から65に回復するとき、53, 55, 60…といった具合に増加する。
             *          増加量は取得するフレームの間隔に依存するため一定ではない。
             *  3. Pow減少(スキル使用・パワブレ)は一気に減少する
             *       → 回復と異なり減少は一気に行われる
             *
             *  ■アルゴリズム
             *  前回と今回のPowBuffの一覧を比較し、新たにパワブレが追加されていれば、
             *  前回から今回までの間にパワブレをもらったということになる。
             *  パワブレはスキルをもらってから、3, 6, 9 ... 24秒経過後(3秒間隔)でPowが減少するため、
             *  1回目のPow減少は "前回のタイムスタンプ + 3秒後"、
             *  2回目のPow減少は "前回のタイムスタンプ + 6秒後"、
             *  ・
             *  ・
             *  ・
             *  がそれぞれ最短のPow減少タイミングとなる。
             *  パワブレをもらったタイミングで減少する最短タイミングをリストに追加し、
             *  以降はその最短タイミングを超えていないかチェックし、
             *  超えていればその時点でのPow減少がパワブレによるものと判定する。
             */
            Skill usedSkill = null;

            // 選択中のスキル取得
            var activeSkill = skills.FirstOrDefault(x => x.State == SkillState.Active);
            if (activeSkill == null)
            {
                activeSkill = Skill.UnknownSkill;
            }

            // 初回実行時は判定しようがないので終了
            if (_previousPowDebuff == null || _previousPow == int.MinValue)
            {
                goto Finish;
            }

            // 今回新たに発生したデバフ(今回と前回の差集合)を取得
            var enterPowDebuffs = powDebuff.Except(_previousPowDebuff, x => x.Id);

            // 新たに発生したデバフがあればリストに追加
            if (enterPowDebuffs.Any())
            {
                foreach (var debuff in enterPowDebuffs)
                {
                    for (int i = 0; i < debuff.EffectCount; i++)
                    {
                        _debuffList.Add(new DebuffData()
                        {
                            DebuffId = debuff.Id,
                            TimeStamp = (_previousTimeStamp + (i + 1) * debuff.EffectTimeSpan.Ticks),
                            Pow = debuff.Pow.Clone() as int[]
                        });
                    }
                }

                // タイムスタンプの昇順でソート(＝ 直近のもの)
                _debuffList = _debuffList.OrderBy(x => x.TimeStamp).ToList();

                Logger.WriteLine($"-------------------------");
                Logger.WriteLine($"PowDebuffList added.");
                Logger.WriteLine($"[debuffList]");
                Logger.WriteLine($"{string.Join("     " + Environment.NewLine, _debuffList.Select(x => "ID:" + x.DebuffId + " TimeStamp:" + x.TimeStamp + " Pow:[" + string.Join(",", x.Pow) + "]"))}");
            }

            //var ask = activeSkill != null ? activeSkill.Name : "null";
            //Logger.WriteLine($"now:{pow} prev:{_previousPow}");
            //Logger.WriteLine($"activeSkill:{ ask }");

            if (pow >= _previousPow)
            {
                // 以下の場合はスキルを使用していないため終了
                // ・前回と同じPow
                // ・前回よりPowが多い(回復した)
                goto CheckLeaveDebuff;
            }

            // 今回の消費Pow
            var powDiff = _previousPow - pow;

            // 今回のPow減少デバフの候補一覧を取得
            var list = _debuffList.Where(x => timeStamp > x.TimeStamp).ToArray();
            if (list.Any())
            {
                // 今回のデバフによる消費Powとして考えうるもの一覧
                // TODO: デバフが1つ(パワブレ)前提の作りになっている。
                //       2つ以上デバフが存在する場合は、デバフによる消費Powの取りうる値をすべて出す必要がある。
                //       対応すると無駄に複雑な処理となるため除外する。
                var debuffPowSum = list.First().Pow;

                // Pow回復とデバフによるPow消費が同時に発生した際を考慮し、
                // 回復しうる閾値以下であればデバフによるPow消費と判断する。
                // なお、上記にさらにスキル使用によるPow消費が同時に発生しうるが、
                // こちらは滅多にないため考慮しない。
                // TODO: その場合でも以降問題なく動作するようにする(現状ではおそらくバグる)
                if (debuffPowSum.Any(x => Math.Abs(x + powDiff) < PowRegenerateThreashold))
                {
                    Logger.WriteLine($"-------------------------");
                    Logger.WriteLine($"Detected to PowDebuff.");
                    Logger.WriteLine($"[previous]");
                    Logger.WriteLine($"     time:{_previousTimeStamp} pow:{_previousPow} debuff:{string.Join(",", _previousPowDebuff.Select(x => x.Name))}");
                    Logger.WriteLine($"[current]");
                    Logger.WriteLine($"     time:{timeStamp} pow:{pow} debuff:{string.Join(",", powDebuff.Select(x => x.Name))}");

                    // デバフによるPow消費があったため一覧から削除
                    foreach (var d in list)
                    {
                        _debuffList.Remove(d);
                    }
                }
            }
            else
            {
                var tmpActiveSkill = activeSkill;
                // パニはスキル発動と同時に1つ上のスキルが選択状態となる
                // そのため特殊ケースとして1つ前に選択状態だったスキルを確認して判定する
                if (tmpActiveSkill != null && _previousSelectedSkill != null &&
                    _previousSelectedSkill.Name == "パニッシングストライク" &&
                    tmpActiveSkill.Name != "パニッシングストライク")
                {
                    tmpActiveSkill = _previousSelectedSkill;
                }

                // 現在選択中のスキルが不明な場合、解析できていた以前のスキル情報を使用する
                // (ゲイザーはスキル発動と同時にスキルアイコンが特殊なものに変化するため)
                if (tmpActiveSkill == null || tmpActiveSkill.IsUnknownSkill())
                {
                    tmpActiveSkill = _previousActiveSkill;
                }

                if (tmpActiveSkill != null &&
                    tmpActiveSkill.Pow.Any(x => Math.Abs(x - powDiff) < PowRegenerateThreashold))
                {
                    Logger.WriteLine($"-------------------------");
                    Logger.WriteLine($"Detected to use skill. skill:{tmpActiveSkill.Name}");
                    Logger.WriteLine($"[previous]");
                    Logger.WriteLine($"     time:{_previousTimeStamp} pow:{_previousPow} debuff:{string.Join(",", _previousPowDebuff.Select(x => x.Name))}");
                    Logger.WriteLine($"[current]");
                    Logger.WriteLine($"     time:{timeStamp} pow:{pow} debuff:{string.Join(",", powDebuff.Select(x => x.Name))}");

                    // Pow回復とスキル使用によるPow消費が同時に発生した際を考慮し、
                    // 回復しうる閾値以下であればスキル使用によるPow消費と判断する。
                    usedSkill = tmpActiveSkill;
                }
            }

    CheckLeaveDebuff:
            // 今回消失したもの(前回と今回の差集合)を取得し、
            // 消失されているものの一覧に該当デバフがあれば削除する。
            // 通常のPow減少などでは常にfalseとなるが、
            // パワブレ状態でデッドしてしまうとデバフが削除されるが、
            // 一覧には残り続けるため、そのための対応としてチェック→削除する。
            var leavePowDebuffs = _previousPowDebuff.Except(powDebuff, x => x.Id);
            if (leavePowDebuffs.Any())
            {
                _debuffList.RemoveAll(x => leavePowDebuffs.Any(y => y.Id == x.DebuffId));

                Logger.WriteLine($"-------------------------");
                Logger.WriteLine($"_debuffList removed.");
                Logger.WriteLine($"{string.Join("     " + Environment.NewLine, _debuffList.Select(x => "ID:" + x.DebuffId + " TimeStamp:" + x.TimeStamp + " Pow:[" + string.Join(",", x.Pow) + "]"))}");
            }

    Finish:
            // 今回の値を保持
            //   _previousSelectedSkillのみ選択中のスキルが変更した場合のみ更新
            if (_previousActiveSkill != null && activeSkill != null &&
                !_previousActiveSkill.IsUnknownSkill() && !activeSkill.IsUnknownSkill() &&
                _previousActiveSkill.Name != activeSkill.Name)
            {
                _previousSelectedSkill = _previousActiveSkill;
            }
            if (activeSkill != null && !activeSkill.IsUnknownSkill())
            {
                _previousActiveSkill = activeSkill;
            }
            _previousTimeStamp   = timeStamp;
            _previousPow         = pow;
            _previousPowDebuff   = powDebuff;

            //var pas = _previousActiveSkill != null ? _previousActiveSkill.Name : "null";
            //var pss = _previousSelectedSkill != null ? _previousSelectedSkill.Name : "null";

            //Logger.WriteLine($"_previousActiveSkill  :{pas}");
            //Logger.WriteLine($"_previousSelectedSkill:{pss}");
            //Logger.WriteLine($"-------------------------");

            return usedSkill;
        }
    }
}
