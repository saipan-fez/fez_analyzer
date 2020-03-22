using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;

namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class WarTimeScanner
    {
        private static readonly Vec3b White = new Vec3b(0xFF, 0xFF, 0xFF);

        // 走査するy座標
        private const int Y = 0;

        private readonly Rect _rect = new Rect(105, 273, 314, 9);

        public TimeSpan? Scan(WarResultImage resultImage)
        {
            var str = "";

            // 解析アルゴリズムは DetailScoreScanner と同一(フォント・文字サイズが同じ)
            using (var tmp = resultImage.Image.Clone(_rect))
            using (var mat = new Mat<Vec3b>(tmp))
            {
                var idx = mat.GetIndexer();
                for (var x = 0; x < mat.Width - 3; x++)
                {
                    if (idx[Y, x] != White)
                    {
                        continue;
                    }

                    var skipPixel = 0;
                    var r1c = idx[Y, x + 1]; // 1px横のピクセルの色
                    var r2c = idx[Y, x + 2]; // 2px横のピクセルの色
                    var r3c = idx[Y, x + 3]; // 3px横のピクセルの色

                    if (r1c != White)
                    {
                        // Group:1px
                        // (+2, -1)pxの色をチェック
                        if (idx[Y + 2, x - 1] == White)
                        {
                            str += "4";
                        }
                        else
                        {
                            str += "1";
                        }

                        skipPixel = 0;
                    }
                    else if (r1c == White && r2c != White)
                    {
                        // Group:2px
                        // 指定のピクセルの色をチェック
                        var c1 = idx[Y + 3, x - 1]; // (+3, -1)px
                        var c2 = idx[Y + 4, x - 1]; // (+4, -1)px
                        var c3 = idx[Y + 5, x - 1]; // (+5, -1)px
                        var c4 = idx[Y + 4, x + 0]; // (+4,  0)px

                        if (c1 == White && c2 == White && c3 == White && c4 == White)       // true,  true,  true,  true :6
                        {
                            str += "6";
                        }
                        else if (c1 == White && c2 == White && c3 == White && c4 != White)  // true,  true,  true,  false:0
                        {
                            str += "0";
                        }
                        else if (c1 == White && c2 != White && c3 == White && c4 == White)  // true,  false, true,  true :8
                        {
                            str += "8";
                        }
                        else if (c1 == White && c2 != White && c3 != White && c4 == White)  // true,  false, false, true :9
                        {
                            str += "9";
                        }
                        else if (c1 != White && c2 != White && c3 != White && c4 != White)  // false, false, false, false:2
                        {
                            str += "2";
                        }
                        else if (c1 != White && c2 != White && c3 != White && c4 == White)  // false, false, false, true :3
                        {
                            str += "3";
                        }

                        skipPixel = 1;
                    }
                    else if (r1c == White && r2c == White && r3c == White)
                    {
                        // Group:4px
                        // (+1, 0)pxの色をチェック
                        if (idx[Y + 1, x + 0] == White)
                        {
                            str += "5";
                        }
                        else
                        {
                            str += "7";
                        }

                        skipPixel = 3;
                    }

                    // 同じ数字の部分は走査をスキップさせる
                    x += skipPixel;
                }
            }

            TimeSpan? ret = null;
            if (!string.IsNullOrEmpty(str))
            {
                // retには数字のみ入っているため、経過時間に変換する
                var min = str.Substring(0, str.Length - 2);
                var sec = str.Substring(str.Length - 2, 2);

                ret = TimeSpan.Parse($"00:{min}:{sec}");
            }

            return ret;
        }
    }
}
