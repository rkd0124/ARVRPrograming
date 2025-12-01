using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    public int firstDamage = 5; // 1차 데미지
    public int dotDamage = 1; // 도트 데미지
    public float dotDuration = 3f; // 도트 지속시간
    public float dotInterval = 1f; // 1초마다 도트딜 적용
    public float speed = 20f; // 독 이동속도
    public float lifeTime = 2f; //독 생존시간
    public float maxDistance = 10f; //사거리

    private Vector3 startPos; // 시작 위치
    private float elapsedTime = 0f; // 경과 시간


    // Start is called before the first frame update

    public void Activate(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.forward = direction;
        startPos = position;
        elapsedTime = 0f;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //독 이동
        transform.position += transform.forward * speed * Time.deltaTime;

        // 경과 시간 업데이트
        elapsedTime += Time.deltaTime;

        // 사거리나 생존시간 체크
        if (elapsedTime >= lifeTime || Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            gameObject.SetActive(false); //총알 삭제
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // 적레이어에게만 작동
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {// 적에게 1차 데미지 + 도트 적용
                enemy.ApplyPoison(firstDamage, dotDamage, dotDuration, dotInterval);
            }

            gameObject.SetActive(false); // 총알 충돌 후 삭제
        }
        
    }
    //임시커밋용 주석

}
