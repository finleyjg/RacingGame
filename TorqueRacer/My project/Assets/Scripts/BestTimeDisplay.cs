using UnityEngine;
using UnityEngine.UI;

public class BestTimeDisplay : MonoBehaviour
{
    public Text bestTimeText;

    void Start()
    {
        float bestTime = PlayerPrefs.GetFloat("BestRaceTime", float.MaxValue);

        if (bestTime == float.MaxValue)
        {
            bestTimeText.text = "Best Time: --:--:--";
        }
        else
        {
            bestTimeText.text = "Best Time: " + TimeFormatter.FormatTime(bestTime);
        }
    }
}
