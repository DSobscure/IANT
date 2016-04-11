using System;

namespace Managers
{
    public class SystemResponseManager
    {
        private event Action<bool> onConnectResponse;
        public void RegistrConnectResponseFunction(Action<bool> responseFunction)
        {
            onConnectResponse += responseFunction;
        }
        public void EraseConnectResponseFunction(Action<bool> responseFunction)
        {
            onConnectResponse -= responseFunction;
        }
        public void CallConnectResponse(bool status)
        {
            if (onConnectResponse != null)
            {
                onConnectResponse(status);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
