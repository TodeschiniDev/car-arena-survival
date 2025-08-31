using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Transform frontAxle;

    [Header("Power & Speed")]
    [SerializeField] float accelForce = 1800f;
    [SerializeField] float brakeForce = 2200f;
    [SerializeField] float reverseForce = 1200f;
    [SerializeField] float maxSpeed = 50f;
    [SerializeField] float maxReverse = 18f;

    [Header("Steering (front axle)")]
    [SerializeField] float maxSteerAngleDeg = 22f;
    [SerializeField] float corneringForce = 200f;
    [SerializeField] float maxCorneringGrip = 25f;

    [Header("Grip / Stability")]
    [SerializeField, Range(0, 1f)] float driftFactor = 0.85f;
    [SerializeField] float gripRecovery = 6f;
    [SerializeField] float downForce = 0f;

    Rigidbody rb;

    float accelInput;
    float brakeInput;
    float reverseInput;
    float steerInput;

    public void onAccelerate(InputAction.CallbackContext c) => accelInput = c.ReadValue<float>();
    public void OnBrake(InputAction.CallbackContext c) => brakeInput = c.ReadValue<float>();
    public void OnReverse(InputAction.CallbackContext c) => reverseInput = c.ReadValue<float>();
    public void OnSteer(InputAction.CallbackContext c) => steerInput = c.ReadValue<float>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float forwardSpeed = Vector3.Dot(rb.linearVelocity, forward);

        if (forwardSpeed < maxSpeed)
            rb.AddForce(forward * accelInput * accelForce, ForceMode.Force);

        if (Mathf.Abs(forwardSpeed) > 0.01f)
            rb.AddForce(-forward * brakeInput * brakeForce, ForceMode.Force);

        if (forwardSpeed < 0.01f && forwardSpeed > -maxReverse)
            rb.AddForce(-forward * reverseInput * reverseForce, ForceMode.Force);

        float steerAngle = steerInput * maxSteerAngleDeg;
        Vector3 wheelsForwardDirection = Quaternion.AngleAxis(steerAngle, Vector3.up) * forward;
        Vector3 wheelsPerpendicularDirection = Vector3.Cross(Vector3.up, wheelsForwardDirection).normalized;

        float currentCorneringGrip = Mathf.InverseLerp(0f, maxCorneringGrip, Mathf.Abs(forwardSpeed));

        float lateralForceMagnitude = corneringForce * currentCorneringGrip * steerInput;

        Vector3 lateralForce = wheelsPerpendicularDirection * lateralForceMagnitude;

        rb.AddForceAtPosition(lateralForce, frontAxle.position, ForceMode.Force);

        Vector3 forwardVelocity = Vector3.Project(rb.linearVelocity, forward);
        Vector3 sidewayVelocity = Vector3.Project(rb.linearVelocity, right);

        Vector3 targetSideVelocity = sidewayVelocity * driftFactor;
        sidewayVelocity = Vector3.Lerp(sidewayVelocity, targetSideVelocity, gripRecovery * Time.fixedDeltaTime);
        rb.linearVelocity = forwardVelocity + sidewayVelocity;

        //float xSpeed = Vector3.Dot(rb.linearVelocity, right);
        //Vector3 xVelocity = right * xSpeed;

        //Vector3 targetVelocity = rb.linearVelocity - xVelocity * (1f - driftFactor);



        //rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * gripRecovery);

        if (downForce > 0f)
            rb.AddForce(-Vector3.up * downForce * Mathf.Abs(forwardSpeed), ForceMode.Force);

        float planarSpeed = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.x).magnitude;
        if (planarSpeed > Mathf.Max(maxSpeed, maxReverse))
        {
            Vector3 v = rb.linearVelocity;
            Vector3 planar = new Vector3(v.x, 0f, v.z).normalized * Mathf.Max(maxSpeed, maxReverse);
            rb.linearVelocity = new Vector3(planar.x, v.y, planar.z);
        }
    }
}
