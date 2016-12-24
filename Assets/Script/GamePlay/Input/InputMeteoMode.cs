using UnityEngine;
using System.Collections;

public class InputMeteoMode : MonoBehaviour, iInput
{
    public bool isPress { get; set; }

    public void OnDown(Vector3 hitPos)
    {

        Projectile bullet = MeteoPool.Inst.Pop();
        bullet.Fire(hitPos);
    }

    public void OnDrag(Vector3 hitPos)
    {
    }

    public void OnPressUpdate()
    {
    }

    public void OnUp(Vector3 hitPos)
    {
    }
}