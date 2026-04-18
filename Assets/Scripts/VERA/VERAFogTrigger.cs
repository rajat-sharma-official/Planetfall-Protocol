using UnityEngine;

public class VERAFogTrigger : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private VERAPopupController popupController; // for popup related stuff
    [SerializeField] private VERAMenu veraMenu; // for vera menu (showing the message)

    [Header("Requirements")]
    [SerializeField] private PlayerInventory playerInventory; // checking scrap count so we dont trigger when not needed
    [SerializeField] private int requiredScrap = 30; // set required scrap amount to trigger the hint, if onlyTriggerIfScrapMissing is true
    [SerializeField] private bool onlyTriggerIfScrapMissing = true; // if true, the hint will only trigger if the player has less than requiredScrap amount of scrap, preventing unnecessary hints when the player is already well-equipped

    [Header("Settings")]
    [SerializeField] private bool hintEnabled = true;
    [SerializeField] private bool triggerOnlyOnce = false; // if true, the trigger will only activate the first time the player enters, and will not trigger again on subsequent entries, even if they leave and re-enter the area. This is useful for hints that are meant to be seen only once to avoid redundancy.
    [SerializeField] private float retriggerCooldown = 20f;


    private bool hasTriggered = false; // to track if the trigger has already been activated, preventing multiple triggers if triggerOnlyOnce is true
    private float lastTriggerTime = -999f;
    private int lastHintIndex = -1;

    [SerializeField] private VERAFogPathController fogManager;
    [SerializeField] private Transform scrapTarget;

    private readonly string[] fogGuideHints = // list of hints to make it more like "AI" since we are using same panel.
    {
        "The fog density here is high enough to reduce visibility to almost nothing, and I would prefer not to lose you in it. I ran a forward scan and detected signs of salvage or other remnants deeper inside. Hold still for a moment and I will illuminate a safer path.",

        "This section is heavily obscured, Atlas. I know it feels like walking into a wall, but my readings suggest there is something worth investigating beyond the fog. Let me light the way so you can move through it without guessing.",

        "Visibility is extremely limited here, and yes, that is as unsettling as it feels. I checked the terrain ahead and picked up traces that could indicate scrap or abandoned technology nearby. Give me a second and I will mark a path forward.",

        "This fog is thick enough to hide just about anything unpleasant, which is not my favorite category of problem. That said, I am reading fragments ahead that may be worth searching. I can illuminate a route if you are ready to continue.",

        "You can barely see more than a few steps ahead, and I am aware that this is not improving morale. My scans show possible salvage signatures further in, along with a navigable route. Let me light the path before you move.",

        "This area is visually compromised to the point of being actively rude. I ran a sweep ahead and found traces of scrap or older remnants inside the fog. Stay close and I will illuminate a path that gives us better odds than blind optimism.",

        "The visibility in this pocket is dangerously low, Atlas. I understand if your first instinct is to avoid it. Mine would be too, if I had legs. But my scans suggest there is something of value deeper inside, so I am going to light a route for you.",

        "This area is almost entirely swallowed by fog, and I would be concerned even if I were not responsible for your continued survival. I swept ahead and found possible remnants worth investigating. Give me a moment and I will illuminate the path.",

        "I know this looks like the kind of place sensible people avoid, and under ordinary circumstances I might agree. However, I detected signs of scrap or structural remains further in. Let me mark the clearest route so you can actually see where you are going.",

        "The fog here is thick enough to turn a simple search into a small nightmare. I checked ahead anyway, and there are readings that suggest this area is worth exploring. I can project a path for you if you would like to survive this efficiently.",

        "This is poor visibility, unstable terrain, and dreadful ambience all in one convenient location. Still, my scans indicate something may be hidden inside the fog, possibly salvageable material. Stay where you are and I will light the way.",

        "I would not recommend walking blind into this on instinct alone. Thankfully, instinct is not the only tool available to us. I have scanned ahead and found enough evidence of remnants to justify a closer look. Let me illuminate a path first.",

        "The fog is thick enough here to make the ground ahead feel unknowable, which I realize is not comforting. My readings do show something deeper inside that may help us, likely scrap or another remnant. I will light a path so we do not have to rely on luck.",

        "This area is visually compromised to the point of being actively unpleasant. I ran a forward check and found indications that there may be useful debris hidden in the fog. Let me reveal a path before you take another step.",

        "You are right to hesitate here. The visibility is terrible, and this place was clearly designed by something with a grudge against confidence. Still, I detected possible salvage deeper inside. I can illuminate a route if you want to press on.",

        "The fog is doing an excellent job of making this seem like a terrible idea. Unfortunately for it, my scans suggest there is something ahead worth finding. Stand by and I will light a path through the worst of it.",

        "I am reading near-total visual obstruction in this section, along with faint returns that may indicate scrap, wreckage, or some other remnant. In other words, it is frightening, but potentially useful. Let me illuminate the route forward.",

        "This fog is dense enough that even a careful search would become guesswork. I have already looked ahead and found signs that exploring further may reward the effort. I will mark a path so you can move through it with something resembling confidence.",

        "The atmosphere here is unsettling for a reason. Visibility is poor, the terrain is obscured, and your instincts are correctly warning you to be careful. Even so, I have picked up readings deeper inside that suggest useful remnants. Allow me to illuminate a path before we proceed.",

        "I know this area feels wrong. The fog is thick, the silence is worse, and there is very little for you to trust with your own eyes. Fortunately, I ran a forward scan and found traces of salvage within it. Stay calm and I will light the safest route I can.",

        "There is enough fog here to make every direction look like a bad decision. Fortunately, I ran a few checks ahead and found what may be useful debris or salvage. Let me highlight a path before you wander into something regrettable.",

        "The visibility in here is so poor that even your bad ideas would have trouble finding you. I ran a few checks and there is something worth searching for beyond the fog. I will light the path, since optimism is not a navigation strategy.",

        "This place has all the charm of a trap and none of the courtesy of warning signs. I scanned through the fog and picked up remnants that may be worth collecting. Let me light the path before you confidently step into the wrong direction.",

        "The fog is obscuring the terrain with almost theatrical commitment. Even so, I ran a scan ahead and found enough to justify exploring it. I will illuminate a route for you, because wandering blindly would reflect poorly on both of us.",

        "This fog is too dense to trust and too quiet to ignore. I scanned ahead and found traces of something buried deeper inside it, scrap perhaps, or what remains after time has finished with it. Stay close, and I will light a path through the dark.",

        "The visibility here falls away like the world is being swallowed a few steps at a time. I looked ahead and found remnants hidden in the haze, enough to draw us forward whether I like it or not. Let me illuminate the route before the fog decides to keep us.",

        "There is something unsettling about this place, Atlas. The fog is thick, the terrain is unreadable, and yet my scans keep returning fragments from somewhere ahead. I am marking a path now. Follow it, and try not to listen too closely to the silence.",

        "You are not imagining it. This area is difficult to read, difficult to trust, and exactly the sort of place people disappear in stories. Still, I found signs of salvage beyond the fog. Hold position and I will illuminate a path for you.",

        "The fog here is dangerous, the visibility is poor, and I am choosing to interpret your continued presence as bravery rather than questionable judgment. I checked ahead and found likely remnants inside. Let me light the route so this becomes exploration instead of stumbling.",

        "I would strongly prefer that you not walk into this blind. The good news is that I have already scanned ahead and found something that may be worth the risk, most likely salvage or older debris. Give me a moment and I will illuminate a path you can follow."
    };

    private void Awake() // preventing manual assignment if forgotten
    {
        if (popupController == null)
            popupController = FindFirstObjectByType<VERAPopupController>();

        if (veraMenu == null)
            veraMenu = FindFirstObjectByType<VERAMenu>();

        if (playerInventory == null)
            playerInventory = FindFirstObjectByType<PlayerInventory>();

        if (fogManager == null)
            fogManager = FindFirstObjectByType<VERAFogPathController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hintEnabled) // if the hint is disabled, do nothing. This allows the trigger to be reused for other purposes or to be turned off if the player has already seen it and doesn't want reminders.
            return;

        if (!other.CompareTag("Player")) // only trigger for the player, not other colliders
            return;

        if (triggerOnlyOnce && hasTriggered)
            return;
    
        if (scrapTarget == null)
            return;

        if (!scrapTarget.gameObject.activeInHierarchy)
        {
            hintEnabled = false;
            hasTriggered = true;
            return;
        }

        if (ScrapManager.Instance != null && ScrapManager.Instance.IsScavenged(scrapTarget.gameObject))
        {
            hintEnabled = false;
            hasTriggered = true;
            return;
        }

        if (Time.time < lastTriggerTime + retriggerCooldown)
            return; // if the player recently trigger, do NOT trigger again

        if (onlyTriggerIfScrapMissing && playerInventory != null && playerInventory.Scrap >= requiredScrap)
            return; // if the player has enough scrap, they likely don't need the hint, so we skip triggering to avoid unnecessary hints



        ShowHint();
        if (fogManager != null && scrapTarget != null)
        {
            fogManager.IlluminatePathTo(scrapTarget);
        }

        hasTriggered = true;
        lastTriggerTime = Time.time;
    }

    private void ShowHint()
    {
        string message = GetRandomHint();

        if (popupController != null) // generate random hint and show msg
            popupController.ShowPopup();

        if (veraMenu != null)
            veraMenu.GetSystemMessage(message);
    }

    private string GetRandomHint()
    {
        if (fogGuideHints == null || fogGuideHints.Length == 0) // if issues for hints 
            return "Visibility is poor here. I scanned ahead and found something worth investigating. Let me illuminate a path for you.";

        if (fogGuideHints.Length == 1)
            return fogGuideHints[0];

        int newIndex;
        do
        {
            newIndex = Random.Range(0, fogGuideHints.Length);
        }
        while (newIndex == lastHintIndex);

        lastHintIndex = newIndex;
        return fogGuideHints[newIndex];
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }

    public void SetHintEnabled(bool enabled)
    {
        hintEnabled = enabled;
    }
}