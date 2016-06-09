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
}
