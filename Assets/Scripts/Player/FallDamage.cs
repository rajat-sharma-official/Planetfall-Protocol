using UnityEngine;
// simple script for jump/fall damage
public class FallDamage : MonoBehaviour
{
    // if player is slower than this no dmg, if faster than maxSpeed then max dmg, in between is scaled
    [SerializeField] private float safeSpeed = 12f;
    [SerializeField] private float maxSpeed = 22f;
    [SerializeField] private float maxDamage = 70f;
    [SerializeField] private GameObject fallImpactPrefab; // for effects 
    // references to other player scripts
    private PlayerController player;
    private PlayerHealth health;
    private CharacterController controller;
    // new hue!
    private HazardRedHueUI redHueUI;

    // used to detect when we land
    private bool wasOnGroundLastFrame;
    // storest velo
    private float lowestVelocity;
    private float peakYValue;
    private bool isFalling;

    void Start()
    {
        // get references
        player = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
        controller = GetComponent<CharacterController>();
        wasOnGroundLastFrame = true;
        lowestVelocity = 0f;

        if(HazardWarningUI.Instance != null)
            redHueUI = HazardWarningUI.Instance.RedHue;
    }

    void Update()
    {
        bool grounded = controller.isGrounded;
        // if we are in air, track the lowest (most negative) vertical velocity we reach
        if (!grounded)
        {
            if (wasOnGroundLastFrame)
            {
                lowestVelocity = 0f; // reset when we first leave the ground
                peakYValue = transform.position.y;
                isFalling = false;
            }
            if (transform.position.y > peakYValue)
                peakYValue = transform.position.y;
            if (player.velocity.y < lowestVelocity)
                lowestVelocity = player.velocity.y;
            if (player.velocity.y < 0)
                isFalling = true;
        }
        // grab our landing event - if we were in the air last frame but are grounded now, we just landed
        if (!wasOnGroundLastFrame && grounded)
        {
            if (isFalling)
            {
                float heightDropped = peakYValue - transform.position.y;
                float impactSpeed = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * Mathf.Max(0f, heightDropped));
                // only apply damage if strong enough!!!!
                if (impactSpeed > safeSpeed)
                {
                    if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3f))
                    {
                        Instantiate(fallImpactPrefab, hit.point, Quaternion.identity);
                    }
                    // convert fall speed, 0-1 range via lerp
                    float t = Mathf.InverseLerp(safeSpeed, maxSpeed, impactSpeed);
                    // scaling damage by how much damage they get
                    float damage = Mathf.Pow(t, 0.6f) * maxDamage;
                    health.TakeDamage(damage);

                    //play sfx, flash red, etc
                    redHueUI?.DoFlashingDamage(t);
                    FindObjectOfType<AudioManager>().Play("FallDamage");
                }
            }
            // reset for the next fall
            lowestVelocity = 0f;
            isFalling = false;
        }
        // remember if we were on the ground for the next frame
        wasOnGroundLastFrame = grounded;
    }
}