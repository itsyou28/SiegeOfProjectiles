using UnityEngine;
using System.Collections;

public class DeerStarTarget : MonoBehaviour
{
    [SerializeField]
    GameObjectPool pool;
    [SerializeField]
    TextMesh text;

    float fireRemainTime;
    float arriveRemainTime;
    
    public void Fire(float fireRemainTime, float arriveRemainTime)
    {
        this.fireRemainTime = fireRemainTime;
        this.arriveRemainTime = arriveRemainTime;
        
        StartCoroutine(UpdateCursor());
    }

    IEnumerator UpdateCursor()
    {
        yield return true;

        float accumeTime = 0;
        text.gameObject.SetActive(true);

        while(accumeTime < fireRemainTime)
        {
            accumeTime += Time.deltaTime;

            if (accumeTime < fireRemainTime)
            {
                float temp = fireRemainTime - accumeTime;
                text.text = (Mathf.Floor((temp) * 10) * 0.1f).ToString();
            }

            yield return true;
        }

        //yield return new WaitForSeconds(arriveRemainTime - accumeTime);
        accumeTime = (fireRemainTime - accumeTime)*-1;

        while (accumeTime < arriveRemainTime)
        {
            accumeTime += Time.deltaTime;

            float temp = arriveRemainTime - accumeTime;
            text.text = (Mathf.Floor((temp) * 10) * 0.1f).ToString();

            yield return true;
        }

        OnEnd();        
    }

    void OnEnd()
    {
        pool.Push(gameObject);
        gameObject.SetActive(false);
    }
}

