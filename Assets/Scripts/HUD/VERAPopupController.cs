using UnityEngine;
using TMPro;

// this script controls the little vera popup on the hud, including showing it, hiding it, and playing a sound when it appears
public class VERAPopupController : MonoBehaviour
{
    // ain popup object that gets shown or hidden. if left empty, the script will use the object it is attached to
    [SerializeField] private GameObject popupRoot;
    // for optional text in the future
    // name of sound in audiomanager
    [SerializeField] private string popupSoundName = "VERAAlert";

    // makesure popuproot always points to valid obj 
    private void Awake()
    {
        // if no popup object was assigned in the inspector, use the current game object instead
        if (popupRoot == null)
            popupRoot = gameObject;
    }

    // hide popup at the beginning bc we dont need it until the player collects enough scrap, and it looks weird if it starts on screen
    private void Start()
    {
        // turn the popup off at the start of the game
        if (popupRoot != null)
            popupRoot.SetActive(false);
    }

    // call this when you want the vera popup to appear
    public void ShowPopup(string message = "")
    {  
        // make the popup visible on screen
        if (popupRoot != null)
            popupRoot.SetActive(true);


        // ask the audio manager to play the popup sound. the ? means it will safely do nothing if no audio manager is found
        // adds grace fallback
        FindObjectOfType<AudioManager>()?.Play(popupSoundName);
    }

    // call this when you want the popup to disappear
    public void HidePopup()
    {
        // turn the popup off so it is no longer visible
        if (popupRoot != null)
            popupRoot.SetActive(false);
    }
}