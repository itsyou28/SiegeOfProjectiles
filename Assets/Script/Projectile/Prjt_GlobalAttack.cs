using UnityEngine;
using System.Collections;

public class Prjt_GlobalAttack : Projectile
{
    [SerializeField]
    Animator _ani;

    float accumeTime = 0;

    void Update()
    {
        accumeTime += Time.deltaTime;

        if (accumeTime > 1)
        {
            gameObject.SetActive(false);
            GlobalAttackPool.Inst.Push(this);
        }
    }

    public override void Fire(Vector3 targetPos)
    {
        accumeTime = 0;
        transform.position = targetPos;
        gameObject.SetActive(true);
        _ani.Play("GlobalAttack");
    }
}
