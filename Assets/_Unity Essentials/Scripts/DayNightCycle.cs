using UnityEngine;

public class DayNightCycle : MonoBehaviour
{


    [SerializeField]
    private float dayDurationInSeconds = 120f;


    private void Update()
    {
        // Berechne die Rotation pro Sekunde
        // Ein kompletter Tag = 360 Grad / dayDurationInSeconds
        float rotationPerSecond = 360f / dayDurationInSeconds;

        // Drehe das Directional Light um die X-Achse
        transform.Rotate(-rotationPerSecond * Time.deltaTime, 0, 0, Space.Self);
    }
}