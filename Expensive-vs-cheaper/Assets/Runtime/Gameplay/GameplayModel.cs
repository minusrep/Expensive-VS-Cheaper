using System;
using DoubleB.Runtime.Runtime.Gameplay.ItemChoicer;
using DoubleB.Runtime.Runtime.Gameplay.ItemSequence;
using UniRx;

namespace DoubleB.Runtime.Runtime.Gameplay
{
    public class GameplayModel
    {
        public event Action OnLose;

        public event Action OnScoreChange;
        
        public event Action OnHighScoreChange;
        
        public event Action<ItemChoice> OnSelect;

        public event Action<ItemChoiceResult> OnGetResult;
        
        public event Action<bool> OnInteractionChange;
        
        public ItemSequenceModel ItemSequence { get; set; }
        
        public bool CanInteract { get; private set; }

        public int Score { get;  private set; }
        
        public int HighScore { get; private set; }
        
        public GameplayModel(ItemSequenceModel itemSequence)
        {
            ItemSequence = itemSequence;
        }

        public void Reset()
        {
            ItemSequence.Reset();
        }

        public void Lose()
        {
            OnLose?.Invoke();
        }

        public void LockInteraction()
        {
            CanInteract = false;
            OnInteractionChange?.Invoke(CanInteract);
        }

        public void AddScore()
        {
            Score++;

            if (HighScore < Score)
            {
                HighScore = Score;
                OnHighScoreChange?.Invoke();
            }
            
            OnScoreChange?.Invoke();
        }

        public void UnlockInteraction()
        {
            CanInteract = true;
            OnInteractionChange?.Invoke(CanInteract);
        }

        public void SelectMoreExpensive()
        {
            OnSelect?.Invoke(ItemChoice.MoreExpensive);
        }

        public void SelectCheaper()
        {
            OnSelect?.Invoke(ItemChoice.Cheaper);
        }

        public void RegisterResult(ItemChoiceResult result)
        {
            OnGetResult?.Invoke(result);
        }
    }
}