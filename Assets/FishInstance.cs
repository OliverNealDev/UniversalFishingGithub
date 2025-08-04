using UnityEngine;

[System.Serializable]
public class FishInstance
{
    public FishData baseData;

    public float fishQuality;
    public int calculatedValue;

    public FishInstance(FishData caughtFishData, float secondsStruggled)
    {
        baseData = caughtFishData;
        //fishQuality = 
    }
}