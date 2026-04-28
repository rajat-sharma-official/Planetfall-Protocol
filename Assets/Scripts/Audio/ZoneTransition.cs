using UnityEngine;

public class ZoneTransition : MonoBehaviour
{
    //skybox
    [SerializeField] private Material zoneSkybox;

    //audio clip name
    [SerializeField] private string zoneMusic; 

    //audio manager 
    private AudioManager audioManager;
    public static string currentMusic = "";

    private void Awake()
    {
        //object has a collider, and isTrigger is checked
        GetComponent<Collider>().isTrigger = true;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(zoneSkybox != null)
            {
                RenderSettings.skybox = zoneSkybox;
            }

            if(!string.IsNullOrEmpty(zoneMusic))
            {
                audioManager.Stop(currentMusic);
                audioManager.Play(zoneMusic);
                currentMusic = zoneMusic;
            }
        }
    }
}
