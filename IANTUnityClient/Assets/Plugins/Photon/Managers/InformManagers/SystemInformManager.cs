using System;

namespace Managers
{
    public class SystemInformManager
    {
        private event Action<string> onDebugReturn;
        public void RegistrDebugReturnInformFunction(Action<string> informFunction)
        {
            onDebugReturn += informFunction;
        }
        public void EraseDebugReturnInformFunction(Action<string> informFunction)
        {
            onDebugReturn -= informFunction;
        }
        public void CallDebugReturn(string message)
        {
            if (onDebugReturn != null)
            {
                onDebugReturn(message);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
