using System;

namespace DoubleB.Runtime.Runtime.Router
{
    public class UIRouterModel
    {
        public UIWindowRouterModel WindowRouterModel { get; private set; }
        public UIPopupRouterModel PopupRouterModel { get; private set; }

        public event Action OnRewardedContinueRequested;
        
        public event Action OnRestartRequested;

        public event Action OnExitToMainMenuRequested;

        public UIRouterModel(string startWindow)
        {
            WindowRouterModel = new UIWindowRouterModel(startWindow);
            PopupRouterModel = new UIPopupRouterModel();
        }

        public void RequestRestart()
        {
            OnRestartRequested?.Invoke();
        }

        public void RequestExitToMainMenu()
        {
            OnExitToMainMenuRequested?.Invoke();
        }

        public void RequestRewardedContinue()
        {
            OnRewardedContinueRequested?.Invoke();
        }
    }
}