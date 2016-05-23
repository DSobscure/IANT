using System;

namespace Managers
{
    public class SystemInformManager
    {
        private event Action<string> onDebugReturn;
        private event Action<string> onAlert;
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

        public void RegistrAlertInformFunction(Action<string> alertFunction)
        {
            onAlert += alertFunction;
        }
        public void EraseAlertInformFunction(Action<string> alertFunction)
        {
            onAlert -= alertFunction;
        }
        public void CallAlert(string message)
        {
            if (onAlert != null)
            {
                onAlert(message);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
