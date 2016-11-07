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
        Projectile bullet = ProjectilePool.Inst.Pop();

        bullet.Fire(player.position, targetPos, aimHeight, DispersionStart);
    }

    void DispersionStart(Vector3 from)
    {
        Vector3 randPos = Vector3.zero;
        Projectile dispersionBullet;

        float range = 10;

        for (int count = 0; count < Mathf.FloorToInt(targetHeight*0.25f); count++)
        {
            randPos.x = Random.Range(targetPos.x - range, targetPos.x + range);
            randPos.z = Random.Range(targetPos.z - range, targetPos.z + range);

            dispersionBullet = ProjectilePool.Inst.Pop();
            dispersionBullet.Fire(from, randPos, targetHeight);
        }
    }
    
}
