using System;
using UnityEngine;

public class SignalBossLevel : MonoBehaviour
{
    public static event Action OnEnterBossLevel;
    public static event Action OnExitBossLevel;

    private void Start()
    {
        Debug.Log("Entering Boss Level");
        OnEnterBossLevel?.Invoke();
    }

    private void OnDestroy()
    {
        Debug.Log("Exiting Boss Level");
        OnExitBossLevel?.Invoke();
    }
}
