using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputNormalMode : MonoBehaviour, iInput
{
    public Transform player;
    public LineRenderer _line;

    public GameObjectPool deerstarCursorPool;

    public bool isPress { get; set; }

    Transform curCursor;
    Transform curCursorArea;

    Projectile projectile;
    float aimHeight;

    const int max = 10;

    Queue<Vector3> fireQueue = new Queue<Vector3>();
    Queue<float> aimHeightQueue = new Queue<float>();

    void Awake()
    {
        _line.SetVertexCount(max);
    }

    public void OnDown(Vector3 hitPos)
    {
        aimHeight = 0;

        GameObject obj = deerstarCursorPool.Pop();
        curCursor = obj.transform;
        curCursorArea = curCursor.GetChild(1);
        curCursor.position = hitPos;
        curCursor.gameObject.SetActive(true);
    }

    public void OnDrag(Vector3 hitPos)
    {
        curCursor.position = hitPos;

        DrawLine();
    }

    public void OnPressUpdate()
    {
        aimHeight += Time.deltaTime * 70;
        aimHeight = Mathf.Clamp(aimHeight, 0, 58);

        curCursorArea.localScale = new Vector3(
            aimHeight * 0.2f * 2, aimHeight * 0.2f * 2, 1);
    }

    public void OnUp(Vector3 hitPos)
    {
        if (curCursor != null)
        {
            fireQueue.Enqueue(curCursor.position);
            aimHeightQueue.Enqueue(aimHeight);
        }

        curCursor = null;
    }

    float accumeTime = 0;
    const float fireRate = 0.5f;
    void Update()
    {
        if(fireQueue.Count > 0)
        {
            accumeTime += Time.deltaTime;

            if (accumeTime > fireRate)
            {
                FireManager.Inst.Fire(fireQueue.Dequeue(), aimHeightQueue.Dequeue());
                accumeTime -= fireRate;
            }

            if (fireQueue.Count == 0)
                accumeTime = 0;
        }
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
