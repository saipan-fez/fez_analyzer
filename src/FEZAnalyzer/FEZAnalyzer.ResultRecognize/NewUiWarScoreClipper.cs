using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class NewUiWarScoreClipper
    {
        private const int Threshold = 3;

        // 戦績結果ウィンドウのサイズ
        //   X,Y座標は画像の中心点から戦績結果ウィンドウ左上の相対座標
        private readonly Rect ResultWindowRect = new Rect(-262, -353, 747, 706);

        // 検証する座標と色
        // X, Y, 色(BGR)
        private readonly List<(int, int, Vec3b)> ValidateList = new List<(int, int, Vec3b)>()
        {
            (200,  20, new Vec3b( 50,  50,  50)),
            (200,  50, new Vec3b(145, 155, 167)),
            (335, 730, new Vec3b( 50,  50,  50)),
            (550, 730, new Vec3b( 43,  47,  61)),
        };

        public WarResultImage Clip(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            var mat = fezImage.Image;

            // FEZのスコア画面は解像度に依存せず、一定の大きさである。
            // また、画面中央に表示されるため、中心から一定の範囲を切り抜いて、
            // 解析で扱う画像サイズを一定にする。
            var rect = ResultWindowRect;
            rect.X = mat.Width  / 2 + rect.X;
            rect.Y = mat.Height / 2 + rect.Y;

            // 戦績画面かどうかチェック
            var indexer = mat.GetIndexer();
            var isValid = ValidateList.All(v =>
            {
                var c = indexer[rect.Y + v.Item1, rect.X + v.Item2];
                return ColorDiff.CompareAsBgrColor(c, v.Item3, Threshold);
            });

            using (var m = mat.Clone(rect))
            {
                return isValid ?
                    new WarResultImage(new Mat<Vec3b>(m), fezImage.TimeStamp) :
                    null;
            }
        }
    }
}
