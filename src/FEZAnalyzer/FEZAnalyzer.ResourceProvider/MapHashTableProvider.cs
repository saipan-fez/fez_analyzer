using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FEZAnalyzer.ResourceProvider
{
    public class MapHashTableProvider
    {
        private static string ResourceNamespace = "FEZAnalyzer.ResourceProvider.assets.MapBitmap";
        private static ConcurrentDictionary<string, Map> _table = null;

        public static async Task<IReadOnlyDictionary<string, Map>> ProvideAsync()
        {
            return await Task.Run(() =>
            {
                return _table ?? CreateTable();
            });
        }

        private static ConcurrentDictionary<string, Map> CreateTable()
        {
            _table = new ConcurrentDictionary<string, Map>();

            var assembly      = typeof(MapHashTableProvider).Assembly;
            var mapResources = assembly.GetManifestResourceNames()
                .Where(x => x.IndexOf(ResourceNamespace) == 0);

            // 画像ファイルを読み込みハッシュ値を計算するため並列化して実行
            Parallel.ForEach(mapResources, r =>
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
                var mapName = r.Split(".")[^2];

                // テーブルに追加
                if (!_table.TryAdd(hash, new Map(mapName)))
                {
                    // 追加に失敗するケースは、キーの重複などによるもののため例外としてスローする
                    // ※並列処理で同時に追加されるケースなどでは失敗しない
                    throw new Exception($"TryAdd failed. key:{hash} value:{mapName}");
                }
            });

            return _table;
        }
    }
}
