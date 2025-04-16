using UnityEngine;

public class MatchingIcon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject holderKnob;
    [HideInInspector] public int iconIndex = -1;
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
}
