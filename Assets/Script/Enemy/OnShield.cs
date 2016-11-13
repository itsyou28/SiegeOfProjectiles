using UnityEngine;
using System.Collections;

public class OnShield : MonoBehaviour
{
    public Enemy iControl;

    void OnTriggerEnter()
    {
        iControl.OnShield();
    }
}
