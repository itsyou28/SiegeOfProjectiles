using UnityEngine;
using System.Collections;

public class FireProjectiles : MonoBehaviour {

    public Transform from;
    public Transform to;

    CBezierSpline sline;

    float accumeTime = 0;
    public float height = 50;
    public float speed = 1; //0~1 높이가 높을 수록 0에 가까워진다. 

    void Awake()
    {
        sline = new global::CBezierSpline(from.position.y, height, height*1.2f, to.position.y);
    }

    void Update()
    {
        accumeTime += Time.deltaTime;

        Vector3 targetPos = Vector3.Lerp(from.position, to.position, accumeTime * speed);
        targetPos.y = sline.GetB_Spline(accumeTime * speed);

        transform.position = targetPos;
    }    

    public void Fire()
    {
        accumeTime = 0;

        speed = 1- BK_Function.ConvertRangePercent(0, 100, height);

        sline.SetCP2(height);
        sline.SetCP3(height * 1.2f);
    }
}
