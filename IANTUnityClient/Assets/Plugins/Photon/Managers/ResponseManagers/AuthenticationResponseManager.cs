using System;

public class AuthenticationResponseManager
{
    private event Action onLoginResponse;
    public void RegistrLoginResponseFunction(Action responseFunction)
    {
        onLoginResponse += responseFunction;
    }
    public void EraseLoginResponseFunction(Action responseFunction)
    {
        onLoginResponse -= responseFunction;
    }
    public void CallLoginResponse()
    {
        if (onLoginResponse != null)
        {
            onLoginResponse();
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
