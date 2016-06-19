using System;

public class OperationResponseManager
{
    private event Action<int,DateTime> onTakeCakeResponse;
    public void RegistrTakeCakeResponseFunction(Action<int, DateTime> responseFunction)
    {
        onTakeCakeResponse += responseFunction;
    }
    public void EraseTakeCakeResponseFunction(Action<int, DateTime> responseFunction)
    {
        onTakeCakeResponse -= responseFunction;
    }
    public void CallTakeCakeResponse(int cakeCount, DateTime lastTime)
    {
        if (onTakeCakeResponse != null)
        {
            onTakeCakeResponse(cakeCount, lastTime);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private event Action<int> onStartGameResponse;
    public void RegistrStartGameResponseFunction(Action<int> responseFunction)
    {
        onStartGameResponse += responseFunction;
    }
    public void EraseStartGameResponseFunction(Action<int> responseFunction)
    {
        onStartGameResponse -= responseFunction;
    }
    public void CallStartGameResponse(int usedCakeNumber)
    {
        if (onStartGameResponse != null)
        {
            onStartGameResponse(usedCakeNumber);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private event Action<int> onStartChallengeGameResponse;
    public void RegistrStartChallengeGameResponseFunction(Action<int> responseFunction)
    {
        onStartChallengeGameResponse += responseFunction;
    }
    public void EraseStartChallengeGameResponseFunction(Action<int> responseFunction)
    {
        onStartChallengeGameResponse -= responseFunction;
    }
    public void CallStartChallengeGameResponse(int usedCakeNumber)
    {
        if (onStartChallengeGameResponse != null)
        {
            onStartChallengeGameResponse(usedCakeNumber);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private event Action onSetDefenceResponse;
    public void RegistrSetDefenceResponseFunction(Action responseFunction)
    {
        onSetDefenceResponse += responseFunction;
    }
    public void EraseSetDefenceResponseFunction(Action responseFunction)
    {
        onSetDefenceResponse -= responseFunction;
    }
    public void CallSetDefenceResponse()
    {
        if (onSetDefenceResponse != null)
        {
            onSetDefenceResponse();
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private event Action<int,string,int> onStartHarvestGameResponse;
    public void RegistrStartHarvestGameResponseFunction(Action<int, string, int> responseFunction)
    {
        onStartHarvestGameResponse += responseFunction;
    }
    public void EraseStartHarvestGameResponseFunction(Action<int, string, int> responseFunction)
    {
        onStartHarvestGameResponse -= responseFunction;
    }
    public void CallStartHarvestGameResponse(int usedCakeNumber, string harvestDefenceDataString, int harvestableCakeNumber)
    {
        if (onStartHarvestGameResponse != null)
        {
            onStartHarvestGameResponse(usedCakeNumber, harvestDefenceDataString, harvestableCakeNumber);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
