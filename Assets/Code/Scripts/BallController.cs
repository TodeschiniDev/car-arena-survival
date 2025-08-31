using UnityEngine;
using UnityEngine.XR;

public class BallController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float forwardAcceleration = 8f, reverseAcceleration = 4f, maxSpeed = 50f, turnStrength = 180f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = rb.transform.position;
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * forwardAcceleration);
    }
}
