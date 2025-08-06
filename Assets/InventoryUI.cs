// InventoryUI.cs
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject fishSlotPrefab;
    [SerializeField] private FishInventoryDetailsPanel detailsPanel;
    
    private void OnEnable()
    {
        FishSlot.OnFishSlotClicked += HandleSlotSelection;
        DrawInventory();
    }

    private void OnDisable()
    {
        FishSlot.OnFishSlotClicked -= HandleSlotSelection;
        if(detailsPanel != null) detailsPanel.ClearDetails();
    }
    
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        GameManager.instance.SetState(GameManager.GameState.InventoryOpen);
        DrawInventory(); // Redraw inventory when opening
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        GameManager.instance.SetState(GameManager.GameState.Playing);
    }
    
    private void DrawInventory()
    {
        // Clear existing slots before drawing new ones
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Spawn a slot for each fish in the inventory data
        foreach (FishInstance fish in InventoryManager.fishInventory)
        {
            GameObject slotGO = Instantiate(fishSlotPrefab, contentParent);
            FishSlot slot = slotGO.GetComponent<FishSlot>();
            slot.SetData(fish);
        }
    }

    private void HandleSlotSelection(FishInstance selectedFish)
    {
        detailsPanel.DisplayFishDetails(selectedFish);
    }
}