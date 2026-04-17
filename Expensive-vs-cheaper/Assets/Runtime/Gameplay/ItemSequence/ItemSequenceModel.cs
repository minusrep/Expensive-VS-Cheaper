using System;
using DoubleB.Runtime.Runtime.Descriptions;

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

        public void Next()
        {
            var first = Items[0];

            for (var i = 0; i < Items.Length - 1; i++)
            {
                Items[i] = Items[i + 1];
                Items[i].Position = Description.GetViewPosition(i);
            }

            Items[^1] = first;
            Items[^1].Position = Description.GetViewPosition(Items.Length - 1);

            first.Description = Description.Items.GetRandom();
            
            OnChange?.Invoke();
        }
    }
}