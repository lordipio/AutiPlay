using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class GeneralIcon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;

    public delegate void OnIconCollided(int iconIndex);
    public OnIconCollided onIconCollided;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
