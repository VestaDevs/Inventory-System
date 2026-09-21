using VestaDev.Inventory.Utilities;
using System.Collections.Generic;
using System;

public class InventoryManager<TSlot, TItem>
where TSlot : class, IInventorySlot<TItem>
where TItem : class, IInventoryItem
{
    private readonly Dictionary<int, TSlot> slotsMap = new Dictionary<int, TSlot>();

    // Register slot list; assigning each slot an integer index starting from 0
    public void RegisterSlots(IEnumerable<TSlot> slots)
    {
        slotsMap.Clear();

        int index = 0;
        foreach (var slot in slots)
        {
            slotsMap[index] = slot;
            index++;
        }
    }

    // Fetch the slot at specific index from dictionary if no item there, return null
    public TSlot GetSlot(int index)
    {
        return slotsMap.TryGetValue(index, out TSlot slot) ? slot : null;
    }

    public IReadOnlyDictionary<int, TSlot> GetAllSlots() => slotsMap;

    // Add item into the dictionary list
    public int AddItem(TItem itemToAdd, int amount)
    {
        if (itemToAdd == null || amount <= 0) return amount;
        int remaining = amount;

        // First, try adding into existing stack
        remaining = TryAddToExistingStacks(itemToAdd, remaining);
        if (remaining <= 0) return 0;

        // Second, try adding to empty slots
        remaining = TryAddToEmptySlots(itemToAdd, remaining);
        return remaining;
    }

    public void HandleDraggedItem(TSlot fromSlot, TSlot toSlot)
    {
        if (fromSlot == null || toSlot == null || fromSlot == toSlot)
            return;

        // Merge/Stack Logic: Both slots hold the same item type
        if (toSlot.HasItem() && IsSameItem(toSlot.GetItem(), fromSlot.GetItem()))
        {
            int maxStack = toSlot.GetItem().MaxStackSize;
            int availableSpace = maxStack - toSlot.GetAmount();

            if (availableSpace > 0)
            {
                int amountToMove = Math.Min(availableSpace, fromSlot.GetAmount());
                toSlot.SetItem(toSlot.GetItem(), toSlot.GetAmount() + amountToMove);

                int remainingInFromSlot = fromSlot.GetAmount() - amountToMove;
                if (remainingInFromSlot <= 0)
                {
                    fromSlot.Clear();
                }
                else
                {
                    fromSlot.SetItem(fromSlot.GetItem(), remainingInFromSlot);
                }
                return;
            }
        }

        // Swap Logic: Target slot has a different item or target stack is full
        if (toSlot.HasItem())
        {
            TItem tempItem = toSlot.GetItem();
            int tempAmount = toSlot.GetAmount();

            toSlot.SetItem(fromSlot.GetItem(), fromSlot.GetAmount());
            fromSlot.SetItem(tempItem, tempAmount);
            return;
        }

        // Move Logic: Target slot is completely empty
        toSlot.SetItem(fromSlot.GetItem(), fromSlot.GetAmount());
        fromSlot.Clear();
    }

    public void ClearSlot(int index)
    {
        if (slotsMap.TryGetValue(index, out TSlot slot))
        {
            slot.Clear();
            slot.SetUnOccupied();
        }
    }

    #region Helper Functions
    //Check whether they are same SO Assets 
    private bool IsSameItem(TItem a, TItem b)
    {
        if (a == null || b == null) return false;
        return ReferenceEquals(a, b);
    }

    // Helper Function to Add item into empty slot
    private int TryAddToEmptySlots(TItem itemToAdd, int remaining)
    {
        foreach (var kvp in slotsMap)
        {
            TSlot slot = kvp.Value;
            if (!slot.HasItem() && !slot.IsOccupied())
            {
                int amountToPlace = Math.Min(itemToAdd.MaxStackSize, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;
                if (remaining <= 0) break;
            }
        }
        return remaining;
    }

    // Helper Function to Add item into existing stack
    private int TryAddToExistingStacks(TItem itemToAdd, int remaining)
    {
        foreach (var kvp in slotsMap)
        {
            TSlot slot = kvp.Value;
            if (slot.HasItem() && IsSameItem(slot.GetItem(), itemToAdd) && !slot.IsOccupied())
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.MaxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Math.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

                    if (remaining <= 0) break;
                }
            }
        }
        return remaining;
    }
    #endregion
}
