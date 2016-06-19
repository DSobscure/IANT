using UnityEngine;
using System.Collections.Generic;
using IANTProtocol;
using IANTLibrary;

public class OperationManager
{
    public void TakeCake()
    {
        var parameter = new Dictionary<byte, object>();
        IANTGame.Service.SendOperation(OperationCode.TakeCake, parameter);
    }
    public void UpgradeNest(AntGrowthDirection direction)
    {
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)UpgradeNestParameterCode.Direction, direction }
        };
        IANTGame.Service.SendOperation(OperationCode.UpgradeNest, parameter);
    }
    public void StartGame(int usedCakeNumber)
    {
        if(usedCakeNumber > 0)
        {
            var parameter = new Dictionary<byte, object>()
            {
                { (byte)StartGameParameterCode.UsedCakeNumber, usedCakeNumber }
            };
            IANTGame.Service.SendOperation(OperationCode.StartGame, parameter);
        }
    }
    public void StartChallengeGame(long challengeFacebookID, int usedCakeNumber)
    {
        if (usedCakeNumber > 0)
        {
            var parameter = new Dictionary<byte, object>()
            {
                { (byte)ChallengeGameParameterCode.ChallengeFacebookID, challengeFacebookID },
                { (byte)ChallengeGameParameterCode.UsedCakeNumber, usedCakeNumber }
            };
            IANTGame.Service.SendOperation(OperationCode.ChallengeGame, parameter);
        }
    }
    public void GameOver(int finalWave)
    {
        string distributionMap1, distributionMap2, distributionMap3;
        IANTGame.Player.Nests[0].Serialize3DistributionMap(out distributionMap1, out distributionMap2, out distributionMap3);
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)GameOverParameterCode.FinalWaveNumber, finalWave },
            { (byte)GameOverParameterCode.NestDistributionMap1, distributionMap1 },
            { (byte)GameOverParameterCode.NestDistributionMap2, distributionMap2 },
            { (byte)GameOverParameterCode.NestDistributionMap3, distributionMap3 }
        };
        IANTGame.Service.SendOperation(OperationCode.GameOver, parameter);
    }
    public void HarvestGameOver(int finalWave, long defenderFacebookID, int harvestCount)
    {
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)HarvestGameOverParameterCode.FinalWaveNumber, finalWave },
            { (byte)HarvestGameOverParameterCode.DefenderFacebookID, defenderFacebookID },
            { (byte)HarvestGameOverParameterCode.HarvestCount, harvestCount },
        };
        IANTGame.Service.SendOperation(OperationCode.HarvestGameOver, parameter);
    }
    public void SetDefence(TowerFactory towerFactory, int usedBudget)
    {
        IANTGame.Player.DefenceDataString = towerFactory.SerializeTowers();
        IANTGame.Player.UsedDefenceBudget = usedBudget;
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)SetDefenceParameterCode.DefenceDataString, IANTGame.Player.DefenceDataString },
            { (byte)SetDefenceParameterCode.UsedBudget, usedBudget }
        };
        IANTGame.Service.SendOperation(OperationCode.SetDefence, parameter);
    }
    public void StartHarvestGame(long harvestFacebookID, int usedCakeNumber, int harvestableCakeNumber)
    {
        if (usedCakeNumber > 0)
        {
            var parameter = new Dictionary<byte, object>()
            {
                { (byte)HarvestGameParameterCode.HarvestFacebookID, harvestFacebookID },
                { (byte)HarvestGameParameterCode.UsedCakeNumber, usedCakeNumber },
                { (byte)HarvestGameParameterCode.HarvestableCakeNumber, harvestableCakeNumber }
            };
            IANTGame.Service.SendOperation(OperationCode.HarvestGame, parameter);
        }
    }
}
