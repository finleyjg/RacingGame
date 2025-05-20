using UnityEngine;
using System.Collections;

public class CarControlScript : MonoBehaviour
{
    private float maxTorque = 1000;  
    private float maxSteerAngle = 20;
    private float driftFactor = 0.75f; //lower drift factor makes turning feel more natural and intuitive, compared to higher factors.
    private float brakeTorque = 12000;  

    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;

    public Transform WheelFLTrans;
    public Transform WheelFRTrans;
    public Transform WheelRLTrans;
    public Transform WheelRRTrans;


    private Rigidbody rb;

    //variables for offroad

    private float onGroundTimer = 0f;       //timer to count how long car is on ground
    private float requiredOnGroundTime = 0.25f; //the time required to switch to offroad or back

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);

        Suspension(WheelFL);
        Suspension(WheelFR);
        Suspension(WheelRL);
        Suspension(WheelRR);

        Friction(WheelFL);
        Friction(WheelFR);
        Friction(WheelRL);
        Friction(WheelRR);
    }



    void FixedUpdate()
    {
        bool onGroundSurface = false;

        if (IsWheelOnTag(WheelFL, "Ground") ||
            IsWheelOnTag(WheelFR, "Ground") ||
            IsWheelOnTag(WheelRL, "Ground") ||
            IsWheelOnTag(WheelRR, "Ground"))
        {
            onGroundSurface = true;
        }

        
        if (onGroundSurface)
        {
            onGroundTimer += Time.fixedDeltaTime; //add time
        }
        else
        {
            onGroundTimer = 0f;  //resets to normal
        }

        //only apply drag and maxTorque change after being on ground for at least 1 second
        if (onGroundTimer >= requiredOnGroundTime)
        {
            maxTorque = 700f;
            rb.drag = 0.3f;  //increase drag offroad and lower torque
        }
        else
        {
            maxTorque = 1000f;
            rb.drag = 0.05f; //default drag and torque
        }

        //normal torque on road
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");
        float speedFactor = Mathf.Clamp01(rb.velocity.magnitude / 20);

        float currentSteerAngle = maxSteerAngle * steerInput * (1 - 0.6f * speedFactor);
        WheelFL.steerAngle = currentSteerAngle;
        WheelFR.steerAngle = currentSteerAngle;

        AccelerateAndBrake(moveInput);

        drift();
    }


    //function to check if wheel is touching tags to detect offroad
    bool IsWheelOnTag(WheelCollider wheel, string tag)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            if (hit.collider.CompareTag(tag))
                return true;
        }
        return false;
    }


    void AccelerateAndBrake(float moveInput)
    {
        float forwardSpeed = Vector3.Dot(rb.velocity, transform.forward); //calculates speed

        bool movingForward = forwardSpeed > 0.1f;
        bool movingBackward = forwardSpeed < -0.1f; ; //determines whether the car is moving forward or backward

        bool isBraking = (movingForward && moveInput < 0) || (movingBackward && moveInput > 0); //based on player's input, determines whether the car should be braking

        if (isBraking)
        {
            WheelFL.brakeTorque = brakeTorque;
            WheelFR.brakeTorque = brakeTorque;
            WheelRL.brakeTorque = brakeTorque;
            WheelRR.brakeTorque = brakeTorque;

            WheelRL.motorTorque = 0;
            WheelRR.motorTorque = 0;
        }
        else
        {
            WheelFL.brakeTorque = 0;
            WheelFR.brakeTorque = 0;
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;

            WheelRL.motorTorque = maxTorque * moveInput;
            WheelRR.motorTorque = maxTorque * moveInput;
        }
    }

    //this function adjusts the sidways velocity of the car based on the drift factor (determined at the top), which allows simulation of slight sliding mechanics
    void drift()
    {
        Vector3 velocity = rb.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        localVelocity.x *= driftFactor;
        rb.velocity = transform.TransformDirection(localVelocity);
    }

    void Suspension(WheelCollider wheel)
    {
        JointSpring suspensionSpring = wheel.suspensionSpring;
        suspensionSpring.spring = 15000f; //stiffness of the spring (higher = more stiff)
        suspensionSpring.damper = 4500f;  // damping - how fast the spring returns to normal
        suspensionSpring.targetPosition = 0.5f; //standard suspension of the car

        wheel.suspensionSpring = suspensionSpring;
        wheel.suspensionDistance = 0.3f; //movement range
    }

    void Friction(WheelCollider wheel)
    {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.stiffness = 1.5f;
        wheel.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = 1.5f;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    void Update()
    {
        UpdateWheelSpin(WheelFL, WheelFLTrans);
        UpdateWheelSpin(WheelFR, WheelFRTrans);
        UpdateWheelSpin(WheelRL, WheelRLTrans);
        UpdateWheelSpin(WheelRR, WheelRRTrans);
    }

    void UpdateWheelSpin(WheelCollider col, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);

        trans.position = pos;
        trans.rotation = rot;
    }

}

