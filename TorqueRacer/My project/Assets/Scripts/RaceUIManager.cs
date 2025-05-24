using UnityEngine;
using UnityEngine.UI;

public class RaceUIManager : MonoBehaviour
{
    public Text timerText;
    public Text lapText;
    public Text speedText;

    public CarControlScript car;
    public int totalLaps = 3;

    private float raceTime = 0f;
    private bool raceStarted = true;
    private Rigidbody carRb;

    public static RaceUIManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (car != null)
        {
            carRb = car.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (raceStarted && !PauseMenu.GameIsPaused)
        {
            raceTime += Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (car != null)
        {
            UpdateLapDisplay();
            UpdateSpeedDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(raceTime / 60);
        int seconds = Mathf.FloorToInt(raceTime % 60);
        int milliseconds = Mathf.FloorToInt((raceTime * 100) % 100);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    void UpdateLapDisplay()
    {
        lapText.text = $"Lap {car.lapNumber}/{totalLaps}";
    }

    void UpdateSpeedDisplay()
    {
        if (carRb != null)
        {
            float speed = carRb.velocity.magnitude * 2.237f; //converts to mph
            speedText.text = $"{speed:0} mph";
        }
    }

    public void StopTimer()
    {
        raceStarted = false;
    }

    public float GetRaceTime()
    {
        return raceTime;
    }

    public string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}
