using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Game Settings")]
    private float elapsedTime;
    
    public enum GameState
    {
        MainMenu,
        Playing,
        OptionsOpen,
        InventoryOpen
    }
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        elapsedTime = 0f;
        CurrentState = GameState.Playing;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("Game state changed to: " + newState);

        // You can add logic here, e.g., Time.timeScale = 0 when Paused
        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;
    }
}
