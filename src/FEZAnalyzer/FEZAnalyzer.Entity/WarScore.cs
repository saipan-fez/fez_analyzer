using System;

namespace FEZAnalyzer.Entity
{
    public enum WarResult
    {
        Win,
        Lose
    }

    public enum Side
    {
        Offence,
        Defence
    }

    public enum Country
    {
        Ielsord,
        Netzawar,
        Hordaine,
        Geburand,
        Cesedria
    }

    public class WarScore
    {
        public DateTime 記録日時 { get; set; }
        public WarResult 勝敗 { get; set; }
        public Side 参戦側 { get; set; }
        public Country 攻撃側国 { get; set; }
        public Country 防衛側国 { get; set; }
        public TimeSpan 戦争継続時間 { get; set; }
        public int 戦闘 { get; set; }
        public int 領域 { get; set; }
        public int 支援 { get; set; }
        public int PC与ダメージ { get; set; }
        public int キルダメージボーナス { get; set; }
        public int 召喚解除ボーナス { get; set; }
        public int 建築与ダメージ { get; set; }
        public int 領域破壊ボーナス { get; set; }
        public int 領域ダメージボーナス { get; set; }
        public int 貢献度 { get; set; }
        public int クリスタル運用ボーナス { get; set; }
        public int 召喚行動ボーナス { get; set; }
        public int キル数 { get; set; }
        public int デッド数 { get; set; }
        public int 建築数 { get; set; }
        public int 建築物破壊数 { get; set; }
        public int クリスタル採掘量 { get; set; }
    }
}
