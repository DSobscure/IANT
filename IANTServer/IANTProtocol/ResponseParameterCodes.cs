using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IANTProtocol
{
    public enum GetConfigurationsResponseParameterCode : byte
    {
        TowerUpgradeConfigurationXMLString
    }
    public enum LoginResponseParameterCode : byte
    {
        UniqueID,
        Level,
        EXP,
        LastTakeCakeTime,
        CakeCount,
        FirstNestDuration,
        FirstNestSpeed,
        FirstNestResistant,
        FirstNestPopulation,
        FirstNestSensitivity
    }
    public enum TakeCakeResponseParameterCode : byte
    {
        LastTakeCakeTime,
        CakeCount
    }
    public enum UpgradeNestResponseParameterCode : byte
    {
        CakeCount,
        Duration,
        Speed,
        Resistant,
        Population,
        Sensitivity
    }
    public enum StartGameResponseParameterCode : byte
    {
        UsedCakeNumber
    }
    public enum GameOverResponseParameterCode : byte
    {
        Level,
        EXP
    }
}
