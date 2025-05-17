using UnityEngine;
using System.Collections;

public class GeneralGuide : MonoBehaviour
{
    public AnimationCurve cursorMovingCurve;
    public GameObject guideCursor ;

    [SerializeField] protected GameObject guidePassage;

    protected CanvasGroup guidePassagecanvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        guidePassagecanvasGroup = guidePassage.GetComponent<CanvasGroup>();

        if (guidePassagecanvasGroup != null)
            guidePassagecanvasGroup.alpha = 0f;

        guideCursor = Instantiate<GameObject>(guideCursor);
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
            guideCursor.gameObject.transform.position = Vector3.Lerp(initPos, finalPos, curvedT);
            yield return null;
        }
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
