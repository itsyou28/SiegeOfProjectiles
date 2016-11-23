using UnityEngine;
using System.Collections;

public delegate void deleFunc();

public class GlobalTowerInfo : GlobalList<Tower>
{
}

public class Tower : MonoBehaviour
{
    //타워 HP 관리
    //타워 애니메이션 관리
    //접촉 적 공격 처리

    public Transform[] frontPos;
    public Animator _ani;

    public Collider attackCollider;


    int HP = 30;
    bool isDestroyed = false;

    public event deleFunc eventDestroyTower;

    Vector3 attackColWaitPos;
    Vector3 attackColAttackPos;

    void Awake()
    {
        attackColWaitPos = attackCollider.transform.position;
        attackColAttackPos = attackColWaitPos;
        attackColAttackPos.x += 20;
        GlobalTowerInfo.Add(this);
    }

    float accumeTime = 0;

    void Update()
    {
        accumeTime += Time.deltaTime;

        if(accumeTime>=2 && accumeTime < 3)
        {
            attackCollider.transform.position = attackColAttackPos;
            accumeTime = 3;
        }
        if(accumeTime >= 3.3f)
        {
            attackCollider.transform.position = attackColWaitPos;
            accumeTime = 0;
        }
    }
    
    void DestroySelf()
    {
        Debug.Log("Destroy " + gameObject.name);
        isDestroyed = true;

        gameObject.SetActive(false);

        GlobalTowerInfo.Remove(this);
        eventDestroyTower();
    }

    public void Hit()
    {
        //HP가 깍이고 흔들린다
        _ani.Play("Damage");
        HP -= 1;

        if (HP <= 0 && !isDestroyed)
            DestroySelf();
    }

    public Vector3 GetRandomFrontPos()
    {
        int randIdx = Random.Range(0, frontPos.Length);

        return frontPos[randIdx].position;
    }
}
