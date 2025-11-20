using UnityEngine;
using UnityEngine.AI;

public class VERAFollow : MonoBehaviour{
    
    //reference to player location data 
    private Transform player;
    
    [Header("Follow Settings")]
    [SerializeField] private float followDistance = 5f; 
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] public float leftOffset = 2f;

    //handles VERA's movement and pathfinding in the navmesh 
    private NavMeshAgent VERA;

    void Start()
    {
        VERA = GetComponent<NavMeshAgent>();
        VERA.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        //find player game object in scene to reference 
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject != null)
        {
            player = playerObject.transform; 
        }
        else
        {
            Debug.LogError("player not found!");
        }
    }

    void Update()
    {
        if(player != null)
        {
            //VERA's distance from the player
            float distance = Vector3.Distance(transform.position, player.position);
            if(distance >= followDistance)
            {
                VERA.isStopped = false; 
                VERA.SetDestination(player.position - (player.right * leftOffset));
            }
            else if(distance <= stopDistance)
            {
                VERA.isStopped = true; 
            }
            else
            {
                VERA.isStopped = false;
                VERA.SetDestination(player.position - (player.right * leftOffset));
            } 
       } 
    }
}