using UnityEngine;
using UnityEngine.InputSystem;

public class SphereCarController : MonoBehaviour
{
    //Physics
    [SerializeField] Rigidbody sphereRb;
    [SerializeField] float forwardAcceleration = 1000f, reverseAcceleration = 1000f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 1000f, dragOnGround = 3f;
    static float midairDrag = 0.1f;

    //Inputs
    private float speedInput, turnInput, reverseInput;

    //Gravity
    private bool grounded;
    public LayerMask whatIsGround;
    public float groundRayLength = .5f;
    [SerializeField] Transform groundRayPoint;

    //Wheels
    [SerializeField] Transform leftFrontWhell, rightFrontWhell;
    [SerializeField] float maxWheelTurn = 25f;

    public void OnAccelerate(InputAction.CallbackContext c) => speedInput = c.ReadValue<float>();
    public void OnSteer(InputAction.CallbackContext c) => turnInput = c.ReadValue<float>();
    public void OnReverse(InputAction.CallbackContext c) => reverseInput = c.ReadValue<float>();
    void Start()
    {
        sphereRb.transform.parent = null;
    }

    void Update()
    {
        CarMeshFollowSphereRb();
        Turn();
    }

    private void FixedUpdate()
    {
        grounded = false;
        FixCarInclination();

        if (grounded)
        {
            sphereRb.linearDamping = dragOnGround;
            Accelerate();
            Reverse();
        }
        else
        {
            sphereRb.linearDamping = midairDrag;
            sphereRb.AddForce(Vector3.up * -gravityForce);
        }
    }

    private void CarMeshFollowSphereRb()
    {
        transform.position = sphereRb.transform.position;
    }

    private void Accelerate()
    {
        if(sphereRb.linearVelocity.z < maxSpeed) sphereRb.AddForce(transform.forward * speedInput * forwardAcceleration);
    }

    private void Reverse()
    {
        sphereRb.AddForce(transform.forward * -reverseInput * reverseAcceleration);
    }

    private void Turn()
    {
        if (grounded)
        {
            float forwardSpeed = Vector3.Dot(sphereRb.linearVelocity, transform.forward);
            float currentSpeedRange = Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(forwardSpeed));
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * currentSpeedRange * turnStrength * Time.deltaTime, 0f));
        }

        leftFrontWhell.localRotation = Quaternion.Euler(leftFrontWhell.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWhell.localRotation.eulerAngles.z);
        rightFrontWhell.localRotation = Quaternion.Euler(rightFrontWhell.localRotation.eulerAngles.x, turnInput* maxWheelTurn, rightFrontWhell.localRotation.eulerAngles.z);
    }

    private void FixCarInclination()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }
}
