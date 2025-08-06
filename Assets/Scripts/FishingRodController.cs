using System.Collections;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    [SerializeField] private FishingMinigameManager fishingMinigameManager;
    [SerializeField] private GameObject bitePromptPanel;

    [SerializeField] private GameObject fishingRodLine;

    [Header("Fishing Settings")]
    [SerializeField] private float biteChancePerSecond = 0.5f;
    [SerializeField] private float biteWindowDuration = 1f;

    private float biteTimer;
    private enum RodState { Idle, WaitingForBite, Bitten, Reeling }
    private RodState currentState;
    
    public RodData equippedRod;

    void Start()
    {
        currentState = RodState.Idle;
        bitePromptPanel.SetActive(false);
        fishingRodLine.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentState == RodState.Bitten)
        {
            HandleBiteTimer();
        }
    }
    
    public void PlayerInteract()
    {
        switch (currentState)
        {
            case RodState.Idle:
                StartFishingSequence();
                break;
            case RodState.Bitten:
                HookFish();
                break;
            case RodState.WaitingForBite:
                CancelFishing();
                break;
        }
    }

    private void StartFishingSequence()
    {
        currentState = RodState.WaitingForBite;
        StartCoroutine(BiteCheckRoutine());
        fishingRodLine.gameObject.SetActive(true);
    }

    private IEnumerator BiteCheckRoutine()
    {
        while (currentState == RodState.WaitingForBite)
        {
            if (Random.Range(0f, 1f) < biteChancePerSecond * Time.deltaTime)
            {
                OnBite();
                yield break;
            }
            yield return null;
        }
    }

    private void OnBite()
    {
        currentState = RodState.Bitten;
        bitePromptPanel.SetActive(true);
        biteTimer = biteWindowDuration;
    }

    private void HandleBiteTimer()
    {
        if (biteTimer > 0)
        {
            biteTimer -= Time.deltaTime;
        }
        else
        {
            bitePromptPanel.SetActive(false);
            StartFishingSequence();
        }
    }

    // In FishingRodController.cs

private void HookFish()
{
    currentState = RodState.Reeling;
    bitePromptPanel.SetActive(false);
    var randomFish = FishManager.allFish[Random.Range(0, FishManager.allFish.Count)];
    
    MinigameParameters parameters = new MinigameParameters();
    
    // QUALITY CALCULATION
    const float BASE_DIFFICULTY_EXPONENT = 2.0f;
    float finalExponent = BASE_DIFFICULTY_EXPONENT / equippedRod.qualityModifier;
    float randomRoll = Random.Range(0f, 1f);
    float skewedRoll = Mathf.Pow(randomRoll, finalExponent);
    float quality = 0.5f + (skewedRoll * 0.5f);
    if (quality > 0.995f) // Grant perfect quality if current quality > 99.5% (0.995)
    {
        quality = 1.0f;
    }
    parameters.FinalQuality = quality;
    Debug.Log("caught fish with quality: " + quality);
    //
    
    float baseSpeed = randomFish.fishAgility * 50f;
    float baseMoveTimer = (5.1f - (randomFish.fishAgility * 5));
    float baseGain = (1.0001f - randomFish.fishWeight) / 2f;
    float baseDrain = (randomFish.fishWeight + 0.0001f) / 2f;
    
    parameters.FinalFishSpeed = baseSpeed * equippedRod.fishSpeedModifier;
    parameters.FinalFishMoveTimer = baseMoveTimer / equippedRod.fishMovementFrequencyModifier;
    parameters.FinalProgressGainRate = baseGain * equippedRod.progressGainModifier;
    parameters.FinalProgressDrainRate = baseDrain * equippedRod.progressDrainModifier;
    
    parameters.FinalInitialFishProgress = equippedRod.initialFishProgress;
    parameters.FinalCatchBarSize = equippedRod.catchBarSize;
    
    fishingMinigameManager.StartMinigame(randomFish, parameters);
}
    
    private void CancelFishing()
    {
        currentState = RodState.Idle;
        StopAllCoroutines();
        fishingRodLine.gameObject.SetActive(false);
    }

    public void OnMinigameFinished(bool wasSuccesful, float postGameQuality)
    {
        if (wasSuccesful)
        {
            // Handle successful catch
            var caughtFish = new FishInstance(fishingMinigameManager.currentFishData, postGameQuality);
            InventoryManager.instance.AddFishToInventory(caughtFish);
            Debug.Log($"Caught {caughtFish.baseData.fishName} with quality {caughtFish.fishQuality}");
        }
        else
        {
            // Handle failed catch
            Debug.Log("Failed to catch the fish.");
        }

        // Reset state after minigame
        currentState = RodState.Idle;
        bitePromptPanel.SetActive(false);
        fishingRodLine.gameObject.SetActive(false);
        
        StartFishingSequence();
    }
}