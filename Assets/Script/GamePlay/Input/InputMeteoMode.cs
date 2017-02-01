using UnityEngine;
using System.Collections;

public class InputMeteoMode : MonoBehaviour, iInput
{
    public bool isPress { get; set; }

    Skill meteo;

    void Start()
    {
        meteo = SkillMng.Inst.GetMeteoSkillData();
    }

    public void OnDown(Vector3 hitPos)
    {
        Projectile bullet = MeteoPool.Inst.Pop();
        bullet.Fire(hitPos);
        meteo.Use();
    }

    public void OnDrag(Vector3 hitPos)
    {
    }

    public void OnPressUpdate()
    {
    }

    public void OnUp(Vector3 hitPos)
    {
    }
}