namespace IANTProtocol
{
    public enum LoginParameterCode : byte
    {
        FacebookUserID,
        AccessToken
    }
    public enum UpgradeNestParameterCode : byte
    {
        Direction
    }
    public enum StartGameParameterCode : byte
    {
        UsedCakeNumber
    }
    public enum GameOverParameterCode : byte
    {
        FinalWaveNumber,
        NestDistributionMap1,
        NestDistributionMap2,
        NestDistributionMap3
    }
    public enum GetChallengePlayerListParameterCode : byte
    {
        FriendsFacebookIDArray
    }
    public enum ChallengeGameParameterCode : byte
    {
        ChallengeFacebookID,
        UsedCakeNumber
    }
    public enum SetDefenceParameterCode : byte
    {
        DefenceDataString,
        UsedBudget
    }
    public enum GetHarvestPlayerListParameterCode : byte
    {
        FriendsFacebookIDArray
    }
    public enum HarvestGameParameterCode : byte
    {
        HarvestFacebookID,
        UsedCakeNumber,
        HarvestableCakeNumber
    }
    public enum HarvestGameOverParameterCode : byte
    {
        FinalWaveNumber,
        DefenderFacebookID,
        HarvestCount
    }
}
