using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //NavMeshAgent

public class Enemy_Tk : MonoBehaviour, IEnemy
{
    public int hp = 30; //체력
    //공격&이동 관련---
    public float moveSpeed = 2.0f; //이동속도
    public int attackDamage = 2; //데미지
    public float attackCoolTime = 2.0f;//공격쿨타임
    public float attackdistance = 3.92f; // 공격 사거리

    float originalSpeed; //얼음 아이템을 위한 이동속도 백업용
    float IceGauge = 5f; //얼음 게이지 채우기용

    private bool isAttacking = false; //쿨타임인지 아닌지
    //공격 이동---

    //NavMesh 관련임.. 설명하자면 장애물 있는 공간에서 자동으로 최적 경로 추적하는거
    NavMeshAgent agent;
    Transform towerTarget;

    //체력 감소 관련
    Coroutine poisonRoutine; 
    //^^^코루틴 : 시간 일시정지 같은 느낌인데...
    // 시간 제어하면서 실행을 단계에 맞춰서?? 처리하는 애 라고 생각하면 편함

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        originalSpeed = moveSpeed;   // 얼음 대비용

        GameObject towerObj = GameObject.FindGameObjectWithTag("Tower");
        // 타워찾아서
        if(towerObj!=null){
            towerTarget = towerObj.transform;
            // 목표지점을 타워가 있는 곳으로 설정
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (towerTarget != null && !isAttacking){
            agent.SetDestination(towerTarget.position); 
            //최단 경로 이동

            // 거리 기반 공격 추가 - 네비게이션 추가하니까 타워에 닿기 어렵더라
            float distance = Vector3.Distance(transform.position, towerTarget.position);
            
            if(distance < 4) //왜인지는 모르겠는데 여기 숫자에 공격사거리 변수 넣으니 작동이 안됨
            //쌩으로 거리가 디버그용 그거 수치로 대충 적어둠
            {
                //Debug.Log(distance); //디버그용 거리측정
                Tower tower = towerTarget.gameObject.GetComponent<Tower>();
                StartCoroutine(AttackTower(tower));
            }
        }//거리기반 공격 여기까지 ----
    }

    // 타워에 닿고 공격하는건데 혹시 몰라서 남겨둠
    private void OnCollisionEnter(Collision other) //타워에 닿기
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            Tower tower = other.gameObject.GetComponent<Tower>();
            if (tower!=null){
                StartCoroutine(AttackTower(tower));
            }
        }
    }

    IEnumerator AttackTower(Tower tower)
    {
        if(isAttacking)
        {
            yield break; //공격 쿨타임중이면 종료
        }

        isAttacking = true; //공격중으로 활성화
        
        tower.TakeDamage(attackDamage); // 타워한테 공격

        yield return new WaitForSeconds(attackCoolTime); //쿨타임동안 여기 기다리기
        isAttacking = false; //쿨타임 종료 - 공격 해제

    }


    // 독 관련 처리 ---
    public void ApplyPoison(int firstDamage, int dotDamage, float duration, float DamageTime){
        TakeDamage(firstDamage); //체력 감소람수 실행

        if(poisonRoutine !=null){
            StopCoroutine(poisonRoutine);
            // 만약 이미 독이 돌고 있다면 이전 도트딜을 멈춘다
            // 쿨타임 갱신하기 전의 빌드업
        }
        poisonRoutine = StartCoroutine(PoisonTick(dotDamage, duration,DamageTime));
        // 이전 도트딜 끊고 다시 쿨타임을 갱신하고
        // 도트딜을 부여하는 함수(PoisonTick) 실행


    }

    IEnumerator PoisonTick(int dot, float duration, float DamageTime){
        float remain = duration;

        while (remain>0){
            TakeDamage(dot);
            yield return new WaitForSeconds(DamageTime);//DamageTime초 기다리고 다음줄 실행
            remain -= DamageTime; //쿨타임 DamageTime초 감소
        }
        poisonRoutine = null; //모든 과정이 끝나면 널이 됨
    }
    // 독 처리 여기까지 ----

    //얼음 처리----
    public void ApplySlow(float percent) //얼음 발동
    {
        moveSpeed = originalSpeed * percent;
        agent.speed = moveSpeed;
    }

    public void RemoveSlow() //얼음 지속시간 끝
    {
        moveSpeed = originalSpeed;
        agent.speed = originalSpeed;
    }
    //얼음 처리 여기까지 ----

    // 체력감소
    void TakeDamage(int amount){
        hp -= amount;
        Debug.Log(gameObject.name + " 현재 체력: " + hp); // 디버그용: 현재 체력 출력
        
        if(hp<=0){

            IceItemManager iceManager = FindObjectOfType<IceItemManager>();
            if (iceManager != null)
            {
                iceManager.AddGauge(IceGauge); // 예: 적 한 마리 처치 시 게이지 10 증가
            }

            Destroy(gameObject);
            // 체력 0이면 죽음
        }
        
        //임시커밋용 주석

    }
}
