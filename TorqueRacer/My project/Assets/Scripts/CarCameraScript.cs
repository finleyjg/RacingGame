using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraScript : MonoBehaviour
{
    public Transform car;

    public float closeDistance = 6.0f;
    public float closeHeight = 2.0f;

    public float farDistance = 10.0f;
    public float farHeight = 4.0f;

    public float rotationDamping = 3.0f;
    public float heightDamping = 2.0f;
    public float transitionSpeed = 2.0f;

    public float brakeZoomInAmount = -3.0f;
    public float accelerateZoomOutAmount = 0.05f;

    public float maxZoomOut = 1.0f;
    public float zoomReturnSpeed = 6.0f;
    public float brakeEffectTime = 0.4f;

    private float currentDistance;
    private float currentHeight;
    private float targetDistance;
    private float targetHeight;

    private int cameraMode = 0; // 0 = close, 1 = far, 2 = free
    private Rigidbody carRigidBody;
    private float baseDistance;

    private bool isBrakingZoomed = false;
    private float brakeZoomTimer = 0f;

    //freecam variables
    public Vector3 freeCamOffset = new Vector3(0, 5, -10);
    public float rotationSpeed = 5f;
    public float smoothingSpeed = 0.2f;
    public float verticalMax = 90f;
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private Quaternion requiredRotation;
    private Vector3 requiredPosition;
    private float targetOffsetZ;

    void Start()
    {
        currentDistance = targetDistance = closeDistance;
        currentHeight = targetHeight = closeHeight;
        baseDistance = closeDistance;

        carRigidBody = car.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetOffsetZ = freeCamOffset.z;
    }

    void LateUpdate()
    {
        //toggle cam mode
        if (Input.GetKeyDown(KeyCode.V))
        {
            cameraMode = (cameraMode + 1) % 3;

            if (cameraMode == 0) //closer view
            {
                targetDistance = closeDistance;
                targetHeight = closeHeight;
                baseDistance = closeDistance;
            }
            else if (cameraMode == 1) //far view
            {
                targetDistance = farDistance;
                targetHeight = farHeight;
                baseDistance = farDistance;
            }
        }

        if (cameraMode == 2) //freecam
        {
            FreeCam();
            return;
        }

        //ensures zoom doesnt apply in free cam
        if (carRigidBody != null)
        {
            float localZVelocity = car.InverseTransformDirection(carRigidBody.velocity).z;

            //zoom on brake
            if (Input.GetKeyDown(KeyCode.S) && brakeZoomTimer <= 0f && !isBrakingZoomed)
            {
                targetDistance = baseDistance + brakeZoomInAmount;
                isBrakingZoomed = true;
                brakeZoomTimer = brakeEffectTime;
            }

            if (isBrakingZoomed)
            {
                brakeZoomTimer -= Time.deltaTime;

                if (brakeZoomTimer <= 0f)
                {
                    targetDistance = Mathf.Lerp(targetDistance, baseDistance, Time.deltaTime * zoomReturnSpeed);

                    if (Mathf.Abs(targetDistance - baseDistance) < 0.1f)
                    {
                        targetDistance = baseDistance;
                        isBrakingZoomed = false;
                    }
                }
            }

            //zoom on accelerate
            if (!isBrakingZoomed && localZVelocity > 0.5f)
            {
                float zoomOffset = Mathf.Clamp(localZVelocity * accelerateZoomOutAmount, 0f, maxZoomOut);
                float accelerateDistance = baseDistance + zoomOffset;
                targetDistance = Mathf.Lerp(targetDistance, accelerateDistance, Time.deltaTime * zoomReturnSpeed);
            }
        }

        //camera following
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * transitionSpeed);
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * transitionSpeed);

        float wantedAngle = car.eulerAngles.y;
        float wantedHeight = car.position.y + currentHeight;

        float myAngle = transform.eulerAngles.y;
        float myHeight = transform.position.y;

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
        myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);

        transform.position = car.position;
        transform.position -= currentRotation * Vector3.forward * currentDistance;

        Vector3 newPosition = transform.position;
        newPosition.y = myHeight;

        transform.position = newPosition;
        transform.LookAt(car);
    }

    private void FreeCam()
    {
        //mouse controls
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        currentRotationX += mouseX;
        currentRotationY -= mouseY;
        currentRotationY = Mathf.Clamp(currentRotationY, -verticalMax, verticalMax);

        //scroll wheel zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            targetOffsetZ += scrollInput * 5f; // adjust multiplier as needed
            targetOffsetZ = Mathf.Clamp(targetOffsetZ, -30f, -2f); // zoom limits
        }

        //smoothing
        freeCamOffset.z = Mathf.Lerp(freeCamOffset.z, targetOffsetZ, Time.deltaTime * 10f);

        //rotation + position
        requiredRotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        requiredPosition = car.position + requiredRotation * freeCamOffset;

        transform.position = Vector3.Lerp(transform.position, requiredPosition, smoothingSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, requiredRotation, smoothingSpeed);
    }

}
