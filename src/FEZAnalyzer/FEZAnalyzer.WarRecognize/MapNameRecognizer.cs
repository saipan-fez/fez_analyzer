using FEZAnalyzer.Entity;
using FEZAnalyzer.ResourceProvider;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.WarRecognize
{
    /// <summary>
    /// Map名解析クラス
    /// </summary>
    internal class MapNameRecognizer : IWarRecognize<Map>
    {
        // MAP名が表示されている位置　(切り抜いて解析する範囲)
        private readonly Rect MapNameRect = new Rect(103, 10, 160, 11);
        private IReadOnlyDictionary<string, Map> _table = null;

        private MapNameRecognizer()
        { }

        public static async Task<MapNameRecognizer> CreateAsync()
        {
            var mapRecognizer = new MapNameRecognizer()
            {
                _table = await MapHashTableProvider.ProvideAsync()
            };

            return mapRecognizer;
        }

        public Map Recognize(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            using (var tmp = fezImage.Image.Clone(MapNameRect))
            using (var mat = new Mat<Vec3b>(tmp))
            {
                // 二値化
                Cv2.Threshold(mat, mat, 0, 255, ThresholdTypes.Binary);

                // ハッシュ値で該当するマップ名があるかチェック
                var hash = mat.CalculateHash();
                if (_table.TryGetValue(hash, out Map map))
                {
                    return map;
                }

                /*
                 * 闘技場マップ名と一致するかチェックする
                 * 
                 * 闘技場のマップは 「闘技場/闘技場01」 のように部屋番号がマップ名に含まれる。
                 * 画像一致でのマップ名解析では部屋番号をすべて用意する必要があるが、
                 * 現実的にすべてを用意することは難しいため、
                 * 部屋番号の部分(x=78以降)を白で塗りつぶしてチェックする
                 */
                // x=78以降を白で塗りつぶし
                var white   = new Vec3b(0xff, 0xff, 0xff);
                var indexer = mat.GetIndexer();
                for (var y = 0; y < mat.Height; y++)
                {
                    for (var x = 78; x < mat.Width; x++)
                    {
                        indexer[y, x] = white;
                    }
                }

                // 再度ハッシュ値で該当するマップ名があるかチェック
                var reHash = mat.CalculateHash();
                if (_table.TryGetValue(reHash, out Map reMap))
                {
                    return reMap;
                }

                return Map.UnknownMap;
            }
        }
    }
}
