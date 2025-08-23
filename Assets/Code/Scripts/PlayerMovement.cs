using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody player_rb;

    [SerializeField] float accelerationForce = 10f;
    [SerializeField] float brakeForce = 10f;
    [SerializeField] float reverseForce = 5f;
    [SerializeField] float maxForwardSpeed = 50f;
    [SerializeField] float maxReverseSpeed = 20f;

    private float accelerationInputValue;
    private float brakeInputValue;
    private float reverseInput;

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
        reverseInput = context.ReadValue<float>();
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
    }

    void Accelerate()
    {
        Vector3 acceleration = new Vector3(accelerationInputValue, 0, 0) * accelerationForce;
        if(player_rb.linearVelocity.x < maxForwardSpeed)
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
        Vector3 reverse = new Vector3(-reverseInput, 0, 0) * reverseForce;
        if (player_rb.linearVelocity.x <= 0f)
        {
            player_rb.AddForce(reverse, ForceMode.Force);
        }
    }
}
