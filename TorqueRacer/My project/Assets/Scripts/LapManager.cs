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

                //saves the finish time so it can be displayed in Race Summary scene
                PlayerPrefs.SetFloat("LastRaceTime", finishTime);
                PlayerPrefs.Save();

                if (finishTime < bestTime)
                {
                    PlayerPrefs.SetFloat("BestRaceTime", finishTime);
                    //PlayerPrefs.DeleteKey("BestRaceTime");
                    PlayerPrefs.Save();
                    Debug.Log($"New Best Time: {RaceUIManager.Instance.FormatTime(finishTime)}");
                }
                else
                {
                    Debug.Log($"Finish time: {RaceUIManager.Instance.FormatTime(finishTime)} (Best: {RaceUIManager.Instance.FormatTime(bestTime)})");
                }

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
}
