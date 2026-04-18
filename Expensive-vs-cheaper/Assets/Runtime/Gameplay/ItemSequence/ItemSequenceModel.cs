using System;
using Cysharp.Threading.Tasks;
using DoubleB.Runtime.Runtime.Descriptions;
using UnityEngine;

namespace DoubleB.Runtime.Gameplay
{
    public class ItemSequenceModel
    {
        public event Action OnChange;

        public ItemModel CurrentItem => Items[Description.CurrentIndex];
        public ItemModel NextItem => Items[Description.NextIndex];
        
        public ItemModel[] Items { get; private set; }
        
        public ItemSequenceDescription Description { get; private set; }
        
        public ItemSequenceModel(ItemSequenceDescription description)
        {
            Description = description;
            Items = new ItemModel[description.Capacity];
        }

        public void Reset()
        {
            Items = new ItemModel[Description.Capacity];
        }

        public void Next()
        {
            var first = Items[0];

            for (var i = 0; i < Items.Length - 1; i++)
            {
                Items[i] = Items[i + 1];
            }

            Items[^1] = first;

            first.Description = Description.Items.GetRandom();

            for (var i = 0; i < Items.Length; i++)
            {
                Items[i].Color.Value = Description.GetViewColor(i);
                Items[i].Position.Value = Description.GetViewPosition(i);
            }

            OnChange?.Invoke();
        }
    }
}