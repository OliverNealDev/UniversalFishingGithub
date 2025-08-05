using UnityEngine;

[System.Serializable]
public class FishInstance
{
    public FishData baseData;
    public float fishQuality;
    public int baseValue;

    public FishInstance(FishData caughtFishData, float postGameQuality)
    {
        baseData = caughtFishData;
        fishQuality = postGameQuality;

        float statValue = (0.1f + baseData.fishWeight + baseData.fishAgility) * 50f;
        
        float rarityMultiplier = 1f;
        switch (baseData.rarity)
        {
            case FishRarity.Uncommon:
                rarityMultiplier = 2f;
                break;
            case FishRarity.Rare:
                rarityMultiplier = 4f;
                break;
            case FishRarity.Epic:
                rarityMultiplier = 8f;
                break;
            case FishRarity.Legendary:
                rarityMultiplier = 16f;
                break;
        }
        
        float qualityMultiplier = 1 + (Mathf.Pow(fishQuality, 4) * 10);

        float finalValue = statValue * rarityMultiplier * qualityMultiplier;

        baseValue = Mathf.CeilToInt(finalValue);
    }
}