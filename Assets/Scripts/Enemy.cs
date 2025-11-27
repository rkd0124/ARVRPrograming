using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int hp = 30; //체력

    Coroutine poisonRoutine; 
    //^^^코루틴 : 시간 일시정지 같은 느낌인데...
    // 시간 제어하면서 실행을 단계에 맞춰서?? 처리하는 애 라고 생각하면 편함

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name + " 현재 체력: " + hp); // 디버그용: 현재 체력 출력
    }

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

    void TakeDamage(int amount){
        hp -= amount;
        
        if(hp<=0){
            Destroy(gameObject);
            // 체력 0이면 죽음
        }
        
        //임시커밋용 주석

    }
}
