using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FishManager : MonoBehaviour
{
    // A static list that can be accessed from any other script in the game.
    public static List<FishData> allFish;

    void Awake()
    {
        var loadedFish = Resources.LoadAll<FishData>("Fish/Kepler");
        allFish = new List<FishData>(loadedFish);
        foreach (var fish in loadedFish)
        {
            if (!fish.isAvailable)
            {
                allFish.Remove(fish);
            }
        }
        allFish = allFish.OrderBy(fish => fish.fishName).ToList();
        Debug.Log("Loaded " + allFish.Count + " fish from Resources folder.");
    }
}