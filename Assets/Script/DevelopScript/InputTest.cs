using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour
{
    public Transform targetPos;

    Projectile projectile;
    float aimHeight;

    Ray _ray;
    RaycastHit _hit;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            aimHeight = 0;
        }

        if (Input.GetMouseButton(0))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(_ray, out _hit))
            {
                targetPos.position = _hit.point;
                aimHeight += Time.deltaTime * 70;
                aimHeight = Mathf.Clamp(aimHeight, 0, 58);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            FireManager.Inst.Fire(targetPos.position, aimHeight);
        }
    }
}
