using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TamiraAI : MonoBehaviour
{
    //npc
    private NavMeshAgent agent; 
    //set to the player
    [SerializeField] private GameObject target; 
    //animator controller 
    private Animator animator; 

    //variables for npc states + checking player
    private enum NPCBehaviors
    {   
        //standing
        idle, 
         //walking around
        wandering,
        //in conversation, or acknowledged player within conversation distance
        engaged

    }

    [SerializeField] private NPCBehaviors currentState;
    [SerializeField] private bool playerInRange;
    
    [Header("NPC Wander/Idle Settings")]
    [SerializeField] private float wanderRadius = 4f;
    [SerializeField] private float wanderingTime = 5f;
    [SerializeField] private float idleTime = 3f;

    void Start()
    {
        //get npc
        agent = GetComponent<NavMeshAgent>();
        if(agent == null)
        {
            Debug.LogError("nav mesh agent is null");
        }
        agent.stoppingDistance = 0.5f;

        //get controller 
        animator = GetComponent<Animator>();

        //get player "target"
        target = GameObject.FindWithTag("Player");
        if(target == null)
        {
            Debug.LogError("player not found");
        }

        //when game starts, npc defaults to idle
        currentState = NPCBehaviors.idle;
        StartCoroutine(WanderingCooldownRoutine());
    }

    void Update()
    {                
        switch (currentState)
        {
            case NPCBehaviors.idle:
                agent.isStopped = true;
                animator.SetFloat("Speed", 0f);
                //npc idle, stopped
                Debug.Log("i'm idle");
                break;

            case NPCBehaviors.wandering:
                agent.isStopped = false;
                animator.SetFloat("Speed", agent.velocity.magnitude);
                //npc is wandering, moving
                Debug.Log("i'm wandering");
                break;

            case NPCBehaviors.engaged:
                agent.isStopped = true; 
                animator.SetFloat("Speed", 0f);
                FacePlayer();
                //npc is stopped, engaged w/ player
                Debug.Log("i'm engaged");
                break;
        }
    }

    //on trigger enter method 
    //check if we hit something tagged player 
    //switch to engaged state 
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInRange = true; 
            currentState = NPCBehaviors.engaged;
            animator.SetBool("Engaged", true);
        }    
    }

    //on trigger exit method 
    //check if player is no longe in range 
    //switch to idle state
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInRange = false;
            animator.SetBool("Engaged", false);
            StartCoroutine(WanderingCooldownRoutine());
        }
    }

    private IEnumerator WanderingCooldownRoutine()
    {       
        currentState = NPCBehaviors.idle;
        yield return new WaitForSeconds(idleTime);

        if(!playerInRange)
        {
            Vector3 destination = GetRandomNavMeshPoint(transform.position, wanderRadius);
            //if distance is too close to npc origin, go idle and don't attempt to walk
            if(Vector3.Distance(transform.position, destination) < 0.5f)
            {
                Debug.Log("too close");
                StartCoroutine(WanderingCooldownRoutine());
                yield break;
            }

            currentState = NPCBehaviors.wandering;
            agent.isStopped = false;
            agent.SetDestination(destination);
            StartCoroutine(IdleCooldownRoutine());
            yield break;
        }
    }

    private IEnumerator IdleCooldownRoutine()
    {       
        yield return new WaitForSeconds(wanderingTime);

        if(!playerInRange)
        {
            currentState = NPCBehaviors.idle;
            StartCoroutine(WanderingCooldownRoutine());
            yield break;
        }
    }

    //generate random point within npc radius to wander to 
    private Vector3 GetRandomNavMeshPoint(Vector3 origin, float radius)
    {
        Vector3 randomPoint = origin + Random.insideUnitSphere * radius; 
        randomPoint.y = origin.y;

        NavMeshHit hit; 
        if(NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return origin; 
    }

    //rotate to face player when engaged
    private void FacePlayer()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;

        if(direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        }
    }
}  