using UnityEngine;
using TMPro;

public class MatchAutoFontSize : MonoBehaviour
{
    public TMP_Text[] texts;

    void Start()
    {
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
