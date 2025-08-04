using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject
{
    // --- Basic Info ---
    [Header("Basic Information")]
    public string fishName;
    [TextArea(3, 5)] // Makes the description box in the Inspector bigger
    public string description;
    public Sprite fishIcon;
    public FishRarity rarity;

    // --- Minigame Settings ---
    [Header("Minigame Settings")]
    public float fishWeight = 0.5f; // Progress Gain decreases with weight - Progress Drain increases with weight
    public float fishAgility = 0.5f; // Fish Speed increases with fishAgility - Fish moves more often with fishAgility
}

public enum FishRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
