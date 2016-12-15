using UnityEngine;
using System.Collections;

public class OnCore : OnEnemyCollider
{
    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        
        if (col.CompareTag("Obstacle"))
        {
            OnObstacle();
        }
    }

    protected override void OnDeerStar()
    {
        iControl.OnDamage();
    }
    
    protected override void OnTowerAttack()
    {
        iControl.OnDamage();
        iControl.OnKnuckback(8);
    }

    bool isObstacle = false;
    float lastStayTime;
    void OnObstacle()
    {
        iControl.OnEnterObstacle();
        isObstacle = true;
    }

    void Update()
    {
        if(isObstacle)
        {
            if(Time.realtimeSinceStartup - lastStayTime > 0.1f)
            {
                isObstacle = false;
                iControl.OnExitObstacle();
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Obstacle"))
        {
            iControl.OnEnterObstacle();

            lastStayTime = Time.realtimeSinceStartup;

            if(!isObstacle)
                isObstacle = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.CompareTag("Obstacle"))
        {
            iControl.OnExitObstacle();
            isObstacle = false;
        }
    }
}
