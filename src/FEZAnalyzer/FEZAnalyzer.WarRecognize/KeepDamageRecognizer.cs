using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.WarRecognize
{
    /// <summary>
    /// 拠点ダメージ解析クラス
    /// </summary>
    internal class KeepDamageRecognizer : IWarRecognize<KeepDamage>
    {
        /// <summary>
        /// L*a*b色空間上での色差の閾値
        /// </summary>
        private const double Threshold = 10.0d;

        // 攻撃・防衛側それぞれで走査するy座標
        private const int Y_Attack  = 18;
        private const int Y_Defence = 51;

        // 走査するx座標 ※画像中心からの相対位置
        private const int X_leftOffset  = -124;
        private const int X_rightOffset =    3;

        // 攻撃・防衛側それぞれのゲージ色(RGBColor[])とゲージの数値(int)の組み合わせ
        // ※ゲージ色が2つあるのはゲージ両端の色が異なるため
        private readonly List<Tuple<Vec3b[], int>> AttackKeepDamageMap = new List<Tuple<Vec3b[], int>>()
        {
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0x2e, 0x85, 0xff), new Vec3b(0x2d, 0x9f, 0xff) }, 3),
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0x35, 0x4e, 0xf8), new Vec3b(0x3a, 0x5d, 0xfa) }, 2),
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0x43, 0x41, 0xcf), new Vec3b(0x47, 0x4c, 0xd2) }, 1),
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0x05, 0x22, 0x55),                             }, 0),
        };

        private readonly List<Tuple<Vec3b[], int>> DefenceKeepDamageMap = new List<Tuple<Vec3b[], int>>()
        {
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0xfd, 0x7d, 0x23), new Vec3b(0xfd, 0x95, 0x25) }, 3),
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0xf6, 0x4d, 0x40), new Vec3b(0xf9, 0x5c, 0x44) }, 2),
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0xd4, 0x42, 0x6b),                             }, 1),
            new Tuple<Vec3b[], int>(new Vec3b[] { new Vec3b(0x35, 0x21, 0x08),                             }, 0),
        };

        public KeepDamage Recognize(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            var mat     = fezImage.Image;
            var attack  = GetAttackKeepDamage(mat);
            var defence = GetDefenceKeepDamage(mat);

            // いずれかが解析できなければ失敗
            if (double.IsNaN(attack) || double.IsNaN(defence))
            {
                return null;
            }

            return new KeepDamage(attack, defence);
        }

        private double GetAttackKeepDamage(Mat<Vec3b> mat)
        {
            return GetKeepDamage(mat, Y_Attack, AttackKeepDamageMap);
        }

        private double GetDefenceKeepDamage(Mat<Vec3b> mat)
        {
            return GetKeepDamage(mat, Y_Defence, DefenceKeepDamageMap);
        }

        private double GetKeepDamage(Mat<Vec3b> mat, int y, List<Tuple<Vec3b[], int>> damageMap)
        {
            var center = mat.Width / 2;
            var leftX  = center + X_leftOffset;
            var rightX = center + X_rightOffset;

            var isValid = false;
            var value   = 0;
            var maxX    = rightX;
            var idx     = mat.GetIndexer();
            for (var x = leftX; x <= rightX; x++)
            {
                var color = idx[y, x];
                int v = -1;
                foreach (var t in damageMap)
                {
                    if (IsSameColorContains(color, t.Item1))
                    {
                        v       = t.Item2;
                        isValid = true;
                        break;
                    }
                }

                if (value < v)
                {
                    value   = v;
                    maxX    = x;
                }
            }

            if (!isValid)
            {
                return double.NaN;
            }

            var vv = Math.Max(0.0, value - 1);
            var ww = (rightX - maxX + 1.0) / (rightX - leftX + 1.0);

            return vv + ww;
        }

        private bool IsSameColorContains(Vec3b target, Vec3b[] colors)
        {
            return colors.Any(c =>
            {
                return ColorDiff.CompareAsLabColor(target, c, Threshold);
            });
        }
    }
}
