using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private FishingRodController fishingRodController;

    void Update()
    {
        switch (GameManager.instance.CurrentState)
        {
            case GameManager.GameState.Playing:
                HandleGameplayInput();
                break;
            
            case GameManager.GameState.InventoryOpen:
                HandleInventoryInput();
                break;
        }
    }

    private void HandleGameplayInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            fishingRodController.PlayerInteract();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryUI.OpenInventory();
        }
    }

    private void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryUI.CloseInventory();
        }
    }
}