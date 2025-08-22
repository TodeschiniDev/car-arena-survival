using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody player_rb;

    [SerializeField] float accelerationForce = 10f;
    [SerializeField] float brakeForce = 10f;
    private float accelerationInputValue;
    private float brakeInputValue;

    public void AccelerationInput(InputAction.CallbackContext context)
    {
        accelerationInputValue = context.ReadValue<float>();
    }

    public void BrakeInput(InputAction.CallbackContext context)
    {
        brakeInputValue = context.ReadValue<float>();
    }
    void Start()
    {
        player_rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Accelerate();
        Brake();
    }

    void Accelerate()
    {
        Vector3 acceleration = new Vector3(accelerationInputValue, 0, 0) * accelerationForce;
        player_rb.AddForce(acceleration, ForceMode.Force);
    }

    void Brake()
    {
        Vector3 brake = new Vector3(-brakeInputValue, 0, 0) * brakeForce;
        player_rb.AddForce(brake, ForceMode.Force);
    }
}
