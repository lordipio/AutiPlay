using System;
using UnityEngine;

public class SortingGameCharacter : MonoBehaviour
{
    public FixedJoystick joystick;
    public SpriteRenderer spriteRenderer;
    public delegate void OnCharacterCollided(Collider2D collision);
    public OnCharacterCollided onCharacterCollidedEvent;
    public float moveSpeed = 10f;
    public static SortingGameCharacter instance;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    
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

            if (!AudioHandler.instance.generalAudioSource.isPlaying)
                AudioHandler.instance.PlayLadybugMovementSound();

            animator.SetBool("IsMoving", true);
        }

        else
        {
            animator.SetBool("IsMoving", false);
            AudioHandler.instance.StopLadybugMovementSound();
        }

        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);
        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCharacterCollidedEvent?.Invoke(collision);
    }
}
