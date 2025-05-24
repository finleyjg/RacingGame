using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LapManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints;
    public int totalLaps;

    public static LapManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        CarControlScript player = other.gameObject.GetComponent<CarControlScript>();
        if (player && player.checkpointIndex == checkpoints.Count)
        {
            player.checkpointIndex = 0;
            player.lapNumber++;

            Debug.Log($"lap {player.lapNumber} out of {totalLaps}");

            if (player.lapNumber > totalLaps)
            {
                float finishTime = RaceUIManager.Instance.GetRaceTime();
                float bestTime = PlayerPrefs.GetFloat("BestRaceTime", float.MaxValue);

                Debug.Log($"Saving LastRaceTime: {finishTime}");
                Debug.Log($"BestRaceTime currently: {bestTime}");


                //save the latest race time
                PlayerPrefs.SetFloat("LastRaceTime", finishTime);

                //update best time if the new time is better
                if (finishTime < bestTime)
                {
                    PlayerPrefs.SetFloat("BestRaceTime", finishTime);
                    Debug.Log($"New Best Time: {RaceUIManager.Instance.FormatTime(finishTime)}");
                }
                else
                {
                    Debug.Log($"Finish time: {RaceUIManager.Instance.FormatTime(finishTime)} (Best: {RaceUIManager.Instance.FormatTime(bestTime)})");
                }

                PlayerPrefs.Save();

                Debug.Log("Win");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        }
    }

    public Vector3 GetCheckpointPosition(int index)
    {
        if (index >= 0 && index < checkpoints.Count)
            return checkpoints[index].transform.position;
        return Vector3.zero;
    }

    public Quaternion GetCheckpointRotation(int index)
    {
        if (index >= 0 && index < checkpoints.Count)
            return checkpoints[index].transform.rotation;
        return Quaternion.identity;
    }

    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey("BestRaceTime");
        PlayerPrefs.Save();
    }


}
