using UnityEngine;
using UnityEngine.AI;

public class ChaserNavigation : MonoBehaviour
{
    Transform playerTransform;
    NavMeshAgent agent;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerTransform.position;
        
    }
}
