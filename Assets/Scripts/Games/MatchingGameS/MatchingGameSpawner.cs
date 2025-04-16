using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class MatchingGameSpawner : MonoBehaviour
{

    [SerializeField] GameObject matchingIconGO;
    List<MatchingIcon> topMatchingIcon = new List<MatchingIcon>();
    List<MatchingIcon> buttomMatchingIcon = new List<MatchingIcon>();


    void Start()
    {
        List<Vector2> Positions = SpawnUtility.GetUpDividedPositions(4);
        
        GameObject tempMatchingIconGO;

        MatchingIcon tempMatchingIcon;

        Texture2D[] allTextures = SpawnUtility.LoadTextures(SpawnUtility.IconTypes.Animals);

        List<int> texturesRandomSelection = SpawnUtility.RandomNumberGenerator(0, allTextures.Length - 1, 5);

        List<Texture2D> textures = new List<Texture2D>();

        foreach(int i in texturesRandomSelection)
        {
            textures.Add(allTextures[i]);
        }

        int temp = 0;

        foreach (Vector2 position in Positions)
        {
            tempMatchingIconGO = Instantiate<GameObject>(matchingIconGO, position, Quaternion.identity);

            tempMatchingIcon = tempMatchingIconGO.GetComponent<MatchingIcon>();

            Texture2D tex = textures[temp];

            tempMatchingIcon.spriteRenderer.sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f) );
            tempMatchingIcon.SetKnobToTheTop();
            tempMatchingIcon.iconIndex = temp;
            topMatchingIcon.Add(tempMatchingIcon);
            temp++;
        }

        Positions = SpawnUtility.GetDownDividedPositions(4);

        temp = 0;

        texturesRandomSelection = SpawnUtility.RandomNumberGenerator(0, 4, 5);

        foreach (Vector2 position in Positions)
        {
            tempMatchingIconGO = Instantiate<GameObject>(matchingIconGO, position, Quaternion.identity);

            tempMatchingIcon = tempMatchingIconGO.GetComponent<MatchingIcon>();

            Texture2D tex = textures[texturesRandomSelection[temp]];

            tempMatchingIcon.SetKnobToTheButtom();

            tempMatchingIcon.iconIndex = temp; // change it

            tempMatchingIcon.spriteRenderer.sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f));

            buttomMatchingIcon.Add(tempMatchingIcon);
            temp++;
        }
    }
}
