using UnityEngine;
using System.Collections;

public class OnShield : OnEnemyCollider
{
    public int shieldIdx = -1;

    [SerializeField]
    int shieldHP;
    
    public void Damage(int power)
    {
        ReduceShieldHP(power);
    }

    private void ReduceShieldHP(int reduceValue)
    {
        shieldHP -= reduceValue;

        if (shieldHP <= 0)
        {
            iControl.OnDestroyedShield(shieldIdx);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    protected override void OnDeerStar()
    {
        iControl.OnKnuckback(10);
        ReduceShieldHP(1);
    }

    protected override void OnMeteo()
    {
        iControl.OnMeteo();
        ReduceShieldHP(3);
    }
}
