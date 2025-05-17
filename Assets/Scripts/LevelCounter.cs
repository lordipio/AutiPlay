using RTLTMPro;
using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    public static LevelCounter instance;

    [SerializeField] RTLTextMeshPro levelText;

    int level = 1;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else
            Destroy(instance);
    }

    public void LevelUp()
    {
        level++;
        levelText.text = level.ToString();
    }
}
