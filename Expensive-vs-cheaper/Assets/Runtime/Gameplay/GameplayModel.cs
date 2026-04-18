using System;
using UniRx;

namespace DoubleB.Runtime.Gameplay
{
    public class GameplayModel
    {
        public event Action OnLose;

        public event Action<ItemChoice> OnSelected;
        
        public event Action<bool> OnInteractionChanged;
        
        public ItemSequenceModel ItemSequence { get; set; }
        
        public bool CanInteract { get; private set; }
        
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
            OnInteractionChanged?.Invoke(CanInteract);
        }

        public void UnlockInteraction()
        {
            CanInteract = true;
            OnInteractionChanged?.Invoke(CanInteract);
        }

        public void SelectMoreExpensive()
        {
            OnSelected?.Invoke(ItemChoice.MoreExpensive);
        }

        public void SelectCheaper()
        {
            OnSelected?.Invoke(ItemChoice.Cheaper);
        }
    }
}