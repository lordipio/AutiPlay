using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class MatchingIcon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject holderKnob;
    private CircleCollider2D circleCollider;
    [HideInInspector] public int iconIndex = -1;

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

    public void SetKnobToTheTop()
    {
        holderKnob.transform.localPosition = new Vector3(0, -3.5f, 0);
    }

    public void SetKnobToTheButtom()
    {
        holderKnob.transform.localPosition = new Vector3(0, 3.5f, 0);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void OnMouseDown() // touch
    {
        onIconCollided?.Invoke(iconIndex);
    }

    public void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
           onIconCollided?.Invoke(iconIndex);
    }

}
