using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Alarm : MonoBehaviour
{
    public event Action OnAlarm;

    public void TriggerAlarm()
    {
        OnAlarm?.Invoke();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TriggerAlarm();
        }
    }
}