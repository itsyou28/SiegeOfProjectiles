using UnityEngine;
using System.Collections;

public class FireProjectiles : MonoBehaviour {

    public Transform from;
    public Transform to;
    public Transform center;

    CBezierSpline sline;

    float accumeTime = 0;
    public float height = 50;
    public float speed = 1; //0~1 0에 가까울수록 느려진다. 

    void Awake()
    {
        sline = new global::CBezierSpline(from.position.y, height, height*1.2f, to.position.y);
    }

    void Update()
    {
        accumeTime += Time.deltaTime;

        targetPos = Vector3.Lerp(from.position, to.position, accumeTime * speed);
        targetPos.y = sline.GetB_Spline(accumeTime * speed);

        transform.position = targetPos;
    }

    const float speedMin = 0.3f;
    const float speedMax = 3;
    const float speedSum = speedMin + speedMax;
    const float max = 220;

    float moveDistance = 0;
    Vector3 centerPos, targetPos;

    public void Fire()
    {
        accumeTime = 0;

        centerPos = Vector3.Lerp(from.position, to.position, 0.5f);
        centerPos.y = height;
        center.position = centerPos;
        moveDistance = Vector3.Distance(from.position, centerPos) +
            Vector3.Distance(centerPos, to.position);
        //Debug.Log("distance : " + moveDistance + " // height : " + height);
        //Debug.Log("result (0~"+max+") : " + moveDistance);
        moveDistance = Mathf.Clamp(moveDistance, 0, max);
        
        speed = speedSum - BK_Function.ConvertRange(0, max, speedMin, speedMax, moveDistance);

        sline.SetCP2(height);
        sline.SetCP3(height * 1.2f);
    }
}
