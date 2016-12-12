using UnityEngine;
using System.Collections;

public class InputMeteoMode : MonoBehaviour, iInput
{
    Ray _ray;
    RaycastHit _hit;

    public void OnDown()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            Projectile bullet = MeteoPool.Inst.Pop();
            bullet.Fire(_hit.point);
        }
    }

    public void OnPress()
    {
    }

    public void OnUp()
    {
    }
}