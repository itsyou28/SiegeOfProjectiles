using UnityEngine;
using System.Collections;

public class InputObstacleMode : MonoBehaviour, iInput
{
    public GameObjectPool obstaclePool;
    public bool isPress { get; set; }

    Vector3 targetPos;

    float accumeTime = 0;

    Skill obstacle;

    void Start()
    {
        obstacle = SkillMng.Inst.GetObstacleSkillData();
    }

    public void OnDown(Vector3 hitPos)
    {
        targetPos = hitPos;
    }

    public void OnDrag(Vector3 hitPos)
    {
        targetPos = hitPos;
    }

    public void OnPressUpdate()
    {
        accumeTime += Time.deltaTime;

        if (accumeTime > 0.2f)
        {
            GameObject obj = obstaclePool.Pop();
            obj.transform.position = targetPos;
            obj.gameObject.SetActive(true);

            accumeTime -= 0.2f;

            obstacle.Use();
        }
    }

    public void OnUp(Vector3 hitPos)
    {
    }
}