using UnityEngine;
using System.Collections;

public class OnDamage : MonoBehaviour
{
    public Enemy iControl;

    void OnTriggerEnter()
    {
        iControl.OnDamage();
    }
}
