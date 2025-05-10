using UnityEngine;
using System.Collections;

public class GeneralGuide : MonoBehaviour
{
    public AnimationCurve cursorMovingCurve;

    public GameObject guideCursor ;
    
    protected CanvasGroup guidePassagecanvasGroup;

    [SerializeField] protected GameObject guidePassage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        guidePassagecanvasGroup = guidePassage.GetComponent<CanvasGroup>();
        if (guidePassagecanvasGroup != null)
            guidePassagecanvasGroup.alpha = 0f;

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


    protected IEnumerator FadeInGuidePassage(float speed)
    {
        if (guidePassagecanvasGroup)
            while (guidePassagecanvasGroup.alpha < 1)
            {
                guidePassagecanvasGroup.alpha += speed * Time.deltaTime;
                yield return null;
            }
        else
        guidePassagecanvasGroup.alpha = 1;
    }


    protected IEnumerator FadeOutGuidePassage(float speed)
    {
        if (guidePassagecanvasGroup)
            while (guidePassagecanvasGroup.alpha > 0)
            {
                guidePassagecanvasGroup.alpha -= speed * Time.deltaTime;
                yield return null;
            }

        guidePassagecanvasGroup.alpha = 0;
    }
}
