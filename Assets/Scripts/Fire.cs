using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Gauge")]
    public float maxGauge = 100f; // 게이지 최대치
    public float gauge;  // 현재 게이지
    public float consumePerTick = 4f; //게이지 소모량 0.5초당     
    public float recoverPerTick = 2f; //게이지 회복량 0.5초당  
    public float tickInterval = 0.5f; //데미지랑 게이지 처리   

    [Header("Damage")]
    public int baseDamage = 1; //기본 데미지

    [Header("Attack Range")]
    public float range = 2f; // 공격 반경          
    public float angle = 100f; // 부채꼴 각도  
    public LayerMask enemyMask; // 적 Layer

    private bool isHolding = false; // 버튼 누름 여부
    private float tickTimer = 0f; // Tick 누적 시간

    [Header("Effect")]
    public ParticleSystem fireEffect;     

    void Start()
    {
        gauge = maxGauge;
        // 시작할 때 이펙트가 켜져있으면 끈다
        if (fireEffect != null) fireEffect.Stop(); 
    }

    void Update()
    {
        // 1. 입력 감지
        isHolding = Input.GetKey(KeyCode.R);

        // 2. 이펙트 처리 함수 호출 (이름 수정됨: FireEffect -> AttackEffect)
        AttackEffect(); 

        // 3. 게이지 및 데미지 처리
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;

            // 버튼 누름 AND 게이지 있음
            if (isHolding && gauge > 0f)
            {
                gauge -= consumePerTick;  
                
                // 게이지가 바닥나면 0으로 고정
                if (gauge <= 0f)
                {
                    gauge = 0f;
                    // 게이지가 다 떨어지면 이펙트도 즉시 끔
                    if (fireEffect != null) fireEffect.Stop(); 
                }
                else
                {
                    // 게이지가 남아있을 때만 공격
                    ApplyRadialDamage();
                }
            }
            else
            {
                // 버튼 안 누름 OR 게이지 없음 -> 회복
                // (버튼을 안 누르고 있을 때만 회복하도록 로직 수정)
                if (!isHolding && gauge < maxGauge)
                {
                    gauge += recoverPerTick;
                    if (gauge > maxGauge) gauge = maxGauge;
                }
            }
        }
    }

    // 이펙트 켜고 끄는 함수
    void AttackEffect()
    {
        // 버튼을 누르고 있고 + 게이지가 남아있어야 불이 나감
        if (isHolding && gauge > 0)
        {
            if (fireEffect != null && !fireEffect.isPlaying)
            {
                fireEffect.Play();
            }
        }
        else
        {
            // 버튼을 뗐거나 + 게이지가 없으면 불이 꺼짐
            if (fireEffect != null && fireEffect.isPlaying)
            {
                fireEffect.Stop();
            }
        }
    }

    // 범위 내 적들에게 부채꼴 판정 후 데미지 적용
    void ApplyRadialDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyMask);

        foreach (var hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward, dirToTarget);
            float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (theta <= angle / 2f)
            {
                var enemyTk = hit.GetComponent<Enemy_Tk>();
                var enemyNK = hit.GetComponent<Enemy_NK>();
                var enemyFly = hit.GetComponent<Enemy_fly>(); 

                if (enemyTk != null) enemyTk.TakeDamage(baseDamage);
                if (enemyNK != null) enemyNK.TakeDamage(baseDamage);
                if (enemyFly != null) enemyFly.TakeDamage(baseDamage);
            }
        }
    }
}