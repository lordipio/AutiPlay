using UnityEngine;

public class SortingIcon : GeneralIcon
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        onIconCollided?.Invoke(-1);
    }
}
