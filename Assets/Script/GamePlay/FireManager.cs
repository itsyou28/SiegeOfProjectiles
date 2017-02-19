using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireManager : MonoBehaviour
{
    private static FireManager instance = null;
    public static FireManager Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<FireManager>();

            return instance;
        }
    }

    public Transform player;
    Vector3 targetPos;
    float targetHeight;

    Queue<Vector3> fireQueue = new Queue<Vector3>();
    Queue<float> aimHeightQueue = new Queue<float>();

    float accumeTime = 0;
    const float fireRate = 0.5f;
    void Update()
    {
        if (fireQueue.Count > 0)
        {
            accumeTime += Time.deltaTime;

            if (accumeTime > fireRate)
            {
                Fire(fireQueue.Dequeue(), aimHeightQueue.Dequeue());
                accumeTime -= fireRate;
            }

            if (fireQueue.Count == 0)
                accumeTime = 0;
        }
    }

    public void Fire(Transform obj, float aimHeight)
    {
        fireQueue.Enqueue(obj.position);
        aimHeightQueue.Enqueue(aimHeight);
        float fireRemainTime = accumeTime + ((fireQueue.Count - 1) * fireRate);

        obj.gameObject.GetComponent<DeerStarTarget>().Fire(fireRemainTime, 
            Prjt_DeerStar.CalDurationOfFlight(player.position, obj.position, aimHeight)*1.35f);
    }

    void Fire(Vector3 to, float aimHeight)
    {
        targetPos = to;
        targetHeight = aimHeight;
        Projectile bullet = DeerStarPool.Inst.Pop();

        bullet.Fire(player.position, targetPos, aimHeight, DispersionStart);
    }

    void DispersionStart(Vector3 from)
    {
        Vector3 randPos = Vector3.zero;
        Projectile dispersionBullet;

        float range = targetHeight * 0.2f;
        Vector3 vRandPosInArea;

        for (int count = 0; count < Mathf.FloorToInt(targetHeight*0.4f); count++)
        {
            vRandPosInArea = Random.insideUnitCircle * range;
            randPos.x = targetPos.x + vRandPosInArea.x;
            randPos.z = targetPos.z + vRandPosInArea.y;
            
            dispersionBullet = DeerStarPool.Inst.Pop();
            dispersionBullet.Fire(from, randPos, targetHeight);
        }
    }
    
}
