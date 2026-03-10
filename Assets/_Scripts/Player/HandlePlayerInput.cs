using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlayerInput : MonoBehaviour
{
    public InputActionAsset InputActions;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }
}
