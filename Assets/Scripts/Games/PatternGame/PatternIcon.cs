using System;
using UnityEngine;

public class PatternIcon : GeneralIcon
{
    [HideInInspector] public int iconIndex = -1;

    public Action<Collider2D, int> onIconCollided = (otherCollider, currentIconIndex) => { };



    private void OnTriggerEnter2D(Collider2D collision)
    {
        onIconCollided?.Invoke(collision, iconIndex);
    }



    public void OnMouseDown()
    {
        onIconMouseCollided?.Invoke(iconIndex);
    }

    public void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
            onIconMouseCollided?.Invoke(iconIndex);
    }
}
