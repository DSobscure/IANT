using UnityEngine;
using IANTLibrary;
using Managers;
using System;

public class UpgradeNestController : MonoBehaviour
{
    public void UpgradeNest(int directionNumber)
    {
        AntGrowthDirection direction = (AntGrowthDirection)directionNumber;
        IANTGame.ActionManager.OperationManager.UpgradeNest(direction);
    }
}
