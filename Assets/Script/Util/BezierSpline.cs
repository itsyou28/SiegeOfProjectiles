using UnityEngine;
using System;
using System.Collections;

public static class BezierSpline
{
    ///<summary>
    ///BEZIER SPLINE 
    ///출처: GPG 사이트 "베지어 곡선, spline에 대해서 질문 합니다." 스레드의 chadr 님이 올려주신 소스입니다. 
    ///http://www.gpgstudy.com/forum/viewtopic.php?t=7166&sid=ca00a6ecfd77f415f15cdcf48478fb23
    /// </summary>
    /// <param name="u">u == (1-t), u_2 == (1-t)^2, u_3 == (1-t)^3 (t=시간의 흐름 (t < 1 ) ) </param>
    /// <param name="cntrl0">시작점</param>
    /// <param name="cntrl1">중간점1. 이 점은 반드시 지나지 않는다. 근처를 지나는 곡선이 생긴다. </param>
    /// <param name="cntrl2">중간점2. 이 점은 반드시 지나지 않는다. 근처를 지나는 곡선이 생긴다. </param>
    /// <param name="cntrl4">최종점. u가 1이었을 때 이 값이 반환된다. </param>
    public static float BEZIER_SPLINE(
        float u, float u_2, float u_3, float cntrl0, float cntrl1, float cntrl2, float cntrl3)
    {
        return ((-1.0f * u_3 + 3.0f * u_2 - 3.0f * u + 1.0f) * cntrl0 +
          (3.0f * u_3 - 6.0f * u_2 + 3.0f * u + 0.0f) * cntrl1 +
          (-3.0f * u_3 + 3.0f * u_2 + 0.0f * u + 0.0f) * cntrl2 +
          (1.0f * u_3 + 0.0f * u_2 + 0.0f * u + 0.0f) * cntrl3
          );
    }
}

public class CBezierSpline
{
    float[] m_fArrayControlPoint = null;

    float u, u2, u3;

    public CBezierSpline(float cp1, float cp2, float cp3, float cp4)
    {
        m_fArrayControlPoint = new float[4];

        m_fArrayControlPoint[0] = cp1;
        m_fArrayControlPoint[1] = cp2;
        m_fArrayControlPoint[2] = cp3;
        m_fArrayControlPoint[3] = cp4;
    }

    /// <summary>
    /// SetControlPoint 함수를 통해 사전에 CP값을 지정해 뒀을 때 
    /// fValue만으로 지정 곡선 그래프의 현재 값을 반환합니다. 
    /// </summary>
    /// <param name="fValue">0~1 사이 시간의 흐름 혹은 입력값의 범위를 0~1사이로 환산한 값</param>
    /// <returns></returns>
    public float GetB_Spline(float fValue)
    {
        if (m_fArrayControlPoint == null)
        {
            Exception e = new Exception("Call SetControlPoint Before this");
            throw (e);
        }

        u = fValue;
        u2 = u * u;
        u3 = u2 * u;

        return BezierSpline.BEZIER_SPLINE(u, u2, u3,
            m_fArrayControlPoint[0], m_fArrayControlPoint[1], m_fArrayControlPoint[2], m_fArrayControlPoint[3]);
    }
}