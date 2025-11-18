using UnityEngine;
using UnityEngine.AI;

public class VERAFollow : MonoBehaviour{
    public Transform player;
    
    public float followDistance = 3f; 
    public float stopDistance = 2f;

    private NavMeshAgent VERA;

    void Start()
    {
        VERA = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if(distance >= followDistance)
            {
                VERA.isStopped = false; 
                VERA.SetDestination(player.position - (transform.right * stopDistance));
            }
            else if(distance <= stopDistance)
            {
                VERA.isStopped = true; 
            }
            else
            {
                VERA.isStopped = false;
                VERA.SetDestination(player.position - (transform.right * stopDistance));
            } 
       } 
    }
}