using System;

namespace DoubleB.Runtime.Gameplay
{
    public class GameplayModel
    {
        public event Action OnLose;
        
        public ItemSequenceModel ItemSequence { get; set; }
        
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
    }
}