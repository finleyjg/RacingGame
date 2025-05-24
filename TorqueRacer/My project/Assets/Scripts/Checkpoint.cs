using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index;

    private void OnTriggerEnter(Collider other)
    {
        CarControlScript player = other.gameObject.GetComponent<CarControlScript>();
        if (player)
        {
            Debug.Log($"Checkpoint {index} hit by {other.gameObject.name}. Player checkpointIndex: {player.checkpointIndex}");

            if (player.checkpointIndex == index - 1)
            {
                player.checkpointIndex = index;
                Debug.Log($"Checkpoint index updated to {index}");
            }
        }
    }
}
