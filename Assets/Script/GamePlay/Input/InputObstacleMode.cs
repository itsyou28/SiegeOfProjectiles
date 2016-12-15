using UnityEngine;
using System.Collections;

public class InputObstacleMode : MonoBehaviour, iInput
{
    public GameObjectPool obstaclePool;
    Ray _ray;
    RaycastHit _hit;

    public void OnDown()
    {
    }

    float accumeTime = 0;
    public void OnPress()
    {
        accumeTime += Time.deltaTime;

        if (accumeTime > 0.2f)
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {
                GameObject obj = obstaclePool.Pop();
                obj.transform.position = _hit.point;
                obj.gameObject.SetActive(true);
            }
            accumeTime -= 0.2f;
        }
    }

    public void OnUp()
    {
    }
}