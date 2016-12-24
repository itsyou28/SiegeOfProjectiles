using UnityEngine;
using System.Collections;

public class Prjt_DeerStar : Projectile
{
    [SerializeField]
    Animator _ani;
    [SerializeField]
    ParticleSystem _trailParticle;

    const float speedMin = 0.3f;
    const float speedMax = 1.5f;
    const float speedSum = speedMin + speedMax;
    const float max = 220;

    float moveDistance = 0;

    Vector3 fromPos, toPos;
    float height = 50;
    float speed = 1; //0에 가까울수록 느려진다. 

    Vector3 centerPos, targetPos;

    CBezierSpline sline;

    float accumeTime = 0;
    float reviseTime = 0;

    bool isCollide = false;

    Collider _col;

    void Awake()
    {
        _col = GetComponent<Collider>();
    }

    void Update()
    {
        accumeTime += Time.deltaTime;

        reviseTime = accumeTime * speed;

        if (callback != null && reviseTime >= 0.3f)
        {
            callback(transform.position);
            callback = null;
            DestroySelf();
        }

        if (reviseTime >= 3.0f)
            DestroySelf();

        if (reviseTime <= 1.0f && !isCollide)
        {
            targetPos = Vector3.Lerp(fromPos, toPos, reviseTime);
            targetPos.y = sline.GetB_Spline(reviseTime);

            transform.position = targetPos;

            targetPos = Vector3.Lerp(fromPos, toPos, reviseTime + 0.01f);
            targetPos.y = sline.GetB_Spline(reviseTime + 0.01f);

            transform.LookAt(targetPos);
        }
        else if (!isCollide)
        {
            if (_ani.GetCurrentAnimatorStateInfo(0).IsName("Projectile_Idle"))
            {
                _ani.Play("Hit_Ground");
                _trailParticle.Stop();
            }
            _col.enabled = false;
        }
        else
            _col.enabled = false;
    }

    void DestroySelf()
    {
        gameObject.SetActive(false);
        DeerStarPool.Inst.Push(this);
    }

    callbackDispersion callback = null;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("EnemyShield"))
        {
            _ani.Play("Projectile_On_Shield");
            isCollide = true;
            transform.SetParent(col.transform);
            _col.enabled = false;
            _trailParticle.Stop();
        }

        if (col.CompareTag("EnemyCore"))
        {
            _ani.Play("Hit_Enemy");
            isCollide = true;
            _col.enabled = false;
            _trailParticle.Stop();
        }
    }

    public override void Fire(Vector3 from, Vector3 to, float aimHeight, callbackDispersion _callback = null)
    {
        isCollide = false;
        fromPos = from;
        toPos = to;
        height = aimHeight;

        callback = _callback;

        accumeTime = 0;

        centerPos = Vector3.Lerp(fromPos, toPos, 0.5f);
        centerPos.y = height;
        moveDistance = Vector3.Distance(fromPos, centerPos) +
            Vector3.Distance(centerPos, toPos);

        moveDistance = Mathf.Clamp(moveDistance, 0, max);

        speed = speedSum - BK_Function.ConvertRange(0, max, speedMin, speedMax, moveDistance);

        if (_callback == null)
            speed *= 0.85f;

        sline = new CBezierSpline(fromPos.y, height, height * 1.2f, toPos.y);
        sline.SetCP2(height);
        sline.SetCP3(height * 1.2f);


        transform.position = fromPos;
        targetPos = Vector3.Lerp(fromPos, toPos, 0.01f);
        targetPos.y = sline.GetB_Spline(0.01f);
        transform.LookAt(targetPos);

        gameObject.SetActive(true);

        _col.enabled = true;

        _ani.Play("Projectile_Idle");
        _trailParticle.Play();
    }
}
