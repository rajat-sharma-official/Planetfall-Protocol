using UnityEngine;
using UnityEngine.AI;

public class TamiraAI : MonoBehaviour
{
    [SerializeField] private GameObject target; //set to the player
    private NavMeshAgent agent; //npc
    private Animator animator; 

    [SerializeField] private float wanderRadius = 11f; 
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 4f;

    [SerializeField] private float engageDistance = 7f;
    
    private float waitTimer = 0f;
    private float waitDuration = 0f;

    //npc states
    private enum NPCBehaviors
    {
        idle, //standing
        wandering, //walking around
        engaged //in conversation, or acknowledged player within conversation distance
    }

    [SerializeField] private NPCBehaviors currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if(agent == null)
        {
            Debug.LogError("nav mesh agent is null");
        }
        agent.stoppingDistance = 3f;
        agent.radius = 0.5f;

        target = GameObject.FindWithTag("Player");
        if(target == null)
        {
            Debug.LogError("player not found");
        }

        //when game starts, npc defaults to idle
        currentState = NPCBehaviors.idle;
    }

    void Update()
    {
        //calculate npc distance to player to determine if they should stop wandering
        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
        animator.SetFloat("Speed", agent.velocity.magnitude);
        switch (currentState)
        {
            case NPCBehaviors.idle:
                HandleIdle(distanceToPlayer);
                break;

            case NPCBehaviors.wandering:
                HandleWandering(distanceToPlayer);
                break;

            case NPCBehaviors.engaged:
                HandleEngaged(distanceToPlayer);
                break;
        }
    }

    void HandleIdle(float distanceToPlayer)
    {
        agent.isStopped = true;

        //if npc is within conversation distance, acknowledge player
        if(distanceToPlayer <= engageDistance)
        {
            EnterEngaged();
            return;
        }

        //if in idle for longer than the random wait duration, start wandering
        waitTimer += Time.deltaTime;
        if(waitTimer >= waitDuration)
        {
            EnterWandering();
        }
    }
    
    void HandleWandering(float distanceToPlayer)
    {
        agent.isStopped = false;
        
        if(distanceToPlayer <= engageDistance)
        {
            EnterEngaged();
            return;
        }

        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            EnterIdle();
        }
    }

    void HandleEngaged(float distanceToPlayer)
    {
        agent.isStopped = true;
        //arrow pointing from npc to the player 
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        
        if(direction != Vector3.zero)
        {   
            //smoothly rotate npc to look at player when within certain distance
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        }

        //if outside of engage distance, just go idle
        if(distanceToPlayer > engageDistance)
        {
            EnterIdle();
        }
    }

    void EnterIdle()
    {
        currentState = NPCBehaviors.idle;
        agent.isStopped = true;
        waitTimer = 0f;
        waitDuration = Random.Range(minWaitTime,  maxWaitTime);
        animator.SetBool("Engaged", false);
    }

    void EnterWandering()
    {
        Vector3 destination = GetRandomNavMeshPoint(transform.position, wanderRadius);
        
        if(Vector3.Distance(transform.position, destination) < 1f)
        {
            EnterIdle();
            return;
        }
        
        agent.isStopped = false; 
        agent.SetDestination(destination);
        currentState = NPCBehaviors.wandering;
        animator.SetBool("Engaged", false);
    }

    void EnterEngaged()
    {
        currentState = NPCBehaviors.engaged;
        agent.isStopped = true; 
        animator.SetBool("Engaged", true);
    }

    Vector3 GetRandomNavMeshPoint(Vector3 start, float radius)
    {
        Vector3 randomPoint = start + Random.insideUnitSphere * radius;
        //want a random point to wander to, but don't want the npc to start floating 
        randomPoint.y = start.y;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return start;
    }
}