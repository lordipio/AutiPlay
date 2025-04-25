using System;
using UnityEngine;

public class SortingGameCharacter : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;
    [SerializeField] Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public delegate void OnCharacterCollided(Collider2D collision);
    public OnCharacterCollided onCharacterCollidedEvent;


    public float moveSpeed = 10f;

    public static SortingGameCharacter instance;
    
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        Vector3 direction = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);

        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCharacterCollidedEvent?.Invoke(collision);

    }
}
