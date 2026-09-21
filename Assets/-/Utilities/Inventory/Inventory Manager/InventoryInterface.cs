namespace VestaDev.Inventory.Utilities
{
    public interface IInventoryItem
    {
        int SlotSize { get; }
        int MaxStackSize { get; }
    }

    public interface IInventorySlot<TItem> where TItem : class, IInventoryItem
    {
        TItem GetItem();
        int GetAmount();
        bool HasItem();
        bool IsOccupied();

        IInventorySlot<TItem> GetRootSlot();

        void SetItem(TItem item, int amount);
        void SetOccupied(IInventorySlot<TItem> rootSlot);
        void SetUnOccupied();
        void Clear();
    }
}

