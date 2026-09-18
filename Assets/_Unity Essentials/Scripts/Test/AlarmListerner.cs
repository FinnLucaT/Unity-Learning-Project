using UnityEngine;

public class AlarmListener : MonoBehaviour
{
    [SerializeField] private Alarm alarm;

    private void OnEnable()
    {
        alarm.OnAlarm += TurnOnLight;
    }

    private void TurnOnLight()
    {
        Debug.Log("Licht an!");
    }

    private void OnDisable()
    {
        alarm.OnAlarm -= TurnOnLight;
    }
}