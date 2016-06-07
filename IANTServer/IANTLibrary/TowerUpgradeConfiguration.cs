using System.IO;
using System.Xml.Serialization;

namespace IANTLibrary
{
    public static class TowerUpgradeConfiguration
    {
        private static TowerUpgradeConfigurationContent content;

        public static double UpgradeCostIncreaseRatio { get { return content.upgradeCostIncreaseRatio; } }
        public static int UpgradeDamageDelta { get { return content.upgradeDamageDelta; } }
        public static float UpgradeSpeedDelta { get { return content.upgradeSpeedDelta; } }
        public static float UpgradeRangeDelta { get { return content.upgradeRangeDelta; } }
        public static float UpgradeFrequencyDelta { get { return content.upgradeFrequencyDelta; } }
        public static int UpgradeBulletNumberDelta { get { return content.upgradeBulletNumberDelta; } }
        public static float UpgradeBulletSpanRangeDelta { get { return content.upgradeBulletSpanRangeDelta; } }

        public static double ElementUpgradeCostIncreaseRatio { get { return content.elementUpgradeCostIncreaseRatio; } }

        public static float IceTowerUpgradeDamageRatio { get { return content.iceTowerUpgradeDamageRatio; } }
        public static float IceTowerUpgradeRangeRatio { get { return content.iceTowerUpgradeRangeRatio; } }
        public static float IceTowerUpgradeFrequencyRatio { get { return content.iceTowerUpgradeFrequencyRatio; } }
        public static float IceTowerUpgradeSpeedRatio { get { return content.iceTowerUpgradeSpeedRatio; } }
        public static int IceTowerUpgradeBulletNumberDelta { get { return content.iceTowerUpgradeBulletNumberDelta; } }
        public static float IceTowerUpgradeBulletSpanRangeDelta { get { return content.iceTowerUpgradeBulletSpanRangeDelta; } }

        public static float FireTowerUpgradeDamageRatio { get { return content.fireTowerUpgradeDamageRatio; } }
        public static float FireTowerUpgradeRangeRatio { get { return content.fireTowerUpgradeRangeRatio; } }
        public static float FireTowerUpgradeFrequencyRatio { get { return content.fireTowerUpgradeFrequencyRatio; } }
        public static float FireTowerUpgradeSpeedRatio { get { return content.fireTowerUpgradeSpeedRatio; } }
        public static int FireTowerUpgradeBulletNumberDelta { get { return content.fireTowerUpgradeBulletNumberDelta; } }
        public static float FireTowerUpgradeBulletSpanRangeDelta { get { return content.fireTowerUpgradeBulletSpanRangeDelta; } }

        public static float ThunderTowerUpgradeDamageRatio { get { return content.thunderTowerUpgradeDamageRatio; } }
        public static float ThunderTowerUpgradeRangeRatio { get { return content.thunderTowerUpgradeRangeRatio; } }
        public static float ThunderTowerUpgradeFrequencyRatio { get { return content.thunderTowerUpgradeFrequencyRatio; } }
        public static float ThunderTowerUpgradeSpeedRatio { get { return content.thunderTowerUpgradeSpeedRatio; } }
        public static int ThunderTowerUpgradeBulletNumberDelta { get { return content.thunderTowerUpgradeBulletNumberDelta; } }
        public static float ThunderTowerUpgradeBulletSpanRangeDelta { get { return content.thunderTowerUpgradeBulletSpanRangeDelta; } }

        public static float WindTowerUpgradeDamageRatio { get { return content.windTowerUpgradeDamageRatio; } }
        public static float WindTowerUpgradeRangeRatio { get { return content.windTowerUpgradeRangeRatio; } }
        public static float WindTowerUpgradeFrequencyRatio { get { return content.windTowerUpgradeFrequencyRatio; } }
        public static float WindTowerUpgradeSpeedRatio { get { return content.windTowerUpgradeSpeedRatio; } }
        public static int WindTowerUpgradeBulletNumberDelta { get { return content.windTowerUpgradeBulletNumberDelta; } }
        public static float WindTowerUpgradeBulletSpanRangeDelta { get { return content.windTowerUpgradeBulletSpanRangeDelta; } }

        public static float PoisonTowerUpgradeDamageRatio { get { return content.poisonTowerUpgradeDamageRatio; } }
        public static float PoisonTowerUpgradeRangeRatio { get { return content.poisonTowerUpgradeRangeRatio; } }
        public static float PoisonTowerUpgradeFrequencyRatio { get { return content.poisonTowerUpgradeFrequencyRatio; } }
        public static float PoisonTowerUpgradeSpeedRatio { get { return content.poisonTowerUpgradeSpeedRatio; } }
        public static int PoisonTowerUpgradeBulletNumberDelta { get { return content.poisonTowerUpgradeBulletNumberDelta; } }
        public static float PoisonTowerUpgradeBulletSpanRangeDelta { get { return content.poisonTowerUpgradeBulletSpanRangeDelta; } }

        public static float WoodTowerUpgradeDamageRatio { get { return content.woodTowerUpgradeDamageRatio; } }
        public static float WoodTowerUpgradeRangeRatio { get { return content.woodTowerUpgradeRangeRatio; } }
        public static float WoodTowerUpgradeFrequencyRatio { get { return content.woodTowerUpgradeFrequencyRatio; } }
        public static float WoodTowerUpgradeSpeedRatio { get { return content.woodTowerUpgradeSpeedRatio; } }
        public static int WoodTowerUpgradeBulletNumberDelta { get { return content.woodTowerUpgradeBulletNumberDelta; } }
        public static float WoodTowerUpgradeBulletSpanRangeDelta { get { return content.woodTowerUpgradeBulletSpanRangeDelta; } }

        static TowerUpgradeConfiguration()
        {
            Load();
        }
        public static void Load()
        {
            if(File.Exists("config/TowerUpgrade.config"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TowerUpgradeConfigurationContent));
                FileStream fs = new FileStream("config/TowerUpgrade.config", FileMode.Open);
                try
                {
                    Load((TowerUpgradeConfigurationContent)serializer.Deserialize(fs));
                }
                catch(IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
                content = new TowerUpgradeConfigurationContent
                {
                    upgradeCostIncreaseRatio = 1.4,
                    upgradeDamageDelta = 2,
                    upgradeSpeedDelta = 40,
                    upgradeRangeDelta = 30,
                    upgradeFrequencyDelta = 0.4f,
                    upgradeBulletNumberDelta = 1,
                    upgradeBulletSpanRangeDelta = 10,

                    elementUpgradeCostIncreaseRatio = 1.9,

                    iceTowerUpgradeDamageRatio = 1,
                    iceTowerUpgradeRangeRatio = 1,
                    iceTowerUpgradeFrequencyRatio = 1,
                    iceTowerUpgradeSpeedRatio = 1,
                    iceTowerUpgradeBulletNumberDelta = 0,
                    iceTowerUpgradeBulletSpanRangeDelta = 1,

                    fireTowerUpgradeDamageRatio = 1.3f,
                    fireTowerUpgradeRangeRatio = 1,
                    fireTowerUpgradeFrequencyRatio = 1.5f,
                    fireTowerUpgradeSpeedRatio = 1,
                    fireTowerUpgradeBulletNumberDelta = 0,
                    fireTowerUpgradeBulletSpanRangeDelta = 0,

                    thunderTowerUpgradeDamageRatio = 1.6f,
                    thunderTowerUpgradeRangeRatio = 1,
                    thunderTowerUpgradeFrequencyRatio = 0.9f,
                    thunderTowerUpgradeSpeedRatio = 1.3f,
                    thunderTowerUpgradeBulletNumberDelta = 0,
                    thunderTowerUpgradeBulletSpanRangeDelta = 0,

                    windTowerUpgradeDamageRatio = 1,
                    windTowerUpgradeRangeRatio = 1.3f,
                    windTowerUpgradeFrequencyRatio = 1,
                    windTowerUpgradeSpeedRatio = 1,
                    windTowerUpgradeBulletNumberDelta = 0,
                    windTowerUpgradeBulletSpanRangeDelta = 0,

                    poisonTowerUpgradeDamageRatio = 1,
                    poisonTowerUpgradeRangeRatio = 1,
                    poisonTowerUpgradeFrequencyRatio = 0.9f,
                    poisonTowerUpgradeSpeedRatio = 1,
                    poisonTowerUpgradeBulletNumberDelta = 1,
                    poisonTowerUpgradeBulletSpanRangeDelta = 1,

                    woodTowerUpgradeDamageRatio = 2,
                    woodTowerUpgradeRangeRatio = 1,
                    woodTowerUpgradeFrequencyRatio = 1,
                    woodTowerUpgradeSpeedRatio = 1,
                    woodTowerUpgradeBulletNumberDelta = 0,
                    woodTowerUpgradeBulletSpanRangeDelta = 0
                };
                XmlSerializer serializer = new XmlSerializer(typeof(TowerUpgradeConfigurationContent));
                TextWriter writer = new StreamWriter("TowerUpgrade.config");
                serializer.Serialize(writer, content);
                writer.Close();
            }
        }
        public static void Load(TowerUpgradeConfigurationContent content)
        {
            TowerUpgradeConfiguration.content = content;
        }
    }

    public struct TowerUpgradeConfigurationContent
    {
        #region upgrade
        public double upgradeCostIncreaseRatio;
        public int upgradeDamageDelta;
        public float upgradeSpeedDelta;
        public float upgradeRangeDelta;
        public float upgradeFrequencyDelta;
        public int upgradeBulletNumberDelta;
        public float upgradeBulletSpanRangeDelta;
        #endregion
        #region element upgrade
        public double elementUpgradeCostIncreaseRatio;
        #region ice tower
        public float iceTowerUpgradeDamageRatio;
        public float iceTowerUpgradeRangeRatio;
        public float iceTowerUpgradeFrequencyRatio;
        public float iceTowerUpgradeSpeedRatio;
        public int iceTowerUpgradeBulletNumberDelta;
        public float iceTowerUpgradeBulletSpanRangeDelta;
        #endregion
        #region fire tower
        public float fireTowerUpgradeDamageRatio;
        public float fireTowerUpgradeRangeRatio;
        public float fireTowerUpgradeFrequencyRatio;
        public float fireTowerUpgradeSpeedRatio;
        public int fireTowerUpgradeBulletNumberDelta;
        public float fireTowerUpgradeBulletSpanRangeDelta;
        #endregion
        #region thunder tower
        public float thunderTowerUpgradeDamageRatio;
        public float thunderTowerUpgradeRangeRatio;
        public float thunderTowerUpgradeFrequencyRatio;
        public float thunderTowerUpgradeSpeedRatio;
        public int thunderTowerUpgradeBulletNumberDelta;
        public float thunderTowerUpgradeBulletSpanRangeDelta;
        #endregion
        #region wind tower
        public float windTowerUpgradeDamageRatio;
        public float windTowerUpgradeRangeRatio;
        public float windTowerUpgradeFrequencyRatio;
        public float windTowerUpgradeSpeedRatio;
        public int windTowerUpgradeBulletNumberDelta;
        public float windTowerUpgradeBulletSpanRangeDelta;
        #endregion
        #region poison tower
        public float poisonTowerUpgradeDamageRatio;
        public float poisonTowerUpgradeRangeRatio;
        public float poisonTowerUpgradeFrequencyRatio;
        public float poisonTowerUpgradeSpeedRatio;
        public int poisonTowerUpgradeBulletNumberDelta;
        public float poisonTowerUpgradeBulletSpanRangeDelta;
        #endregion
        #region wood
        public float woodTowerUpgradeDamageRatio;
        public float woodTowerUpgradeRangeRatio;
        public float woodTowerUpgradeFrequencyRatio;
        public float woodTowerUpgradeSpeedRatio;
        public int woodTowerUpgradeBulletNumberDelta;
        public float woodTowerUpgradeBulletSpanRangeDelta;
        #endregion
        #endregion
    }
}
