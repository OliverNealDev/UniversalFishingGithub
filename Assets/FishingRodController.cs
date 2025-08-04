using System.Collections;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    [SerializeField] private FishingMinigameManager fishingMinigameManager;
    [SerializeField] private GameObject bitePromptPanel;

    [SerializeField] private GameObject fishingRodLine;

    [Header("Fishing Settings")]
    [SerializeField] private float biteChancePerSecond = 0.2f;
    [SerializeField] private float biteWindowDuration = 1f;

    private float biteTimer;
    private enum RodState { Idle, WaitingForBite, Bitten, Reeling }
    private RodState currentState;

    void Start()
    {
        currentState = RodState.Idle;
        bitePromptPanel.SetActive(false);
        fishingRodLine.gameObject.SetActive(false);
    }

    void Update()
    {
        PlayerInteract();

        if (currentState == RodState.Bitten)
        {
            HandleBiteTimer();
        }
    }
    
    private void PlayerInteract()
    {
        if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0)) return;

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

    private void HookFish()
    {
        currentState = RodState.Reeling;
        bitePromptPanel.SetActive(false);
        fishingMinigameManager.StartMinigame();
    }
    
    private void CancelFishing()
    {
        currentState = RodState.Idle;
        StopAllCoroutines();
        fishingRodLine.gameObject.SetActive(false);
    }

    public void OnMinigameFinished()
    {
        StartFishingSequence();
    }
}