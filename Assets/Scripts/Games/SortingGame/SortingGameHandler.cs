using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;
using static UnityEngine.GraphicsBuffer;


public class SortingGameHandler : MonoBehaviour
{

    public Material transitionMaterial;
    public float transitionTime = 2f;

    private float currentRadius = 1.0f;

    private float maxRadius = 1.0f;

    // private bool closing = true;




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

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(SortingGameCharacter.instance.transform.position);
        transitionMaterial.SetVector("_Center", new Vector4(screenPos.x, screenPos.y, 0, 0));
    }

    private void SpawnIcons()
    {
        float aspect = (float)Screen.width / Screen.height;
        transitionMaterial.SetFloat("_Aspect", aspect);

        if (aspect < 1.4)
            transitionMaterial.SetFloat("_Radius", currentRadius * 0.75f);

        else
            transitionMaterial.SetFloat("_Radius", currentRadius);

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
        Coroutine fadeInCoroutine = StartCoroutine(FadeTransition(true));

        yield return new WaitForSeconds(2f);

        if (sortingIcons.Count > 0)
            foreach (SortingIcon sortingIcon in sortingIcons)
            {
                GameObject.Destroy(sortingIcon.gameObject);
                print("REMOVING ICONS");
            }

        collideCoroutine = null;

        StopCoroutine(fadeInCoroutine);
        
        fadeInCoroutine = null;


        yield return new WaitForSeconds(1f);

        SortingGameCharacter.instance.transform.position = characterInitialPosition;

        SpawnIcons();

        StartCoroutine(FadeTransition(false));

    }


    IEnumerator FadeTransition(bool closing)
    {
        while( true )
        {
            if (currentRadius < 0 && closing)
            {
                currentRadius = 0;
                break;
            }

            if (currentRadius > maxRadius && !closing)
            {
                currentRadius = maxRadius;
                break;
            }


            // نسبت عرض به ارتفاع صفحه
            float aspect = (float)Screen.width / Screen.height;
            transitionMaterial.SetFloat("_Aspect", aspect);
        
            // بقیه کدت که Center و Radius رو آپدیت میکنی
            Vector3 screenPos = Camera.main.WorldToViewportPoint(SortingGameCharacter.instance.transform.position);
            transitionMaterial.SetVector("_Center", new Vector4(screenPos.x, screenPos.y, 0, 0));

            //// آپدیت مرکز سوراخ
            //Vector3 screenPos = mainCamera.WorldToViewportPoint(target.position);
            //transitionMaterial.SetVector("_Center", new Vector4(screenPos.x, screenPos.y, 0, 0));

            // آپدیت اندازه سوراخ
            float delta = Time.deltaTime / transitionTime;
            if (closing)
                currentRadius -= delta;
            else
                currentRadius += delta;


            if (aspect < 1.4)
            transitionMaterial.SetFloat("_Radius", currentRadius * 0.75f);

            else
                transitionMaterial.SetFloat("_Radius", currentRadius);

            yield return null;      
        }
    }

    void Kosher()
    {

    }
}
