using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public Sprite icon;
    public Image image;
    public Vector2 position
    {
        get {return new Vector2 (transform.position.x, transform.position.z); } 
    }

}
