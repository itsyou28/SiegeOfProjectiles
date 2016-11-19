using UnityEngine;
using System.Collections;

public delegate void deleFunc();

public class Tower : MonoBehaviour
{
    //타워 HP 관리
    //타워 애니메이션 관리
    //접촉 적 공격 처리

    public Transform[] frontPos;
    public Animator _ani;


    int HP = 30;
    bool isDestroyed = false;

    public event deleFunc eventDestroyTower;

    void Awake()
    {
        GlobalTowerInfo.AddTower(this);
    }

    void DestroySelf()
    {
        Debug.Log("Destroy " + gameObject.name);
        isDestroyed = true;

        gameObject.SetActive(false);

        GlobalTowerInfo.RemoveTower(this);
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
