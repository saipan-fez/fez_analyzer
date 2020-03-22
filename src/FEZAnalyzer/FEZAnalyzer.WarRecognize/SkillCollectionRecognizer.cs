using FEZAnalyzer.Entity;
using FEZAnalyzer.ResourceProvider;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FEZAnalyzer.PerformanceTest")]
[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.WarRecognize
{
    internal class SkillCollectionRecognizer : IWarRecognize<Skill[]>
    {
        private const int Threshold = 3;

        // 各スキルが描かれている位置 (x座標のみ画像右側からの相対座標)
        private readonly Rect[] SkillRectTable = new Rect[]
        {
            new Rect(-30,  22, 12, 12),
            new Rect(-30,  54, 12, 12),
            new Rect(-30,  86, 12, 12),
            new Rect(-30, 118, 12, 12),
            new Rect(-30, 150, 12, 12),
            new Rect(-30, 182, 12, 12),
            new Rect(-30, 214, 12, 12),
            new Rect(-30, 246, 12, 12),
        };

        private IReadOnlyDictionary<Mat<Vec3b>, Skill> _table = null;

        private SkillCollectionRecognizer()
        { }

        public static async Task<SkillCollectionRecognizer> CreateAsync()
        {
            var skillCollectionRecognizer = new SkillCollectionRecognizer()
            {
                _table = await SkillTableProvider.ProvideAsync()
            };

            return skillCollectionRecognizer;
        }

        public Skill[] Recognize(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            var mat = fezImage.Image;
            var skills = new Skill[SkillRectTable.Length];

            Parallel.For(0, SkillRectTable.Length, i =>
            {
                // 切り抜く対象範囲
                //   X のみ右端からの相対座標のため絶対座標に変換
                var rect = SkillRectTable[i];
                rect.X = mat.Width + rect.X;

                // スキルアイコンの部分を切り抜き、一致するスキルを検索する
                //
                // 環境によって何故か微妙に色が異なることがあるため、
                // 完全一致ではなく閾値以内であれば一致とする。
                // なおL*a*b色空間では誤認識が多かったため、あえてRGB色空間で色差をみる。
                using (var tmp = mat.Clone(rect))
                using (var m   = new Mat<Vec3b>(tmp))
                {
                    foreach (var skill in _table)
                    {
                        if (m.CompareAsRgbColor(skill.Key, Threshold))
                        {
                            skills[i] = skill.Value;
                            break;
                        }
                    }
                }
            });

            return skills;
        }
    }
}
