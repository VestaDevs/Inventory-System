using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlotParent;
    private readonly InventoryManager<InventorySlots, ItemSO> inventoryManager = new InventoryManager<InventorySlots, ItemSO>();
    private readonly List<InventorySlots> inventorySlots = new List<InventorySlots>();
    private void Awake()
    {
        InitializeSlot();
    }


    private void InitializeSlot()
    {
        inventorySlots.Clear();

        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<InventorySlots>(true));
        inventoryManager.RegisterSlots(inventorySlots);

    }

    public int Additem(ItemSO item, int amount)
    {
        return inventoryManager.AddItem(item, amount);
    }

}
