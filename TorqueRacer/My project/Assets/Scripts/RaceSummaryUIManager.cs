using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceSummaryUIManager : MonoBehaviour
{
    public Text currentTimeText;
    public Text bestTimeText;

    private float currentRaceTime = 0f;

    void Start()
    {
        //time from the race
        currentRaceTime = PlayerPrefs.GetFloat("LastRaceTime", 0f);
        currentTimeText.text = "Time: " + TimeFormatter.FormatTime(currentRaceTime);

        //best time
        float bestTime = PlayerPrefs.GetFloat("BestRaceTime", float.MaxValue);
        if (bestTime == float.MaxValue)
        {
            bestTimeText.text = "Best Time: --:--:--";
        }

        if (PlayerPrefs.GetFloat("BestRaceTime", float.MaxValue) == 0f)//to prevent a bug where best time could be 0s
        {
            PlayerPrefs.DeleteKey("BestRaceTime");
            PlayerPrefs.Save();
        }

        else
        {
            bestTimeText.text = "Best Time: " + TimeFormatter.FormatTime(bestTime);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
