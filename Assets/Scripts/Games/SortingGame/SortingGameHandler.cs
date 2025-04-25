using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortingGameHandler : MonoBehaviour
{
    [SerializeField] GameObject matchingIconGO;

    [SerializeField] const int iconsCount = 4;

    List<SortingIcon> sortingIcons = new List<SortingIcon>();

    List<Color> colors = new List<Color>();

    SortingIcon targetIcon;

    Coroutine collideCoroutine;

    Vector3 characterInitialPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color customizedBlue = new Color(0.4f, 0.4f, 1f);

        colors = new List<Color> { customizedBlue, Color.green, Color.red, Color.blue };
        
        characterInitialPosition = SortingGameCharacter.instance.transform.position;

        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, SpawnIcons));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnIcons()
    {
        print("START SPAWN ICONS!");

        targetIcon = null;

        List<Vector2> Positions = SpawnUtility.GetUpDividedPositions(iconsCount);

        GameObject tempSortingIconGO;

        SortingIcon tempSortingIcon;

        sortingIcons = new List<SortingIcon>();

        Texture2D[] allTextures = SpawnUtility.LoadTextures("Fruits");

        List<int> texturesRandomSelection = SpawnUtility.RandomNumberGenerator(0, allTextures.Length - 1, iconsCount + 1);

        List<int> randomColorIndex = SpawnUtility.RandomNumberGenerator(0, 4, 4);

        List<Texture2D> textures = new List<Texture2D>();

        foreach (int i in texturesRandomSelection)
        {
            textures.Add(allTextures[i]);
        }

        int temp = 0;
            

        int targetIconIndex = randomColorIndex[Random.Range(0, 4)];


        foreach (Vector2 position in Positions)
        {
            tempSortingIconGO = Instantiate<GameObject>(matchingIconGO, position, Quaternion.identity);

            tempSortingIcon = tempSortingIconGO.GetComponent<SortingIcon>();

            Texture2D tex = textures[temp];

            tempSortingIcon.spriteRenderer.sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f));

            tempSortingIcon.spriteRenderer.color = colors[randomColorIndex[temp]];

            sortingIcons.Add(tempSortingIcon);

            if (randomColorIndex[temp] == targetIconIndex)
                targetIcon = tempSortingIcon;

            else
                tempSortingIcon.circleCollider.enabled = false;

            temp++;
            print(temp);
        }

        SortingGameCharacter.instance.spriteRenderer.color = colors[targetIconIndex];

        SortingGameCharacter.instance.onCharacterCollidedEvent += OnCharacterCollided;
    }

    void OnCharacterCollided(Collider2D collision)
    {
        if (collision == targetIcon.circleCollider)
        {
            if (collideCoroutine == null)
                collideCoroutine = StartCoroutine(RemoveObjects());
        }
    }

    IEnumerator RemoveObjects()
    {
        yield return new WaitForSeconds(2f);

        if (sortingIcons.Count > 0)
            foreach (SortingIcon sortingIcon in sortingIcons)
            {
                GameObject.Destroy(sortingIcon.gameObject);
                print("REMOVING ICONS");
            }

        collideCoroutine = null;

        SortingGameCharacter.instance.transform.position = characterInitialPosition;

        yield return new WaitForSeconds(1f);

        SpawnIcons();
    }
}
