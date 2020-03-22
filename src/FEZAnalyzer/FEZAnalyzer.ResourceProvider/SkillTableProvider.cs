using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FEZAnalyzer.ResourceProvider
{
    public class SkillTableProvider
    {
        private static string ResourceNamespace = "FEZAnalyzer.ResourceProvider.assets.SkillBitmap";
        private static ConcurrentDictionary<Mat<Vec3b>, Skill> _table = null;

        public static async Task<IReadOnlyDictionary<Mat<Vec3b>, Skill>> ProvideAsync()
        {
            return await Task.Run(() =>
            {
                return _table ?? CreateTable();
            });
        }

        private static ConcurrentDictionary<Mat<Vec3b>, Skill> CreateTable()
        {
            _table = new ConcurrentDictionary<Mat<Vec3b>, Skill>();

            var assembly      = typeof(MapHashTableProvider).Assembly;
            var skillResources = assembly.GetManifestResourceNames()
                .Where(x => x.IndexOf(ResourceNamespace) == 0);

            // 画像ファイルを読み込むため並列化して実行
            Parallel.ForEach(skillResources, r =>
            {
                // 画像読み込み
                Mat<Vec3b> mat = null;
                using (var stream = assembly.GetManifestResourceStream(r))
                {
                    var tmp = Mat.FromStream(stream, ImreadModes.Color);
                    mat = new Mat<Vec3b>(tmp);
                }

                // ファイル名取得
                //   拡張子を除いたファイル名を取得
                //   ex)"FEZAnalyzer.ResourceProvider.assets.SkillBitmap.Cestus@アースバインド@N@バインド@42.bmp"
                var skillFileName = r.Split(".")[^2];

                // ファイル名を分解
                //   Fencer@オブティンプロテクト@N@オブティンプロテクト@25_28
                //   ↑     ↑                  ↑    ↑                ↑
                //   職業名 スキル名             状態 スキル短縮名      消費Pow
                //
                //   [状態]
                //   N: 非選択状態
                //   S: 選択状態
                //   D: 無効状態
                //
                //   [消費Pow]
                //   複数ある場合は "_" で区切られている
                //
                var split = skillFileName.Split("@");
                var work  = split[0];
                var name  = split[1];
                var state = SkillState.Invalid;
                switch (split[2])
                {
                    case "N":
                        state = SkillState.NonActive; break;
                    case "S":
                        state = SkillState.Active; break;
                    case "D":
                        state = SkillState.Disable; break;
                }
                var sname = split[3];
                var pows  = split[4].Split("_").Select(s => int.Parse(s)).ToArray();

                var skill = new Skill(name, sname, work, pows, state);

                // テーブルに追加
                if (!_table.TryAdd(mat, skill))
                {
                    // 追加に失敗するケースは、キーの重複などによるもののため例外としてスローする
                    // ※並列処理で同時に追加されるケースなどでは失敗しない
                    throw new Exception($"TryAdd failed. file:{skillFileName}");
                }
            });

            return _table;
        }
    }
}
