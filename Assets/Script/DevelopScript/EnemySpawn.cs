using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public GameObject EnemyOrigin;

    float accumeTime = 0;

    Vector3 spawnPos = new Vector3(70, 0.11f, 0);
    float nextSpawnTime = 0.5f;
    void Update()
    {
        accumeTime += Time.deltaTime;

        if(accumeTime > nextSpawnTime)
        {
            int max = Random.Range(0, 2);
            for(int idx=0; idx< max; idx++)
            {
                GameObject obj = Instantiate(EnemyOrigin);

                spawnPos.z = Random.Range(-50, 25);

                obj.transform.position = spawnPos;
            }

            accumeTime = 0;
            nextSpawnTime = Random.Range(0.5f, 1.8f);
        }
    }
}
