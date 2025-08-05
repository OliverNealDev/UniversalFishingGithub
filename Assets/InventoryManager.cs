using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public static List<FishInstance> fishInventory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            fishInventory = new List<FishInstance>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddFishToInventory(FishInstance caughtFish)
    {
        fishInventory.Add(caughtFish);
    }
}
