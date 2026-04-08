using UnityEngine;

public class SpaceshipMover : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 20f;
    public float wobbleAmplitude = 2.5f;
    public float wobbleFrequency = 1f;

    [Header("Boundaries")]
    public float leftBound = -180f;
    public float rightBound = 180f;

    private float startY;

    void Start()
    {
        startY = Random.Range(80f, 120f);
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }

    void Update()
    {
        // Move forward
        transform.position += Vector3.right * speed * Time.deltaTime;

        // Gentle vertical wobble
        float wobble = Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
        float drift = Mathf.Sin(Time.time * 0.4f) * 5f;
        transform.position = new Vector3(transform.position.x, startY + wobble + drift, transform.position.z);

        // Slight roll with the wobble for style
        float roll = Mathf.LerpUnclamped(-90f, -110f, (Mathf.Sin(Time.time * wobbleFrequency) + 1f) / 2f);
        transform.rotation = Quaternion.Euler(0f, 90f, roll);

        // Reset to other side when past right boundary
        if (transform.position.x > rightBound)
        {
            startY = Random.Range(80f, 120f);
            transform.position = new Vector3(leftBound, startY, transform.position.z);
        }
    }
}