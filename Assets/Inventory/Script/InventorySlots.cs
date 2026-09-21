using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VestaDev.Inventory.Utilities;

public class InventorySlots : MonoBehaviour, IInventorySlot<ItemSO>
{

    [Header(" UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemAmountText;

    private readonly InventorySlotUtility<ItemSO> utility = new InventorySlotUtility<ItemSO>();
    private void Awake()
    {
        utility.OnItemChanged += HandleItemChanged;
        utility.OnSlotCleared += HandleSlotCleared;
    }
    private void OnDestroy()
    {
        utility.OnItemChanged -= HandleItemChanged;
        utility.OnSlotCleared -= HandleSlotCleared;
    }

    private void HandleSlotCleared(object sender, EventArgs e)
    {
        UpdateDisplay();
    }

    private void HandleItemChanged(object sender, ItemSlotChangedEventArgs<ItemSO> e)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (utility.HasItem())
        {
            iconImage.enabled = true;
            iconImage.sprite = utility.GetItem().ItemIcon();
            itemAmountText.text = utility.GetAmount().ToString();

            int amount = utility.GetAmount();
            itemAmountText.text = amount > 1 ? amount.ToString() : "";
        }
        else
        {
            iconImage.enabled = false;
            itemAmountText.text = "";
        }
    }

    public void Clear()
    {
        utility.Clear();
    }

    public int GetAmount()
    {
        return utility.GetAmount();
    }

    public ItemSO GetItem()
    {
        return utility.GetItem();
    }

    public IInventorySlot<ItemSO> GetRootSlot()
    {
        return utility.GetRootSlot();
    }

    public bool HasItem()
    {
        return utility.HasItem();
    }

    public bool IsOccupied()
    {
        return utility.IsOccupied();
    }

    public void SetItem(ItemSO item, int amount)
    {
        utility.SetItem(item, amount);
    }

    public void SetOccupied(IInventorySlot<ItemSO> rootSlot)
    {
        utility.SetOccupied(rootSlot);
    }

    public void SetUnOccupied()
    {
        utility.SetUnOccupied();
    }


}
