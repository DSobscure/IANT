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
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)StartGameParameterCode.UsedCakeNumber, usedCakeNumber }
        };
        IANTGame.Service.SendOperation(OperationCode.StartGame, parameter);
    }
    public void GameOver(int finalWave)
    {
        var parameter = new Dictionary<byte, object>()
        {
            { (byte)GameOverParameterCode.FinalWaveNumber, finalWave }
        };
        IANTGame.Service.SendOperation(OperationCode.GameOver, parameter);
    }
}
