using UnityEngine;
using UnityEngine.UI;

public class FishingMinigameManager : MonoBehaviour
{
    [SerializeField] private FishingRodController fishingRodController;
    
    [Header("Game Elements")]
    public GameObject fishingMinigameUI; // The UI panel for the minigame
    public Slider progressBar;
    public RectTransform fishIcon;
    public RectTransform catchBar;
    public RectTransform track; // The background track

    [Header("Game Settings")]
    [SerializeField] private float fishSpeed = 300f;
    [SerializeField] private float gravity = 150f;
    [SerializeField] private float catchPower = 100f;
    [SerializeField] private float progressGainRate = 0.5f;
    [SerializeField] private float progressDrainRate = 0.2f;

    // Private state variables
    private float fishPosition;
    private float fishDestination;
    private float fishMoveTimer;
    private float fishTimerMulti = 2f; // How quickly the fish changes direction

    private float catchBarPosition;
    private float catchBarVelocity;

    private float currentProgress = 0.25f; // Start with a bit of progress

    private void Start()
    {
        // Deactivate on start, to be activated by another script
        fishingMinigameUI.SetActive(false);
    }

    // This is the public method to call from your other scripts
    public void StartMinigame(/* You can pass FishData here to change difficulty */)
    {
        fishingMinigameUI.SetActive(true);
        currentProgress = 0.25f;
        catchBarPosition = 0;
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

    private void UpdateFishPosition()
    {
        // Make the fish move to a new random destination over time
        fishMoveTimer -= Time.deltaTime;
        if (fishMoveTimer < 0f)
        {
            fishMoveTimer = Random.Range(0.5f, 1.5f) * fishTimerMulti;
            float trackHeight = track.rect.height;
            fishDestination = Random.Range(0, trackHeight);
        }

        // Move fish towards destination
        fishPosition = Mathf.Lerp(fishPosition, fishDestination, Time.deltaTime * fishSpeed * 0.1f);

        // Update the fish icon's actual position on the UI
        fishIcon.anchoredPosition = new Vector2(fishIcon.anchoredPosition.x, fishPosition - track.rect.height / 2);
    }

    private void UpdateCatchBar()
    {
        // Player Input
        if (Input.GetMouseButton(0))
        {
            catchBarVelocity += catchPower * Time.deltaTime;
        }

        // Apply Gravity
        catchBarVelocity -= gravity * Time.deltaTime;

        // Update position based on velocity
        catchBarPosition += catchBarVelocity * Time.deltaTime;

        // Clamp position to stay within the track
        float trackHeight = track.rect.height;
        catchBarPosition = Mathf.Clamp(catchBarPosition, 0, trackHeight - catchBar.rect.height);

        // Reset velocity if hitting the top or bottom to prevent "sticking"
        if (catchBarPosition == 0 || catchBarPosition == trackHeight - catchBar.rect.height)
        {
            catchBarVelocity = 0;
        }

        // Update the catch bar's actual position on the UI
        catchBar.anchoredPosition = new Vector2(catchBar.anchoredPosition.x, catchBarPosition - track.rect.height / 2);
    }

    private void UpdateProgress()
    {
        // Check if the catch bar is over the fish
        float catchBarMin = catchBarPosition;
        float catchBarMax = catchBarPosition + catchBar.rect.height;

        if (catchBarMin <= fishPosition && fishPosition <= catchBarMax)
        {
            // We are catching the fish!
            currentProgress += progressGainRate * Time.deltaTime;
        }
        else
        {
            // We are not catching the fish
            currentProgress -= progressDrainRate * Time.deltaTime;
        }
        
        // Clamp progress between 0 and 1
        currentProgress = Mathf.Clamp01(currentProgress);

        // Update the UI progress bar
        progressBar.value = currentProgress;

        // Check for win/loss conditions
        if (currentProgress >= 1f)
        {
            Win();
        }
        else if (currentProgress <= 0f)
        {
            Lose();
        }
    }

    private void Win()
    {
        Debug.Log("FISH CAUGHT!");
        // Add fish to inventory, give XP, etc.
        fishingMinigameUI.SetActive(false); // Hide the minigame panel
        fishingRodController.OnMinigameFinished(); // Reset the rod state
    }

    private void Lose()
    {
        Debug.Log("Fish got away...");
        // Play a "failure" sound, etc.
        fishingMinigameUI.SetActive(false); // Hide the minigame panel
        fishingRodController.OnMinigameFinished(); // Reset the rod state
    }
}