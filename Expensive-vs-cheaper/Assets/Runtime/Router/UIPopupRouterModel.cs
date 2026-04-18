using System;

namespace DoubleB.Runtime
{
    public class UIPopupRouterModel 
    {
        public event Action OnChangeState;
        
        public string CurrentState { get; private set; }

        public void Invoke(string state)
        {
            CurrentState = state;
            OnChangeState?.Invoke();
        }

        public void Hide()
        {
            CurrentState = string.Empty;
            OnChangeState?.Invoke();
        }
    }
}