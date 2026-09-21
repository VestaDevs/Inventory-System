using UnityEngine;

public class InventoryTesting : MonoBehaviour
{
    [SerializeField] private ItemSO testItemSO;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int testAmount = 5;

    void Update()
    {
        // Space Key နှိပ်ပါက Item Prefab ကို World ထဲသို့ Spawn လုပ်မည်
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (testItemSO != null && testItemSO.ItemPrefab() != null)
            {
                Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

                // Prefab ကို Instantitate လုပ်ခြင်း
                GameObject spawnedObject = Instantiate(testItemSO.ItemPrefab(), spawnPos, Quaternion.identity);

                // Prefab ထဲရှိ Items Component ထဲသို့ SO နှင့် Amount ထည့်ပေးခြင်း
                if (spawnedObject.TryGetComponent<Items>(out Items worldItem))
                {
                    worldItem.item = testItemSO;
                    worldItem.amount = testAmount;
                    Debug.Log($"[Spawned] {worldItem.amount}x {testItemSO.ItemName()} in World!");
                }
                else
                {
                    Debug.LogWarning("Spawned Prefab does not have 'Items' component attached!");
                }
            }
        }
    }
}
