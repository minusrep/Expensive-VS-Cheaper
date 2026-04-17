using System;
using DoubleB.Runtime.Runtime.Descriptions;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemModel
    {
        public event Action OnChangePosition;
        
        public event Action OnChangeDescription;

        public ItemDescription Description
        {
            get => _description;
            set
            {
                _description = value;
                
                OnChangeDescription?.Invoke();
            }
        }

        public int Position
        {
            get => _positon;
            set
            {
                if (_positon == value) return;

                _positon = value;
                OnChangePosition?.Invoke();
            }
        }

        private ItemDescription _description;
        private int _positon;
        
        
        public ItemModel(ItemDescription description)
        {
            Description = description;
        }
    }
}