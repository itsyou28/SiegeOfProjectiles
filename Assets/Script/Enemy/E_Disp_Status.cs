using UnityEngine;
using System.Collections;

public class E_Disp_Status : MonoBehaviour
{
    [SerializeField]
    E_Point_Bar hpBar;
    [SerializeField]
    E_Point_Bar[] arrShieldBar;

    void Awake()
    {
#if UNITY_EDITOR
        if (hpBar == null)
            Debug.LogError(transform.parent.name + " not set hpBar");

        if (arrShieldBar.Length == 0)
            if(transform.parent.name != "E_Shooter(Clone)")
                Debug.LogWarning(transform.parent.name + " not set shieldBar"); 
#endif
    }

    public void DispHP(int curHP, int damage)
    {
        hpBar.RequestShow(curHP, damage);
    }

    public void ReduceShield(int idx, int curHP, int damage)
    {
        arrShieldBar[idx].RequestShow(curHP, damage);
    }
}
