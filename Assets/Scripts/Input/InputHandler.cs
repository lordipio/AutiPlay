using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private GameInputActions inputActions;



    private void Awake()
    {
    //    inputActions = new GameInputActions();

    //    inputActions.Gameplay.TouchOrClick.performed += ctx =>
    //    {
    //        Debug.Log("Touch or Click detected!");

    //        Vector2 screenPos = Mouse.current != null ? Mouse.current.position.ReadValue() : Touchscreen.current.primaryTouch.position.ReadValue();
    //        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

    //        Debug.Log("World Position: " + worldPos);
    //        // „À·« »“‰ »Â Raycast
    //    };
    //}

    //private void OnEnable()
    //{
    //    inputActions.Enable();
    //}

    //private void OnDisable()
    //{
    //    inputActions.Disable();
    }
}