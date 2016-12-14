using UnityEngine;
using System.Collections;

public class InputNormalMode : MonoBehaviour, iInput
{
    public Transform player;
    public LineRenderer _line;
    
    public GameObjectPool deerstarCursorPool;
    Transform curCursor;
    Transform curCursorArea;

    Projectile projectile;
    float aimHeight;

    Ray _ray;
    RaycastHit _hit;

    const int max = 10;

    void Awake()
    {
        _line.SetVertexCount(max);
    }

    public void OnDown()
    {
        aimHeight = 0;

        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            GameObject obj = deerstarCursorPool.Pop();
            curCursor = obj.transform;
            curCursorArea = curCursor.GetChild(1);
            curCursor.position = _hit.point;
            curCursor.gameObject.SetActive(true);
        }
    }

    public void OnPress()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            curCursor.position = _hit.point;
            aimHeight += Time.deltaTime * 70;
            aimHeight = Mathf.Clamp(aimHeight, 0, 58);

            curCursorArea.localScale = new Vector3(
                aimHeight * 0.2f * 2, aimHeight * 0.2f * 2, 1);

            DrawLine();
        }
    }

    public void OnUp()
    {
        FireManager.Inst.Fire(curCursor.position, aimHeight);
    }

    Vector3[] linePoints = new Vector3[max];

    void DrawLine()
    {
        CBezierSpline sline = new CBezierSpline(player.position.y, aimHeight, aimHeight * 1.2f, curCursor.position.y);

        for (int idx = 0; idx < max; idx++)
        {
            linePoints[idx] = Vector3.Lerp(player.position, curCursor.position, 0.03f * idx);
            linePoints[idx].y = sline.GetB_Spline(0.03f * idx);
        }

        _line.SetPositions(linePoints);
    }
}
