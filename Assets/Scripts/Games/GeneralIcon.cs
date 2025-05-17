using UnityEngine;
// using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class GeneralIcon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;
    public delegate void OnIconMouseCollided(int iconIndex);
    public OnIconMouseCollided onIconMouseCollided;
}
