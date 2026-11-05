using System;
using UnityEngine;


public class SignalDemoEnd : MonoBehaviour
{
    public static event Action OnEnterDemoEnd;
    public static event Action OnExitDemoEnd;

    private void Start()
    {
        Debug.Log("Entering Demo End");
        OnEnterDemoEnd?.Invoke();
    }

    private void OnDestroy()
    {
        Debug.Log("Exiting Demo End");
        OnExitDemoEnd?.Invoke();
    }
}
