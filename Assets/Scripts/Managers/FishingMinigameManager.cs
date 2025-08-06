using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishingMinigameManager : MonoBehaviour
{
    [SerializeField] private FishingRodController fishingRodController;

    [Header("Game Elements")]
    public GameObject fishingMinigameUI;
    public Slider progressBar;
    public RectTransform fishIcon;
    public RectTransform catchBar;
    public RectTransform track;

    [Header("Base Game Settings")]
    [SerializeField] private float gravity = 150f;
    [SerializeField] private float catchPower = 100f;

    [Header("Quality Damage Settings")]
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private float qualityReductionPercentPerHit = 10;
    
    [Header("Quality Effect Settings")]
    [SerializeField] private float minFishSpeedFactor = 0.25f;
    [SerializeField] private float maxMoveTimerDelay = 1.5f;

    private float fishQuality;
    private float fishSpeed;
    private float fishMoveTimer;
    private float progressGainRate;
    private float progressDrainRate;
    private float initialFishProgress;

    private float secondsStruggled;
    private float fishPosition;
    private float fishDestination;
    private float fishMoveTimerTicker;
    private float catchBarPosition;
    private float catchBarVelocity;
    private float currentProgress;

    public FishData currentFishData;
    private MinigameParameters currentParams;

    private Image fishImage;
    private Color originalFishColor;
    private Coroutine hurtRoutine;

    private void Awake()
    {
        fishImage = fishIcon.GetComponent<Image>();
        originalFishColor = fishImage.color;
    }

    private void Start()
    {
        fishingMinigameUI.SetActive(false);
    }

    public void StartMinigame(FishData hookedFishData, MinigameParameters parameters)
    {
        currentFishData = hookedFishData;
        currentParams = parameters;

        fishQuality = currentParams.FinalQuality;
        fishSpeed = currentParams.FinalFishSpeed;
        fishMoveTimer = currentParams.FinalFishMoveTimer;
        progressGainRate = currentParams.FinalProgressGainRate;
        progressDrainRate = currentParams.FinalProgressDrainRate;
        initialFishProgress = currentParams.FinalInitialFishProgress;
        currentProgress = initialFishProgress;
        catchBar.GetComponent<RectTransform>().sizeDelta = new Vector2(catchBar.GetComponent<RectTransform>().sizeDelta.x, currentParams.FinalCatchBarSize);

        fishingMinigameUI.SetActive(true);

        catchBarPosition = 0f;
        catchBarVelocity = 0f;
        fishPosition = 0f;
        fishDestination = 0f;
        fishMoveTimerTicker = -1;

        UpdateCatchBarVisuals();
        UpdateFishPositionVisuals();

        secondsStruggled = 0;
        fishImage.color = originalFishColor;

        hurtRoutine = StartCoroutine(HurtFishRoutine());
    }

    void Update()
    {
        if (fishingMinigameUI.activeSelf)
        {
            UpdateFishPosition();
            UpdateCatchBar();
            UpdateProgress();
        }
    }

    private IEnumerator HurtFishRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            if (!IsFishInCatchBar())
            {
                float qualityReductionAmount = fishQuality * (qualityReductionPercentPerHit / 100f);
                fishQuality -= qualityReductionAmount;
                fishQuality = Mathf.Max(0, fishQuality);
                StartCoroutine(FlashFishRed());
            }
        }
    }

    private IEnumerator FlashFishRed()
    {
        fishImage.color = Color.red;
        float elapsedTime = 0f;
        float fadeDuration = 0.4f;

        while (elapsedTime < fadeDuration)
        {
            fishImage.color = Color.Lerp(Color.red, originalFishColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fishImage.color = originalFishColor;
    }

    private bool IsFishInCatchBar()
    {
        float catchBarMin = catchBarPosition;
        float catchBarMax = catchBarPosition + catchBar.rect.height;
        return (catchBarMin <= fishPosition && fishPosition <= catchBarMax);
    }

    private void UpdateFishPosition()
    {
        float qualitySpeedFactor = minFishSpeedFactor + ((1f - minFishSpeedFactor) * fishQuality);
        float effectiveSpeed = fishSpeed * qualitySpeedFactor;

        float addedMoveDelay = maxMoveTimerDelay * (1f - fishQuality);
        float effectiveMoveTimer = fishMoveTimer + addedMoveDelay;

        fishMoveTimerTicker -= Time.deltaTime;
        if (fishMoveTimerTicker < 0f)
        {
            fishMoveTimerTicker = effectiveMoveTimer;
            float trackHeight = track.rect.height;
            fishDestination = Random.Range(0, trackHeight);
        }
        fishPosition = Mathf.Lerp(fishPosition, fishDestination, Time.deltaTime * effectiveSpeed * 0.1f);
        UpdateFishPositionVisuals();
    }

    private void UpdateCatchBar()
    {
        if (Input.GetMouseButton(0))
        {
            catchBarVelocity += catchPower * Time.deltaTime;
        }
        catchBarVelocity -= gravity * Time.deltaTime;
        catchBarPosition += catchBarVelocity * Time.deltaTime;
        float trackHeight = track.rect.height;
        catchBarPosition = Mathf.Clamp(catchBarPosition, 0, trackHeight - catchBar.rect.height);
        if (catchBarPosition == 0 || catchBarPosition == trackHeight - catchBar.rect.height)
        {
            catchBarVelocity = 0;
        }
        UpdateCatchBarVisuals();
    }

    private void UpdateProgress()
    {
        if (IsFishInCatchBar())
        {
            currentProgress += progressGainRate * Time.deltaTime;
        }
        else
        {
            currentProgress -= progressDrainRate * Time.deltaTime;
            secondsStruggled += Time.deltaTime;
        }
        currentProgress = Mathf.Clamp01(currentProgress);
        progressBar.value = currentProgress;

        if (currentProgress >= 1f)
        {
            Win();
        }
        else if (currentProgress <= 0f)
        {
            Lose();
        }
    }

    private void UpdateFishPositionVisuals()
    {
        fishIcon.anchoredPosition = new Vector2(fishIcon.anchoredPosition.x, fishPosition - track.rect.height / 2);
    }

    private void UpdateCatchBarVisuals()
    {
        catchBar.anchoredPosition = new Vector2(catchBar.anchoredPosition.x, catchBarPosition - track.rect.height / 2);
    }

    private void StopMinigame()
    {
        if (hurtRoutine != null)
        {
            StopCoroutine(hurtRoutine);
        }
        fishingMinigameUI.SetActive(false);
    }

    private void Win()
    {
        StopMinigame();
        fishingRodController.OnMinigameFinished(true, fishQuality);
    }

    private void Lose()
    {
        StopMinigame();
        fishingRodController.OnMinigameFinished(false, 0);
    }
}

public class MinigameParameters
{
    public float FinalQuality;
    public float FinalFishSpeed;
    public float FinalFishMoveTimer;
    public float FinalProgressGainRate;
    public float FinalProgressDrainRate;
    public float FinalInitialFishProgress;
    public float FinalCatchBarSize;
}