using UnityEngine;
using System.Collections;

public delegate void callbackDispersion(Vector3 pos);

public class Projectile : MonoBehaviour
{
    public virtual void Fire(
        Vector3 from, Vector3 to, float aimHeight, callbackDispersion _callback = null)
    {
    }
}
