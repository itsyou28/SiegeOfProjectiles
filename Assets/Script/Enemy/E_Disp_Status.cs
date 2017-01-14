using UnityEngine;
using System.Collections;

public class E_Disp_Status : MonoBehaviour
{
    public Sprite hpImg;
    public GameObject hpRoot;
    //감소 이벤트가 발생하면
    //바를 화면에 띄운다. 
    //한 칸씩 감소시킨다
    //감소 애니메이션이 끝나면 바를 감춘다. 

    //애니메이터를 사용해야 하나?
    //어떤 연출을 보여줄거지?
    //깜빡이며 사라진다. 
    //이동하면서 페이드아웃
    //페이드아웃
    //심플컷

    //캔버스를 사용?
    //스프라이트 사용?


    //사용자와 상호작용하지 않는데 캔버스를 사용할 필요는 없다. 

    //스프라이트 심플 컷으로 구현하자


    //10칸 빈 이미지
    //10칸 찬 이미지
    //sprite에 Tile이 되나?
    //컷아웃 연구가 우선되야 하나?


    // 몇 칸에서 몇 칸으로 줄어드는 걸 보여준다. 
    // 시작HP, 목표HP를 받아서 보여준다. 

    //마테리얼로 설정하면 Material을 적마다 개별 생성해야 한다. 
    //그게 괜찮은 방법인가?
    //스프라이트를 여러개 붙인다?

    GameObject[] arrHP = null;

    void Awake()
    {
    }

    //스프라이트 이미지 Arr이를 생성하고 자동 횡 배치
    //시간의 흐름에 따라 감소->HIde 진행
    //진행 중에 추가 데미지가 올 경우 감소부분만 증가하도록 조정;

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

            arrHP[curDispHP].GetComponent<SpriteRenderer>().enabled = false;
        }

        Show();
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
