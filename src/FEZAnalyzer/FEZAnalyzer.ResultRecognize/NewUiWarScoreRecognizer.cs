using FEZAnalyzer.Entity;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FEZAnalyzer.PerformanceTest")]
namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class NewUiWarScoreRecognizer : IWarScoreRecognize
    {
        private NewUiWarScoreClipper newUiWarScoreClipper = new NewUiWarScoreClipper();

        private WarResultScanner warResultScanner = new WarResultScanner();
        private SideScanner sideScanner = new SideScanner();
        private CountryScanner offenceCountryScanner = new CountryScanner(Side.Offence);
        private CountryScanner defenceCountryScanner = new CountryScanner(Side.Defence);
        private WarTimeScanner warTimeScanner = new WarTimeScanner();

        private AttackTotalScanner attackTotalScanner = new AttackTotalScanner();
        private RegionTotalScanner regionTotalScanner = new RegionTotalScanner();
        private SupportTotalScanner supportTotalScanner = new SupportTotalScanner();

        private PlayerDamageDetailScanner playerDamageDetailScanner = new PlayerDamageDetailScanner();
        private KillDamageDetailScanner killDamasgeDetailScanner = new KillDamageDetailScanner();
        private SummonReleaseDetailScanner summonReleaseDetailScorer = new SummonReleaseDetailScanner();
        private BuildingDamageDetailScanner buildingDamageDetailScanner = new BuildingDamageDetailScanner();
        private RegionDestroyDetailScanner regionDestroyDetailScorer = new RegionDestroyDetailScanner();
        private RegionDamageDetailScanner regionDamageDetailScanner = new RegionDamageDetailScanner();
        private ContributionDetailScanner contributionDetailScanner = new ContributionDetailScanner();
        private CrystalOperationDetailScanner crystalOperationDetailScanner = new CrystalOperationDetailScanner();
        private SummonActionDetailScanner summonActionDetailScanner = new SummonActionDetailScanner();

        private KillCountScanner killCountScanner = new KillCountScanner();
        private DeadCountScanner deadCountScanner = new DeadCountScanner();
        private BuildingCountScanner buildingCountScanner = new BuildingCountScanner();
        private BuildingDestroyCountScanner buildingDestroyCountScanner = new BuildingDestroyCountScanner();
        private CrystalCountScanner crystalCountScanner = new CrystalCountScanner();

        public WarScore Recognize(FezImage fezImage)
        {
            var warScore = new WarScore();

            using (var warResultImage = newUiWarScoreClipper.Clip(fezImage))
            {
                warScore.記録日時 = DateTime.Now;
                Task.WaitAll(
                    Task.Run(() => { warScore.勝敗 = warResultScanner.Scan(warResultImage); }),
                    Task.Run(() => { warScore.参戦側 = sideScanner.Scan(warResultImage); }),
                    Task.Run(() => { warScore.攻撃側国 = offenceCountryScanner.Scan(warResultImage); }),
                    Task.Run(() => { warScore.防衛側国 = defenceCountryScanner.Scan(warResultImage); }),
                    Task.Run(() => { warScore.戦争継続時間 = warTimeScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.戦闘 = attackTotalScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.領域 = regionTotalScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.支援 = supportTotalScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.PC与ダメージ = playerDamageDetailScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.キルダメージボーナス = killDamasgeDetailScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.召喚解除ボーナス = summonReleaseDetailScorer.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.建築与ダメージ = buildingDamageDetailScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.領域破壊ボーナス = regionDestroyDetailScorer.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.領域ダメージボーナス = regionDamageDetailScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.貢献度 = contributionDetailScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.クリスタル運用ボーナス = crystalOperationDetailScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.召喚行動ボーナス = summonActionDetailScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.キル数 = killCountScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.デッド数 = deadCountScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.建築数 = buildingCountScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.建築物破壊数 = buildingDestroyCountScanner.Scan(warResultImage) ?? throw new Exception(); }),
                    Task.Run(() => { warScore.クリスタル採掘量 = crystalCountScanner.Scan(warResultImage) ?? throw new Exception(); })
                );
            }

            return warScore;
        }
    }
}
