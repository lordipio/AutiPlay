using System;
using UnityEngine;

public class PatternIcon : GeneralIcon
{
    public Action<Collider2D, int> onIconCollided = (otherCollider, currentIconIndex) => { };
    
    [HideInInspector] public int iconIndex = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onIconCollided?.Invoke(collision, iconIndex);
    }
}
