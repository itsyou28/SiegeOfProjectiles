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
    const float max = 220;  //목표물과의 직선 거리 최대 한계


    Vector3 fromPos, toPos, targetPos;
    float height = 50;
    float speed = 1; //0에 가까울수록 느려진다. 
    
    CBezierSpline sline;

    float accumeTime = 0;
    float reviseTime = 0; //0~1 0:spline 시작점. 1:spline 종료점

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
        
        //분산전 발사체일 경우 분사시간이 됐을 때 분산 명령을 내리고 삭제한다. 
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
            //해당 시간대의 곡선위의 위치 계산
            targetPos = Vector3.Lerp(fromPos, toPos, reviseTime);
            targetPos.y = sline.GetB_Spline(reviseTime);
            transform.position = targetPos;

            //다음 프레임의 곡선위의 위치를 계산에서 탄환 방향 조정
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

    public override void Fire(Vector3 from, Vector3 to, float aimHeight, callbackDispersion callback = null)
    {
        isCollide = false;
        fromPos = from;
        toPos = to;
        height = aimHeight;
        this.callback = callback;
        accumeTime = 0;

        speed = ReviseSpeed(from, to, aimHeight);

        //분산 후 발사체의 속도 조정
        if (callback == null)
            speed *= Random.Range(0.85f, 1.15f);

        //spline 설정
        sline = new CBezierSpline(fromPos.y, height, height * 1.2f, toPos.y);
        sline.SetCP2(height);
        sline.SetCP3(height * 1.2f);

        //발사체 초기위치 및 방향 조정
        transform.position = fromPos;
        targetPos = Vector3.Lerp(fromPos, toPos, 0.01f);
        targetPos.y = sline.GetB_Spline(0.01f);
        transform.LookAt(targetPos);

        gameObject.SetActive(true);
        _col.enabled = true;

        _ani.Play("Projectile_Idle");
        _trailParticle.Play();
    }

    static float moveDistance = 0;

    /// <summary>
    /// 거리에 비례한 속도 계산
    /// </summary>
    static float ReviseSpeed(Vector3 from, Vector3 to, float height)
    {
        //거리계산을 위한 중간 위치 계산
        Vector3 centerPos = Vector3.Lerp(from, to, 0.5f);
        centerPos.y = height;

        //곡선 길이 계산(두변으로 나눠서 단순계산)
        moveDistance = Vector3.Distance(from, centerPos) + Vector3.Distance(centerPos, to);
        moveDistance = Mathf.Clamp(moveDistance, 0, max);

        return speedSum - BK_Function.ConvertRange(0, max, speedMin, speedMax, moveDistance);
    }
    
    public static float CalDurationOfFlight(Vector3 from, Vector3 to, float height)
    {
        return 1 / ReviseSpeed(from, to, height);
    }
}
