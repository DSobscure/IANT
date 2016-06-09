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
        FinalWaveNumber
    }
}
