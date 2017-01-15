using UnityEngine;
using System.Collections;

public class E_Disp_Status : MonoBehaviour
{
    public Sprite hpImg;
    public GameObject hpRoot;

    GameObject[] arrHP = null;

    void Awake()
    {
    }
    
    GameObject Create(int idx)
    {
        GameObject obj = new GameObject("hpSprite");
        obj.transform.parent = hpRoot.transform;
        obj.transform.localScale = new Vector3(8, 15);
        obj.transform.localPosition = new Vector3(4 * idx, 0);
        SpriteRenderer render = obj.AddComponent<SpriteRenderer>();
        render.sprite = hpImg;

        return obj;
    }

    int curDispHP = 0;
    int _curHP = 0;
    float accumeTime = 0;

    public void DispHP(int curHP, int damage)
    {
        if (arrHP == null)
        {
            arrHP = new GameObject[curHP];

            for (int idx = 0; idx < curHP; idx++)
                arrHP[idx] = Create(idx);

            curDispHP = curHP - 1;
        }

        _curHP = curHP - damage;

        for (; curDispHP > _curHP - 1; curDispHP--)
        {
            if (curDispHP < 0)
                break;

            StartCoroutine(DownFadeOut(arrHP[curDispHP]));
        }

        Show();
    }

    IEnumerator DownFadeOut(GameObject obj)
    {
        float effectTime = 0;
        float distance = 0.05f;
        float duration = 0.3f;
        float convertValue = 0;
        
        Transform tTransform = obj.transform;
        Material mat = obj.GetComponent<SpriteRenderer>().material;

        while ( effectTime < duration)
        {
            effectTime += Time.deltaTime;

            convertValue = BK_Function.ConvertRangePercent(0, duration, effectTime);

            tTransform.Translate((1- convertValue) *distance, (1 - convertValue) *-distance, 0);
            mat.color = new Color(1, 1, 1, 1 - convertValue);

            yield return true;
        }
    }

    bool isShow = false;

    const float interval = 1.0f;

    void Update()
    {
        if (isShow)
        {
            accumeTime += Time.deltaTime;

            if(accumeTime > interval)
                Hide();

        }
    }

    public void ReduceShield(int idx, int curHP, int damage)
    {
        Show();
    }

    void Show()
    {
        hpRoot.SetActive(true);
        isShow = true;
        accumeTime = 0;
    }

    void Hide()
    {
        hpRoot.SetActive(false);
        isShow = false;
    }
    
}
