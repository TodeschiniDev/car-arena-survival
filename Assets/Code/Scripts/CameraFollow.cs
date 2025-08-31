using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 cameraOfsset;
    [SerializeField] Transform playerTransform;
    void Start()
    {
        cameraOfsset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + cameraOfsset;
    }
}
