using UnityEngine;
using UnityEngine.AI;

public class VERANav : MonoBehaviour, IDataPersistence
{
    
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

    void Awake()
    {
        // this was moved into awake because save loads in start, and if start wasn't run then VERA wouldn't have any position to load into
        VERA = GetComponent<NavMeshAgent>();
        VERA.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
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
                //offset VERA location 
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

    //load VERA's position
    public void LoadData(GameData data)
    {
        // this.transform.position = data.VERAPosition;
        // navmesh issues w/ transform.posoition so we warp instead, which is basically teleporting but it works with the navmesh and doesn't cause issues
        if (VERA == null)
        {
            VERA = GetComponent<NavMeshAgent>(); // safety + grace 
        }
        // using warp to teleport VERA to the saved position without messing with the navmesh, which caused issues when tried to just set transform.position
        // then resetting path and clearing any old movement or data so VERA does not continue to old path after loading and warping to new position
        VERA.Warp(data.VERAPosition);
        VERA.ResetPath();
    }

    //save VERA's position
    public void SaveData(ref GameData data)
    {
        data.VERAPosition = this.transform.position;
    }
}