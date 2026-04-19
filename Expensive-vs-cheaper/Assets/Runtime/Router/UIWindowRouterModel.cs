using System;

namespace DoubleB.Runtime.Runtime.Router
{
    public class UIWindowRouterModel
    {
        public event Action OnChangeState;
        
        public string CurrentState { get; private set; }
        
        public string PreviousState { get; private set; }

        public UIWindowRouterModel(string currentState)
        {
            CurrentState = currentState;
        }

        public void ChangeState(string newState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;
            OnChangeState?.Invoke();
        }
    }
}