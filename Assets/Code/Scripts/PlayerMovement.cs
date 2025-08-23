using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody player_rb;

    [SerializeField] float accelerationForce = 10f;
    [SerializeField] float brakeForce = 10f;
    [SerializeField] float reverseForce = 5f;
    [SerializeField] float torqueForce = 5f;
    [SerializeField] float maxForwardSpeed = 50f;
    [SerializeField] float maxReverseSpeed = 20f;
    [SerializeField] float maxTorque = 30f;

    private float accelerationInputValue;
    private float brakeInputValue;
    private float reverseInputValue;
    private float turnRightInputValue;
    private float turnLeftInputValue;

    public void AccelerationInput(InputAction.CallbackContext context)
    {
        accelerationInputValue = context.ReadValue<float>();
    }

    public void BrakeInput(InputAction.CallbackContext context)
    {
        brakeInputValue = context.ReadValue<float>();
    }

    public void ReverseInput(InputAction.CallbackContext context)
    {
        reverseInputValue = context.ReadValue<float>();
    }

    public void TurnRightInput(InputAction.CallbackContext context)
    {
        turnRightInputValue = context.ReadValue<float>();
    }
    public void TurnLeftInput(InputAction.CallbackContext context)
    {
        turnLeftInputValue = context.ReadValue<float>();
    }
    void Start()
    {
        player_rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Accelerate();
        Brake();
        Reverse();
        TurnRight();
        TurnLeft();
    }

    void Accelerate()
    {
        Vector3 acceleration =  transform.right * accelerationInputValue * accelerationForce;
        if (player_rb.linearVelocity.magnitude < maxForwardSpeed)
        {
            player_rb.AddForce(acceleration, ForceMode.Force);
        }
    }

    void Brake()
    {
        Vector3 brake = new Vector3(-brakeInputValue, 0, 0) * brakeForce;
        if (Mathf.Abs(player_rb.linearVelocity.x) > 0f)
        {
            player_rb.AddForce(brake, ForceMode.Force);
        }
    }

    void Reverse()
    {
        Vector3 reverse = new Vector3(-reverseInputValue, 0, 0) * reverseForce;
        if (player_rb.linearVelocity.x <= 0f)
        {
            player_rb.AddForce(reverse, ForceMode.Force);
        }
    }

    void TurnRight()
    {
        Vector3 torque = new Vector3(0, turnRightInputValue, 0) * torqueForce;
        if ((player_rb.angularVelocity.y < maxTorque))
        {
            player_rb.AddTorque(Vector3.up * turnRightInputValue * torqueForce);
        }
    }

    void TurnLeft()
    {
        Vector3 torque = new Vector3(0, -turnLeftInputValue, 0) * torqueForce;
        if(Mathf.Abs(player_rb.angularVelocity.y) < maxTorque)
        {
            player_rb.AddTorque(Vector3.up * -turnLeftInputValue * torqueForce);
        }
    }
}
