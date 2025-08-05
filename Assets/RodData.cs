using UnityEngine;

[CreateAssetMenu(fileName = "RodData", menuName = "Scriptable Objects/RodData")]
public class RodData : ScriptableObject
{
    public string rodName;
    public Sprite rodIcon;

    [Header("Rod Modifiers")]
    [Tooltip("e.g., 1.2 gives a 20% bigger bite chance")]
    public float biteChanceModifier = 1f;

    [Tooltip("e.g., 1.2 gives a moderately bigger chance of a better quality fish")]
    public float qualityModifier = 1f; // Not sure how to calculate this
    
    [Header("Minigame Modifiers")]
    [Tooltip("e.g., 200 would make the catch bar 200 units tall")]
    public float catchBarSize = 100f;

    [Tooltip("e.g., 0.9 makes the fish 10% slower")]
    public float fishSpeedModifier = 1f;
    
    [Tooltip("e.g., 0.8 makes the fish move 20% less often")]
    public float fishMovementFrequencyModifier = 1f;

    [Tooltip("e.g., 1.2 gives a 20% bonus to catch progress")]
    public float progressGainModifier = 1f;
    
    [Tooltip("e.g., 0.8 slows down the drain of the catch progress by 20%")]
    public float progressDrainModifier = 1f;
    
    [Tooltip("e.g., 1.5 gives a 50% slower quality drain during fish struggle")]
    public float qualityDrainReductionModifier = 1f;

    [Tooltip("e.g., 1.2 gives a 20% bigger initial fish progress")]
    public float initialFishProgress = 0.25f;
}