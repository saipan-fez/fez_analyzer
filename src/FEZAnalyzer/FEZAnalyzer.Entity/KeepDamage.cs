using System;

namespace FEZAnalyzer.Entity
{
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
}
