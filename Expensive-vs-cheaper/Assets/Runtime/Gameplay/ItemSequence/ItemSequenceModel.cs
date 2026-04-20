using System;
using DoubleB.Runtime.Runtime.Descriptions;

namespace DoubleB.Runtime.Runtime.Gameplay.ItemSequence
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
            CurrentItem.Description = Description.Items.GetRandom();
            NextItem.Description = Description.Items.GetRandom();
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