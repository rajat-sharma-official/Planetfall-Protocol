using UnityEngine;

public class SpaceshipMover : MonoBehaviour
{
    private float speed = 20f;
    private float wobbleAmplitude = 2.5f;
    private float wobbleFrequency = 1f;
    private float leftBound = -160f;
    private float rightBound = 160f;

    private float startY;
    private float wobbleStartTime;

    void Start()
    {
        startY = Random.Range(60f, 130f);
        wobbleStartTime = Time.time;
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        float localTime = Time.time - wobbleStartTime;
        float wobble = Mathf.Sin(localTime * wobbleFrequency) * wobbleAmplitude;
        float drift = Mathf.Sin(localTime * 0.4f) * 5f;
        transform.position = new Vector3(transform.position.x, startY + wobble + drift, transform.position.z);

        float roll = Mathf.LerpUnclamped(-90f, -110f, (Mathf.Sin(localTime * wobbleFrequency) + 1f) / 2f);
        transform.rotation = Quaternion.Euler(0f, 90f, roll);

        if (transform.position.x > rightBound)
        {
            startY = Random.Range(60f, 130f);
            wobbleStartTime = Time.time; // reset wobble timing
            transform.position = new Vector3(leftBound, startY, transform.position.z);
        }
    }
}