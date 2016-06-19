using System;

namespace Managers
{
    public class GameInformManager
    {
        private event Action<int> onCakeNumberChange;
        public void RegistrCakeNumberChangeInformFunction(Action<int> informFunction)
        {
            onCakeNumberChange += informFunction;
        }
        public void EraseCakeNumberChangeInformFunction(Action<int> informFunction)
        {
            onCakeNumberChange -= informFunction;
        }
        public void CallCakeNumberChange(int message)
        {
            if (onCakeNumberChange != null)
            {
                onCakeNumberChange(message);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
