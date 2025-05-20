using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundScript : MonoBehaviour
{
    public AudioSource engineAudio;
    //the audio source component attached to the car, for engine sound

    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;
    //min and max pitch values for engine sound variation

    public float pitchMultiplier = 0.02f;
    //scales the pitch in relation to speed

    private Rigidbody carRigidBody;
    //reference to the car's rigidbody

    void Start()
    {
        //get the rigidbody component from the car object
        carRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (engineAudio != null && carRigidBody != null)
        {
            float speed = carRigidBody.velocity.magnitude;
            float pitch = Mathf.Clamp(minPitch + (speed * pitchMultiplier), minPitch, maxPitch);
            engineAudio.pitch = pitch;
        }
    }
}
