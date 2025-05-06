using UnityEngine;
using System.Collections;

public class GeneralGuide : MonoBehaviour
{
    public AnimationCurve cursorMovingCurve;

    public GameObject guideCursor ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        guideCursor = Instantiate<GameObject>(guideCursor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected IEnumerator MoveCursor(Vector3 initPos, Vector3 finalPos, float speed)
    {
        guideCursor.gameObject.transform.position = initPos;

        float maxDist = Vector3.Distance(initPos, finalPos);
        float duration = maxDist / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float curvedT = cursorMovingCurve.Evaluate(t);

            // حرکت نرم بین init و final با منحنی
            guideCursor.gameObject.transform.position = Vector3.Lerp(initPos, finalPos, curvedT);

            yield return null;
        }

        // در پایان مطمئن شو دقیقاً روی finalPos وایسه
        // guideCursor.gameObject.transform.position = finalPos;
    }
}
