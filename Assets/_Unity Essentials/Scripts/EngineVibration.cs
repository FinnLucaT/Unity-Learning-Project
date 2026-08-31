using UnityEngine;

public class EngineVibration : MonoBehaviour
{
    [SerializeField] private float vibrationStrength = 0.01f;
    [SerializeField] private float vibrationSpeed = 35f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float vibration = Mathf.Sin(Time.time * vibrationSpeed) * vibrationStrength;

        transform.localPosition = startPosition + new Vector3(0, vibration, 0);
    }
}