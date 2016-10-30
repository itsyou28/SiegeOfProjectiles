using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour
{
    public Transform targetPos;
    public FireProjectiles projectile;

    Ray _ray;
    RaycastHit _hit;
    
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            projectile.height = 0;
        }

        if (Input.GetMouseButton(0))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(_ray, out _hit))
            {
                targetPos.position = _hit.point;
                projectile.height += Time.deltaTime * 100;
            }
        }

        if (Input.GetMouseButtonUp(0))
            projectile.Fire();
    }
}
