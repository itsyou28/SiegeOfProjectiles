using UnityEngine;
using System.Collections;

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

    public void Fire(Vector3 to, float aimHeight)
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
