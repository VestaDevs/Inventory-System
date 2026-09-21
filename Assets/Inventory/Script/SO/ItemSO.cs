using UnityEngine;
using VestaDev.Inventory.Utilities;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory/Items")]
public class ItemSO : ScriptableObject, IInventoryItem

{
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName;
    [SerializeField] private int slotSize = 1;
    [SerializeField] private int maxStackSize = 99;
    [SerializeField] private GameObject itemPrefab;

    #region Interface Properties
    int IInventoryItem.SlotSize => slotSize;
    int IInventoryItem.MaxStackSize => maxStackSize;
    #endregion

    // Getter functions
    public Sprite ItemIcon() => itemIcon;
    public string ItemName() => itemName;
    public GameObject ItemPrefab() => itemPrefab;
}
