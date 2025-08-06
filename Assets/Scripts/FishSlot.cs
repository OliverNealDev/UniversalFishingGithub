// FishSlot.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FishSlot : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI fishNameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private Image fishIcon;
    //[SerializeField] private Image rarityBorder;

    private FishInstance storedFish;

    public static event Action<FishInstance> OnFishSlotClicked;

    private void Awake()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    public void SetData(FishInstance fish)
    {
        storedFish = fish;
        
        fishNameText.text = fish.baseData.fishName;
        rarityText.text = fish.baseData.rarity.ToString();
        fishIcon.sprite = fish.baseData.fishIcon;
        qualityText.text = $"{fish.fishQuality:P0}"; // P0 formats as a percentage with 0 decimal places

        rarityText.color = GetRarityColor(fish.baseData.rarity);
        //rarityBorder.color = GetRarityColor(fish.baseData.rarity);
    }

    private void HandleClick()
    {
        OnFishSlotClicked?.Invoke(storedFish);
    }

    private Color GetRarityColor(FishRarity rarity)
    {
        switch (rarity)
        {
            case FishRarity.Common: return Color.gray;
            case FishRarity.Uncommon: return Color.green;
            case FishRarity.Rare: return Color.blue;
            case FishRarity.Epic: return new Color(0.5f, 0f, 1f); // Purple
            case FishRarity.Legendary: return Color.yellow;
            default: return Color.white;
        }
    }
}