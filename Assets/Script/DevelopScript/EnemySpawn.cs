using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public GameObject E_Assault_Origin;
    public GameObject E_Shooter_Origin;
    public GameObject E_Defender_Origin;

    float accumeTime = 0;

    Vector3 spawnPos = new Vector3(70, 0.11f, 0);
    float nextSpawnTime = 0.5f;

    GameObject origin;

    void Update()
    {
        accumeTime += Time.deltaTime;

        if(accumeTime > nextSpawnTime)
        {
            int max = Random.Range(0, 2);
            for(int idx=0; idx< max; idx++)
            {
                int type = Random.Range(0, 3);

                switch(type)
                {
                    case 0:
                        origin = E_Assault_Origin;
                        break;
                    case 1:
                        origin = E_Shooter_Origin;
                        break;
                    case 2:
                        origin = E_Defender_Origin;
                        break;
                }

                GameObject obj = Instantiate(origin);

                spawnPos.z = Random.Range(-50, 25);

                obj.transform.position = spawnPos;
            }

            accumeTime = 0;
            nextSpawnTime = Random.Range(0.5f, 1.8f);
        }
    }
}
