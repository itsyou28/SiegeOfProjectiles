﻿using UnityEngine;
using System.Collections;

public delegate void callbackDispersion(Vector3 pos);

public class Projectile : MonoBehaviour
{
    const float speedMin = 0.3f;
    const float speedMax = 3;
    const float speedSum = speedMin + speedMax;
    const float max = 220;

    float moveDistance = 0;

    Vector3 fromPos, toPos;
    float height = 50;
    float speed = 1; //0에 가까울수록 느려진다. 

    Vector3 centerPos, targetPos;

    CBezierSpline sline;

    float accumeTime = 0;
    float reviseTime = 0;

    void Update()
    {
        accumeTime += Time.deltaTime;

        reviseTime = accumeTime * speed;

        if(callback != null && reviseTime >=0.3f)
        {
            callback(transform.position);
            callback = null;
        }

        if (reviseTime >= 1)
            DestroySelf();

        targetPos = Vector3.Lerp(fromPos, toPos, reviseTime);
        targetPos.y = sline.GetB_Spline(reviseTime);

        transform.position = targetPos;

        targetPos = Vector3.Lerp(fromPos, toPos, reviseTime + 0.01f);
        targetPos.y = sline.GetB_Spline(reviseTime + 0.01f);

        transform.LookAt(targetPos);
    }

    void DestroySelf()
    {
        gameObject.SetActive(false);
        ProjectilePool.Inst.Push(this);
    }

    callbackDispersion callback = null;

    public void Fire(Vector3 from, Vector3 to, float aimHeight, callbackDispersion _callback = null)
    {
        fromPos = from;
        toPos = to;
        height = aimHeight;

        callback = _callback;

        accumeTime = 0;

        centerPos = Vector3.Lerp(fromPos, toPos, 0.5f);
        centerPos.y = height;
        moveDistance = Vector3.Distance(fromPos, centerPos) +
            Vector3.Distance(centerPos, toPos);

        moveDistance = Mathf.Clamp(moveDistance, 0, max);

        speed = speedSum - BK_Function.ConvertRange(0, max, speedMin, speedMax, moveDistance);

        if (_callback == null)
            speed *= 0.85f;

        sline = new CBezierSpline(fromPos.y, height, height * 1.2f, toPos.y);
        sline.SetCP2(height);
        sline.SetCP3(height * 1.2f);

        transform.position = fromPos;
        targetPos = Vector3.Lerp(fromPos, toPos, reviseTime + 0.01f);
        targetPos.y = sline.GetB_Spline(reviseTime + 0.01f);
        Debug.Log(targetPos);
        transform.LookAt(targetPos);

        gameObject.SetActive(true);
    }
}
