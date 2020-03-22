using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FEZAnalyzer.ResourceProvider
{
    public class PowHashTableProvider
    {
        private static string ResourceNamespace = "FEZAnalyzer.ResourceProvider.assets.PowBitmap";
        private static ConcurrentDictionary<string, int> _table = null;

        public static async Task<IReadOnlyDictionary<string, int>> ProvideAsync()
        {
            return await Task.Run(() =>
            {
                return _table ?? CreateTable();
            });
        }

        private static ConcurrentDictionary<string, int> CreateTable()
        {
            _table = new ConcurrentDictionary<string, int>();

            var assembly     = typeof(MapHashTableProvider).Assembly;
            var powResources = assembly.GetManifestResourceNames()
                .Where(x => x.IndexOf(ResourceNamespace) == 0);

            // 画像ファイルを読み込みハッシュ値を計算するため並列化して実行
            Parallel.ForEach(powResources, r =>
            {
                // ハッシュ値計算
                string hash = null;
                using (var stream = assembly.GetManifestResourceStream(r))
                using (var mat = Mat.FromStream(stream, ImreadModes.Color))
                {
                    hash = mat.CalculateHash();
                }

                // マップ名取得
                //   拡張子を除いたファイル名を取得
                //   ex)"FEZAnalyzer.ResourceProvider.assets.MapBitmap.アシロマ山麓.bmp"
                var powNumStr = r.Split(".")[^2];

                // 数値に変換
                //   none.bmp は変換できないため 0 にする
                int powNum = 0;
                if (Regex.IsMatch(powNumStr, "^[0-9]{1}$"))
                {
                    powNum = int.Parse(powNumStr);
                }

                // テーブルに追加
                if (!_table.TryAdd(hash, powNum))
                {
                    // 追加に失敗するケースは、キーの重複などによるもののため例外としてスローする
                    // ※並列処理で同時に追加されるケースなどでは失敗しない
                    throw new Exception($"TryAdd failed. key:{hash} value:{powNum}");
                }
            });

            return _table;
        }
    }
}
