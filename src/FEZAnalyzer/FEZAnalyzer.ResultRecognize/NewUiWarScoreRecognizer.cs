using FEZAnalyzer.Entity;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FEZAnalyzer.PerformanceTest")]
namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class NewUiWarScoreRecognizer
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

        public bool TryRecognize(FezImage fezImage, out WarScore warScore)
        {
            using (var img = newUiWarScoreClipper.Clip(fezImage))
            {
                if (img == null)
                {
                    warScore = null;
                    return false;
                }

                var w = new WarScore();
                w.記録日時 = DateTime.Now;
                Task.WaitAll(
                    Task.Run(() => { w.勝敗 = warResultScanner.Scan(img); }),
                    Task.Run(() => { w.参戦側 = sideScanner.Scan(img); }),
                    Task.Run(() => { w.攻撃側国 = offenceCountryScanner.Scan(img); }),
                    Task.Run(() => { w.防衛側国 = defenceCountryScanner.Scan(img); }),
                    Task.Run(() => { w.戦争継続時間 = warTimeScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.戦闘 = attackTotalScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.領域 = regionTotalScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.支援 = supportTotalScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.PC与ダメージ = playerDamageDetailScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.キルダメージボーナス = killDamasgeDetailScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.召喚解除ボーナス = summonReleaseDetailScorer.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.建築与ダメージ = buildingDamageDetailScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.領域破壊ボーナス = regionDestroyDetailScorer.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.領域ダメージボーナス = regionDamageDetailScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.貢献度 = contributionDetailScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.クリスタル運用ボーナス = crystalOperationDetailScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.召喚行動ボーナス = summonActionDetailScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.キル数 = killCountScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.デッド数 = deadCountScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.建築数 = buildingCountScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.建築物破壊数 = buildingDestroyCountScanner.Scan(img) ?? throw new Exception(); }),
                    Task.Run(() => { w.クリスタル採掘量 = crystalCountScanner.Scan(img) ?? throw new Exception(); })
                );
                warScore = w;
            }

            return true;
        }
    }
}
