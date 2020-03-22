using FEZAnalyzer.Entity;
using OpenCvSharp;

namespace FEZAnalyzer.ResultRecognize.NewUI.Base
{
    internal class CountScoreScanner
    {
        private static readonly Vec3b White = new Vec3b(0xFF, 0xFF, 0xFF);
        private const int Width  = 60;
        private const int Height = 16;

        // 走査するy座標
        private const int Y = 5;

        private readonly Rect _rect;

        public CountScoreScanner(int left, int top)
        {
            _rect = new Rect(left, top, Width, Height);
        }

        public int? Scan(WarResultImage resultImage)
        {
            var ret = "";

            using (var tmp = resultImage.Image.Clone(_rect))
            using (var mat = new Mat<Vec3b>(tmp))
            {
                // 二値化
                mat.Threshold();

                var idx = mat.GetIndexer();
                for (var x = 0; x < mat.Width - 3; x++)
                {
                    if (idx[Y, x] != White)
                    {
                        continue;
                    }

                    var r1c = idx[Y, x + 1]; // 1px横のピクセルの色
                    var r2c = idx[Y, x + 2]; // 2px横のピクセルの色
                    var r3c = idx[Y, x + 3]; // 3px横のピクセルの色

                    if (r1c != White)
                    {
                        // Group:1px
                        // 指定のピクセルの色をチェック
                        var c1 = idx[Y - 1, x + 1]; // (-1, +1)px
                        var c2 = idx[Y + 1, x + 0]; // (+1,  0)px
                        var c3 = idx[Y + 1, x + 1]; // (+1, +1)px
                        var c4 = idx[Y + 2, x + 1]; // (+2, +1)px

                        if (c1 == White && c2 == White && c3 != White && c4 == White)       // true , true , false, true :0
                        {
                            ret += "0";
                        }
                        else if (c1 != White && c2 == White && c3 == White && c4 != White)  // false, true , true , false:9
                        {
                            ret += "9";
                        }
                        else if (c1 != White && c2 == White && c3 != White && c4 != White)  // false, true , false, false:1
                        {
                            ret += "1";
                        }
                        else if (c1 != White && c2 != White && c3 == White && c4 != White)  // false, false, true , false:3
                        {
                            ret += "3";
                        }
                        else if (c1 != White && c2 != White && c3 != White && c4 != White)  // false, false, false, false:4
                        {
                            ret += "4";
                        }
                        else if (c1 == White && c2 == White && c3 != White && c4 != White)  // true , true , false, false
                        {
                            // (-2, +1)pxの色をチェック
                            if (idx[Y - 2, x + 1] == White)
                            {
                                ret += "7";
                            }
                            else
                            {
                                ret += "2";
                            }
                        }
                    }
                    else if (r1c == White && r2c != White)
                    {
                        // Group:2px
                        // (+3, 0)pxの色をチェック
                        if (idx[Y + 3, x + 0] == White)
                        {
                            ret += "8";
                        }
                        else
                        {
                            ret += "5";
                        }
                    }
                    else if (r1c == White && r2c == White && r3c != White)
                    {
                        // Group:3px
                        ret += "6";
                    }

                    // 同じ数字の部分は走査をスキップさせる
                    //   1文字の中で連続しない#FFFFFFのピクセルがあるため、一律4pxスキップさせる
                    x += 4;
                }
            }

            return string.IsNullOrEmpty(ret) ? (int?)null : int.Parse(ret);
        }
    }
}
