using System;
namespace VestaDev.Inventory.Utilities
{

    //Custmn Event Arguments trigged when slot content / quantity change
    public class ItemSlotChangedEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; set; }
        public int Amount { get; set; }
        public bool HasItem { get; set; }
    }
    public class InventorySlotUtility<TItem> where TItem : class, IInventoryItem
    {
        private TItem heldItem;
        private int itemAmount;

        private bool isOccupied = false;

        private IInventorySlot<TItem> rootSlot = null;

        //Events
        public event EventHandler<ItemSlotChangedEventArgs<TItem>> OnItemChanged;
        public event EventHandler OnSlotCleared;
        public event EventHandler OnOccupiedChanged;
        //Getter Functions
        public bool HasItem() => heldItem != null;
        public int GetAmount() => itemAmount;
        public TItem GetItem() => heldItem;
        public bool IsOccupied() => isOccupied;
        public IInventorySlot<TItem> GetRootSlot() => rootSlot;

        // This function assign a new item and initialize quantity to the slot
        public void SetItem(TItem item, int amount)
        {
            heldItem = item;
            itemAmount = amount;
            RaiseItemChangedEvent();
        }
        //This function increase item stack count
        public int AddAmount(int amountToAdd)
        {
            itemAmount += amountToAdd;
            RaiseItemChangedEvent();
            return itemAmount;
        }
        //This function reduce item stack count
        public int RemoveAmount(int amountToRemove)
        {
            itemAmount -= amountToRemove;

            if (itemAmount <= 0)
            {
                Clear();
            }
            else
            {
                RaiseItemChangedEvent();
            }
            return itemAmount;
        }

        //This function mark slot as OCCUPIED
        public void SetOccupied(IInventorySlot<TItem> slot)
        {
            isOccupied = true;
            rootSlot = slot;
            OnOccupiedChanged?.Invoke(this, EventArgs.Empty);
        }

        //This function mark slot as UNOCCUPIED
        public void SetUnOccupied()
        {
            isOccupied = false;
            rootSlot = null;
            OnOccupiedChanged?.Invoke(this, EventArgs.Empty);
        }

        //This Function Clear current holding item and reset slot to zero (null)
        public void Clear()
        {
            heldItem = null;
            itemAmount = 0;
            OnSlotCleared?.Invoke(this, EventArgs.Empty);

        }

        //This function will notify subscribers of teh updated item details
        private void RaiseItemChangedEvent()
        {
            OnItemChanged?.Invoke(this, new ItemSlotChangedEventArgs<TItem>
            {
                Item = heldItem,
                Amount = itemAmount,
                HasItem = HasItem()
            });

        }
    }
}

