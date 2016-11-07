using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour
{
    public Transform targetPos;
    public LineRenderer _line;

    Projectile projectile;
    float aimHeight;

    Ray _ray;
    RaycastHit _hit;

    const int max = 10;

    void Awake()
    {
        _line.SetVertexCount(max);
    }
    
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

                DrawLine();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            FireManager.Inst.Fire(targetPos.position, aimHeight);
        }
    }

    Vector3 startPos = new Vector3(-72, 1, -1);
    Vector3[] linePoints = new Vector3[max];

    void DrawLine()
    {
        CBezierSpline sline = new CBezierSpline(1, aimHeight, aimHeight * 1.2f, 1);
        
        for(int idx=0; idx<max; idx++)
        {
            linePoints[idx] = Vector3.Lerp(startPos, targetPos.position, 0.03f * idx);
            linePoints[idx].y = sline.GetB_Spline(0.03f * idx);
        }

        _line.SetPositions(linePoints);
    }
}
