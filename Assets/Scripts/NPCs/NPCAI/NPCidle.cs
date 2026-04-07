using UnityEngine;
using UnityEngine.AI;

/* this script is used when:
   an npc doesn't walk around, just has an idle animation
   and you still want the npc to turn and face the player when "engaged"
*/

public class NPCidle : MonoBehaviour
{
    private GameObject target; 
    private Animator animator; 

    [SerializeField] private bool playerInRange;

    void Start()
    {
        //get controller 
        animator = GetComponent<Animator>();

        //get player "target"
        target = GameObject.FindWithTag("Player");
        if(target == null)
        {
            Debug.LogError("player not found");
        }
    }

    void Update()
    {                
        if(playerInRange)
        {
            FacePlayer();
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
            //for npcs that do have a diff animation for engaged
            animator.SetBool("Engaged", false);
        }
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