using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CentralInputHandler : MonoBehaviour
{
    public static CentralInputHandler Instance { get; private set; }
    public event Action<Vector2> OnDrag;
    public event Action<Vector2> OnPress;
    public event Action OnRelease;

    private GameInputActions inputActions;
    private bool isDragging = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputActions = new GameInputActions();

        inputActions.Gameplay.Press.performed += ctx =>
        {
            isDragging = true;
            Vector2 pos = GetScreenPosition();
            OnPress?.Invoke(Camera.main.ScreenToWorldPoint(pos));
        };

        inputActions.Gameplay.Press.canceled += ctx =>
        {
            isDragging = false;
            OnRelease?.Invoke();
        };

        inputActions.Gameplay.Drag.performed += ctx =>
        {
            if (!isDragging) return;
            Vector2 pos = ctx.ReadValue<Vector2>();
            OnDrag?.Invoke(Camera.main.ScreenToWorldPoint(pos));
        };
    }

    private Vector2 GetScreenPosition()
    {
        if (Touchscreen.current != null)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        else if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        return Vector2.zero;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
}
