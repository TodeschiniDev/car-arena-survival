using UnityEngine;
using UnityEngine.InputSystem;

public class SphereCarController : MonoBehaviour
{
    //Physics
    [SerializeField] Rigidbody rb;
    [SerializeField] float forwardAcceleration = 1000f, reverseAcceleration = 1000f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 1000f, dragOnGround = 3f;
    static float midairDrag = 0.1f;

    //Inputs
    private float speedInput, turnInput, reverseInput;

    //Gravity
    private bool grounded;
    public LayerMask whatIsGround;
    public float groundRayLength = .5f;
    [SerializeField] Transform groundRayPoint;

    public void OnAccelerate(InputAction.CallbackContext c) => speedInput = c.ReadValue<float>();
    public void OnSteer(InputAction.CallbackContext c) => turnInput = c.ReadValue<float>();
    public void OnReverse(InputAction.CallbackContext c) => reverseInput = c.ReadValue<float>();
    void Start()
    {
        rb.transform.parent = null;
    }

    void Update()
    {
        transform.position = rb.transform.position;
        if(grounded) Turn();
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit groundHit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out groundHit, groundRayLength, whatIsGround))
        {
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
        }

        if(grounded)
        {
            rb.linearDamping = dragOnGround;
            Accelerate();
            Reverse();
        }
        else
        {
            rb.linearDamping = midairDrag;
            rb.AddForce(Vector3.up * -gravityForce);
        }
    }

    private void Accelerate()
    {
        if(rb.linearVelocity.z < maxSpeed) rb.AddForce(transform.forward * speedInput * forwardAcceleration);
    }

    private void Reverse()
    {
        rb.AddForce(transform.forward * -reverseInput * reverseAcceleration);
    }

    private void Turn()
    {
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float currentSpeedRange = Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(forwardSpeed));
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * currentSpeedRange * turnStrength * Time.deltaTime, 0f));
    }
}
