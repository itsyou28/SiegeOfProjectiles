﻿using UnityEngine;
using System.Collections;

public class FireProjectiles : MonoBehaviour
{
    const float speedMin = 0.3f;
    const float speedMax = 3;
    const float speedSum = speedMin + speedMax;
    const float max = 220;

    float moveDistance = 0;

    public Vector3 fromPos, toPos;
    Vector3 centerPos, targetPos;

    
    public Transform center;

    CBezierSpline sline;

    float accumeTime = 0;
    public float height = 50;
    public float speed = 1; //0~1 0에 가까울수록 느려진다. 

    float targetTime = 0;

    void Update()
    {
        accumeTime += Time.deltaTime;

        targetTime = accumeTime * speed;
        targetPos = Vector3.Lerp(fromPos, toPos, targetTime);
        targetPos.y = sline.GetB_Spline(targetTime);

        transform.position = targetPos;

        targetPos = Vector3.Lerp(fromPos, toPos, targetTime+0.01f);
        targetPos.y = sline.GetB_Spline(targetTime + 0.01f);

        transform.LookAt(targetPos);
    }

    public void Fire()
    {
        accumeTime = 0;

        centerPos = Vector3.Lerp(fromPos, toPos, 0.5f);
        centerPos.y = height;
        center.position = centerPos;
        moveDistance = Vector3.Distance(fromPos, centerPos) +
            Vector3.Distance(centerPos, toPos);
        //Debug.Log("distance : " + moveDistance + " // height : " + height);
        //Debug.Log("result (0~"+max+") : " + moveDistance);
        moveDistance = Mathf.Clamp(moveDistance, 0, max);
        
        speed = speedSum - BK_Function.ConvertRange(0, max, speedMin, speedMax, moveDistance);

        sline = new global::CBezierSpline(fromPos.y, height, height*1.2f, toPos.y);
        sline.SetCP2(height);
        sline.SetCP3(height * 1.2f);

        gameObject.SetActive(true);
    }
}
