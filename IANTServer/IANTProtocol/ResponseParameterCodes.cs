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
        FirstNestSensitivity,
        FirstNestDistributionMap1,
        FirstNestDistributionMap2,
        FirstNestDistributionMap3,
        DefenceDataString,
        UsedDefenceBudget
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
    public enum GetChallengePlayerListResponseParameterCode : byte
    {
        FacebookIDArray,
        LevelArray,
        NestLevelArray
    }
    public enum StartChallengeGameResponseParameterCode : byte
    {
        UsedCakeNumber,
        NestDuration,
        NestSpeed,
        NestResistant,
        NestPopulation,
        NestSensitivity,
        NestDistributionMap1,
        NestDistributionMap2,
        NestDistributionMap3
    }
    public enum GetHarvestPlayerListResponseParameterCode : byte
    {
        FacebookIDArray,
        LevelArray,
        HarvestableCakeNumberArray
    }
    public enum StartHarvestGameResponseParameterCode : byte
    {
        UsedCakeNumber,
        HarvestTargetDefenceDataString,
        HarvestableCakeNumber
    }
}
