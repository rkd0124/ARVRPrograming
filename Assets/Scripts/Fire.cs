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
    public Transform crosshair; //크로스헤어

    [Header("Damage")]
    public int baseDamage = 1; //기본 데미지

    [Header("Attack Range")]
    public float range = 2f; // 공격 반경           
    [Range(0, 180)] public float angle = 120f; // 부채꼴 각도 (슬라이더 추가)
    public LayerMask enemyMask; // 적 Layer

    public bool isHolding = false; // 버튼 누름 여부
    private float tickTimer = 0f; // Tick 누적 시간
    public bool followCameraLook = true; // 체크하면 카메라가 보는 곳으로 총구가 향함

    [Header("Effect")]
    public ParticleSystem fireEffect; //이펙트
    public Transform fireOrigin; // 불이 실제로 나가는 위치 (총구/손끝)

    [Header("Audio")]
    public AudioSource audioSource; // AudioSource 컴포넌트 연결
    public AudioClip fireClip; // 발사 중 재생할 오디오 클립
    

    void Start()
    {
        gauge = maxGauge;
        // 시작할 때 이펙트가 켜져있으면 끈다
        if (fireEffect != null)
        {
            fireEffect.Stop();
        }

        // fireOrigin 안전장치 - 오리진 없으면 그냥 플레이어 위치로 넣은거
        if (fireOrigin == null) fireOrigin = transform;

        if (audioSource != null && fireClip != null)
        {
            audioSource.clip = fireClip;
            audioSource.loop = true; 
            audioSource.playOnAwake = false; 
        }
    }

    void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);
        // 파티클 모양 실시간 동기화
        // 게임 도중 인스펙터에서 range나 angle을 바꾸면 이펙트도 같이 변함
        if (followCameraLook)
        {
            UpdateAimDirection();
        }

        UpdateParticleSettings();

        // 1. 입력 감지 -> R에서 마우스 홀드로 바꾼거긴한데 테스트 용임
        //isHolding = Input.GetMouseButton(1);
        isHolding = ARAVRInput.Get(ARAVRInput.Button.IndexTrigger); //VR


        // 2. 이펙트 처리 함ㅅ
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

    void UpdateAimDirection() //카메라 방향 동기화
    {
        if (Camera.main != null)
        {
            // 카메라의 회전값과 똑같이 맞춤 (위/아래/좌/우 모두 동기화)
            fireOrigin.rotation = Camera.main.transform.rotation;
        }
    }

    //파티클 설정을 스크립트 변수와 강제 동기화
    void UpdateParticleSettings()
    {
        if (fireEffect == null) return;

        var main = fireEffect.main;
        var shape = fireEffect.shape;

        // 파티클 모양을 Cone(원뿔)으로 강제 설정
        if (shape.enabled == false) shape.enabled = true;
        if (shape.shapeType != ParticleSystemShapeType.Cone) shape.shapeType = ParticleSystemShapeType.Cone;

        // 1. 사거리 동기화 (속도를 고정하고 수명을 조절)
        // 불꽃이 날아가는 속도
        float particleSpeed = 8f; 
        main.startSpeed = particleSpeed;
        // 거리 = 속도 * 시간 이므로, 시간(수명) = 거리 / 속도
        main.startLifetime = range / particleSpeed;

        // 2. 각도 동기화
        shape.angle = angle / 2f;
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
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // 버튼을 뗐거나 + 게이지가 없으면 불이 꺼짐
            if (fireEffect != null && fireEffect.isPlaying)
            {
                fireEffect.Stop();
            }
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // 범위 내 적들에게 부채꼴 판정 후 데미지 적용
    void ApplyRadialDamage()
    {
        Collider[] hits = Physics.OverlapSphere(fireOrigin.position, range, enemyMask);

        foreach (var hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - fireOrigin.position).normalized;

            float dot = Vector3.Dot(fireOrigin.forward, dirToTarget);
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

    private void OnDrawGizmosSelected() //범위 확인용이라 지워도 ㅇㅋ
    {
        // 기준점이 없으면 자기 자신을 기준으로 그리기
        Transform drawOrigin = fireOrigin != null ? fireOrigin : transform;

        // 1. 전체 사거리 원 그리기 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(drawOrigin.position, range);

        // 2. 부채꼴 각도 표시 선 그리기 (노란색)
        Vector3 leftDir = Quaternion.Euler(0, -angle / 2, 0) * drawOrigin.forward;
        Vector3 rightDir = Quaternion.Euler(0, angle / 2, 0) * drawOrigin.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(drawOrigin.position, leftDir * range);
        Gizmos.DrawRay(drawOrigin.position, rightDir * range);
    }
}