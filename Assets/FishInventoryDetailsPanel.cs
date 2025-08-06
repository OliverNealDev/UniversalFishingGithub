using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishInventoryDetailsPanel : MonoBehaviour
{
    [Header("Detail UI Elements")]
    [SerializeField] private TextMeshProUGUI fishNameText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    //[SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image fishIcon;
    [SerializeField] private GameObject detailsContent; // Parent object of all details

    private void Start()
    {
        ClearDetails();
    }

    public void DisplayFishDetails(FishInstance fish)
    {
        detailsContent.SetActive(true);

        fishNameText.text = fish.baseData.fishName;
        rarityText.text = fish.baseData.rarity.ToString();
        rarityText.color = GetRarityColor(fish.baseData.rarity);
        
        qualityText.text = $"Quality: {fish.fishQuality:P2}"; // P2 for two decimal places
        descriptionText.text = fish.baseData.description;
        fishIcon.sprite = fish.baseData.fishIcon;

        // Assumes you have an EconomyManager instance to calculate value
        // If not, you can display fish.baseValue directly
        //int salePrice = EconomyManager.instance.GetSalePrice(fish);
        //valueText.text = $"Value: {salePrice}c";
    }

    public void ClearDetails()
    {
        detailsContent.SetActive(false);
    }
    
    private Color GetRarityColor(FishRarity rarity)
    {
        switch (rarity)
        {
            case FishRarity.Common: return Color.gray;
            case FishRarity.Uncommon: return Color.green;
            case FishRarity.Rare: return Color.blue;
            case FishRarity.Epic: return new Color(0.5f, 0f, 1f);
            case FishRarity.Legendary: return Color.yellow;
            default: return Color.white;
        }
    }
}