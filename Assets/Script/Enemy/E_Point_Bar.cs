using UnityEngine;
using System.Collections;

public class E_Point_Bar : MonoBehaviour
{
    [SerializeField]
    bool isShield = false;

    const float interval = 1.0f;

    GameObject[] arrPointSprite = null;

    int curDispPoint = 0;
    int _curPoint = 0;
    float accumeTime = 0;

    bool isShow = false;
    
    
    GameObject CreateOnePointBar(int idx)
    {
        GameObject obj = new GameObject("hpSprite");
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(0.6f, 0.8f, 1);
        obj.transform.localPosition = new Vector3(1.8f * idx, 0);
        SpriteRenderer render = obj.AddComponent<SpriteRenderer>();

        if (!isShield)
            render.sprite = ImgResourcesContainer.Inst.enemyHP;
        else
            render.sprite = ImgResourcesContainer.Inst.enemyShield;

        return obj;
    }

    public void RequestShow(int curPoint, int reducePoint)
    {
        Show();

        if (arrPointSprite == null)
        {
            CreateBar(curPoint);
        }

        _curPoint = curPoint - reducePoint;

        for (; curDispPoint > _curPoint - 1; curDispPoint--)
        {
            if (curDispPoint < 0)
                break;

            StartCoroutine(DownFadeOut(arrPointSprite[curDispPoint]));
        }
    }

    private void CreateBar(int curPoint)
    {
        arrPointSprite = new GameObject[curPoint];

        for (int idx = 0; idx < curPoint; idx++)
            arrPointSprite[idx] = CreateOnePointBar(idx);

        curDispPoint = curPoint - 1;
    }

    void Update()
    {
        if (isShow)
        {
            accumeTime += Time.deltaTime;

            if (accumeTime > interval)
                Hide();
        }
    }

    void Show()
    {
        gameObject.SetActive(true);
        isShow = true;
        accumeTime = 0;
    }

    void Hide()
    {
        gameObject.SetActive(false);
        isShow = false;
    }

    IEnumerator DownFadeOut(GameObject obj)
    {
        float effectTime = 0;
        float distance = 0.1f;
        float duration = 0.3f;
        float convertValue = 0;

        Transform tTransform = obj.transform;
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = ImgResourcesContainer.Inst.reduceBar;
        Color targetColor = Color.red;

        while (effectTime < duration)
        {
            effectTime += Time.deltaTime;

            convertValue = BK_Function.ConvertRangePercent(0, duration, effectTime);

            tTransform.Translate((1 - convertValue) * distance, (1 - convertValue) * -distance, 0);
            targetColor.a = 1 - convertValue;
            renderer.color = targetColor;

            yield return true;
        }
    }
}
