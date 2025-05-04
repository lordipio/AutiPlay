using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MatchingIcon : GeneralIcon
{
    public GameObject holderKnob;
    [HideInInspector] public int iconIndex = -1;


    public void SetKnobToTheTop()
    {
        holderKnob.transform.localPosition = new Vector3(0, -3.5f, 0);
    }

    public void SetKnobToTheButtom()
    {
        holderKnob.transform.localPosition = new Vector3(0, 3.5f, 0);
    }


    public void OnMouseDown() 
    {
        // onIconMouseCollided?.Invoke(iconIndex);
    }

    public void OnMouseEnter()
    {
        //if (Input.GetMouseButton(0))
          // onIconMouseCollided?.Invoke(iconIndex);
    }

    //public void on
}
