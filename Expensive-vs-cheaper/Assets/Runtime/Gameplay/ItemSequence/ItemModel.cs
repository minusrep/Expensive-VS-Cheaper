using System;
using DoubleB.Runtime.Runtime.Descriptions;
using UniRx;
using UnityEngine;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemModel
    {
        public event Action OnChangeDescription;

        public ReactiveProperty<int> Position { get; private set; } = new ReactiveProperty<int>();
        
        public ReactiveProperty<Color>  Color { get; private set; } = new ReactiveProperty<Color>();
        
        public ItemDescription Description
        {
            get => _description;
            set
            {
                _description = value;
                
                OnChangeDescription?.Invoke();
            }
        }

        private ItemDescription _description;
        
        public ItemModel(ItemDescription description)
        {
            Description = description;
        }
    }
}