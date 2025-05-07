using UnityEngine;
using TMPro;
using System.Collections;

public class MatchAutoFontSize : MonoBehaviour
{
    public TMP_Text[] texts;

    void Awake()
    {
        StartCoroutine(SetFontsSize());
    }

    IEnumerator SetFontsSize()
    {
        for (int i = 0; i < 5; i++)
            yield return new WaitForEndOfFrame();

        float minFontSize = float.MaxValue;

        foreach (var txt in texts)
        {
            minFontSize = Mathf.Min(minFontSize, txt.fontSize);
        }

        foreach (var txt in texts)
        {
            txt.enableAutoSizing = false;
            txt.fontSize = minFontSize;
        }
    }
}
