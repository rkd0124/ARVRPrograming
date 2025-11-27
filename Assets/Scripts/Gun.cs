using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform poisonImpact; // 독 피격
    ParticleSystem poisonEffect; //독 파편 파티클
    AudioSource poisonAudio; // 독 사운드
    public Transform crosshair; //크로스헤어

    //여기 변수들은 자료에 없던거
    float CoolTime = 0.5f; // 쿨타임
    float lastFireTime = 0f; //마지막으로 총 발사한거 *쿨타임 구현에 필요

    int no1Damage = 5; //최초 데미지
    int DotDamage = 1; //도트뎀
    float DotDamageTime = 1; //도트뎀 들어가는 주기
    float DotTime = 3; //도트뎀 지속시간

    public PoisonFactory bulletPool; // 총알 풀 연결



    // Start is called before the first frame update
    void Start()
    {
        poisonEffect = poisonImpact.GetComponent<ParticleSystem>();
        poisonAudio = poisonImpact.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //크로스헤어 ---
        ARAVRInput.DrawCrosshair(crosshair);
        //크로스헤어 여기까지---
        //1번 무기 : 독
        if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger)){
            //독 발사 실행
            if(Time.time - lastFireTime < CoolTime){ 
                return;
            }//만약 쿨타임이 다 안끝남 = 발사 ㄴㄴ

            PoisonFire(); //쿨타임이 끝났다면 실행됨
            //독 발사 여기까지
            
        }
        //이 코드들은 마우스용 코드임-----
        if (Input.GetMouseButtonDown(1))
        {
            if(Time.time - lastFireTime < CoolTime){ 
                return;
            }//만약 쿨타임이 다 안끝남 = 발사 ㄴㄴ

            PoisonFire();
        }
        // 마우스용 코드 여기까지-------
    }

    void PoisonFire()
    {
        poisonEffect.Stop();
        poisonEffect.Play();

        GameObject posion = bulletPool.GetBullet();

        /* //ARVR용 코드----
        GameObject posion = bulletPool.GetBullet();
        posion.transform.position = ARAVRInput.RHandPosition;
        posion.transform.forward = ARAVRInput.RHandDirection;
        // ARVR----*/

        //마우스용 코드
        Camera cam = Camera.main;
        posion.transform.position = cam.transform.position + cam.transform.forward * 0.5f;
        posion.transform.forward = cam.transform.forward;
        //마우스---
    
        lastFireTime = Time.time;
    }
}
