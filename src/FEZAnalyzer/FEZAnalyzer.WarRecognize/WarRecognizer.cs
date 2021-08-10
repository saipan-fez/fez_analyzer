using FEZAnalyzer.Entity;
using System;
using System.Threading.Tasks;

namespace FEZAnalyzer.WarRecognize
{
    public class WarRecognizer : IWarRecognizer
    {
        private BookUseRecognizer bookUseRecognizer;
        private CostDisplayRecognizer costDisplayRecognizer;
        private KeepDamageRecognizer keepDamageRecognizer;
        private MapNameRecognizer mapNameRecognizer;
        private PowDebuffArrayRecognizer powDebuffArrayRecognizer;
        private PowRecognizer powRecognizer;
        private HpRecognizer hpRecognizer;
        private SkillCollectionRecognizer skillCollectionRecognizer;

        private WarRecognizer()
        { }

        public static async Task<WarRecognizer> CreateAsync()
        {
            return await Task.Run(() =>
            {
                var w = new WarRecognizer()
                {
                    bookUseRecognizer         = new BookUseRecognizer(),
                    costDisplayRecognizer     = new CostDisplayRecognizer(),
                    keepDamageRecognizer      = new KeepDamageRecognizer(),
                    powDebuffArrayRecognizer  = new PowDebuffArrayRecognizer(),
                };

                Task.WaitAll(
                    Task.Run(async () => { w.powRecognizer             = await PowRecognizer.CreateAsync(); }),
                    Task.Run(async () => { w.hpRecognizer              = await HpRecognizer.CreateAsync(); }),
                    Task.Run(async () => { w.mapNameRecognizer         = await MapNameRecognizer.CreateAsync(); }),
                    Task.Run(async () => { w.skillCollectionRecognizer = await SkillCollectionRecognizer.CreateAsync(); })
                );

                return w;
            });
        }

        public bool TryRecognize(FezImage fezImage, out WarInfo warInfo)
        {
            warInfo = null;

            if (!costDisplayRecognizer.Recognize(fezImage))
            {
                return false;
            }

            var w = new WarInfo();

            Task.WaitAll(
                Task.Run(() => { w.IsBookUsing      = bookUseRecognizer.Recognize(fezImage); }),
                Task.Run(() =>
                {
                    var d = keepDamageRecognizer.Recognize(fezImage);
                    w.AttackKeepDamage  = d.AttackKeepDamage;
                    w.DefenceKeepDamage = d.DefenceKeepDamage;
                }),
                Task.Run(() => { w.Map        = mapNameRecognizer.Recognize(fezImage); }),
                Task.Run(() => { w.Hp         = hpRecognizer.Recognize(fezImage); }),
                Task.Run(() => { w.Pow        = powRecognizer.Recognize(fezImage); }),
                Task.Run(() => { w.PowDebuffs = powDebuffArrayRecognizer.Recognize(fezImage); }),
                Task.Run(() => { w.Skills     = skillCollectionRecognizer.Recognize(fezImage); })
            );

            warInfo = w;

            return true;
        }
    }

    /// <summary>
    /// キープ 残ポイント
    /// </summary>
    public class KeepDamage
    {
        /// <summary>
        /// 攻撃側 残ポイント
        /// </summary>
        public double AttackKeepDamage { get; }

        /// <summary>
        /// 防衛側 残ポイント
        /// </summary>
        public double DefenceKeepDamage { get; }

        public KeepDamage(double attack, double defence)
        {
            if (attack  < 0d || 3d < attack ||
                defence < 0d || 3d < defence)
            {
                throw new ArgumentOutOfRangeException("value must be between 0.0-3.0");
            }

            AttackKeepDamage  = attack;
            DefenceKeepDamage = defence;
        }
    }

    /// <summary>
    /// 戦争中情報
    /// </summary>
    public class WarInfo
    {
        /// <summary>
        /// 書を使用してるかどうか
        /// </summary>
        public bool IsBookUsing { get; set; }

        /// <summary>
        /// 攻撃側 残ポイント
        /// </summary>
        public double AttackKeepDamage { get; set; }

        /// <summary>
        /// 防衛側 残ポイント
        /// </summary>
        public double DefenceKeepDamage { get; set; }

        /// <summary>
        /// ヒットポイント
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        /// パワー
        /// </summary>
        public int Pow { get; set; }

        /// <summary>
        /// マップ
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// Powデバフ一覧
        /// </summary>
        public PowDebuff[] PowDebuffs { get; set; }

        /// <summary>
        /// スキル一覧
        /// </summary>
        public Skill[] Skills { get; set; }
    }
}
