using UnityEngine;
using System.Collections;

public class TowerAttack : MonoBehaviour
{
    [SerializeField]
    Tower pTower;

    [SerializeField]
    GameObjectPool towerAttackHitPool;

    public void OnTriggerEnter(Collider col)
    {
        pTower.OnAttackEnemy();

        StartCoroutine(HitEffect(col.transform.position));
    }

    IEnumerator HitEffect(Vector3 targetPos)
    {
        GameObject effect = towerAttackHitPool.Pop();

        effect.transform.position = targetPos;
        effect.SetActive(true);
        effect.GetComponent<Animator>().Play("Tower_Attack_Hit2");

        yield return new WaitForSeconds(1.2f);

        effect.SetActive(false);
        towerAttackHitPool.Push(effect);
    }
}
