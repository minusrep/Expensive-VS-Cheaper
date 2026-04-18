using System;

namespace DoubleB.Runtime
{
    public class UIRouterModel
    {
        public UIWindowRouterModel WindowRouterModel { get; private set; }
        public UIPopupRouterModel PopupRouterModel { get; private set; }

        public UIRouterModel(string startWindow)
        {
            WindowRouterModel = new UIWindowRouterModel(startWindow);
            PopupRouterModel = new UIPopupRouterModel();
        }
    }

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