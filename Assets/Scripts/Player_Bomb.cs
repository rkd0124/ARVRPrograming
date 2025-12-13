using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bomb : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 10f; // 폭발 범위
    public int explosionDamage = 10; // 폭발 데미지
    public LayerMask targetLayer; // 적 레이어 (Enemy)
    public GameObject explosionEffect; // 폭발 이펙트 프리팹
    
    [Header("Audio")]
    public AudioSource audioSource; // 폭탄 오브젝트의 AudioSource 연결
    public AudioClip fuseSound;     // 퓨즈가 타는 소리 클립
    public AudioClip explosionSound; // 폭발 시 재생할 오디오 클립

    public float fuseTime = 10f; // 10초 뒤 자동 폭발
    private bool hasExploded = false; //중복폭발 방지

    // Start is called before the first frame update
    void Start()
    {
        // 1. 퓨즈 소리 재생 시작
        if (audioSource != null && fuseSound != null)
        {
            audioSource.clip = fuseSound;
            audioSource.loop = true; // 퓨즈 소리는 반복 재생
            audioSource.Play();
        }

        // 2. 퓨즈 시간이 지나면 자동 폭발 예약
        Invoke("Explode", fuseTime); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 'Player' 태그를 가지고 있다면 무시
        if (collision.gameObject.CompareTag("Player"))
        {
            return; 
        }

        // 적에게 직빵으로 맞으면 바로 폭발,
        if (IsEnemy(collision.gameObject))
        {
            Explode();
        }

        // 만약 직빵으로 안 맞으면 폭탄 시간 지나야함
    }

    // 부딪힌 게 적인지 확인하는 함수
    bool IsEnemy(GameObject obj)
    {
        // 적 태그를 쓰거나, 적 스크립트가 있는지 확인
        if (obj.GetComponent<Enemy_Tk>() != null || 
            obj.GetComponent<Enemy_NK>() != null || 
            obj.GetComponent<Enemy_fly>() != null)
        {
            return true;
        }
        return false;
    }

    void Explode()
    {
        //중복 폭발 방지
        if (hasExploded) return;
        hasExploded = true;

        // 1. 퓨즈 소리 정지 (폭발하는 순간 멈춤)
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // 2. 폭발 오디오 재생 (AudioSource.PlayClipAtPoint 사용)
        if (explosionSound != null)
        {
            // 폭발 소리는 폭탄 오브젝트가 사라져도 유지되어야 하므로 PlayClipAtPoint 사용
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        // 3. 폭발 이펙트 생성
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // 4. 범위 내 적 감지 및 데미지 적용
        // 
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            // 자식/부모에 스크립트가 있을 가능성이 있으므로 GetComponentInParent 사용
            var enemyTk = hit.GetComponentInParent<Enemy_Tk>();
            var enemyNK = hit.GetComponentInParent<Enemy_NK>();
            var enemyFly = hit.GetComponentInParent<Enemy_fly>();

            if (enemyTk != null)
            {
                enemyTk.TakeDamage(explosionDamage); // 폭탄은 보통 즉발 데미지
            }
            else if (enemyNK != null)
            {
                enemyNK.TakeDamage(explosionDamage);
            }
            else if (enemyFly != null)
            {
                enemyFly.TakeDamage(explosionDamage);
            }
        }

        // 5. 폭탄 오브젝트 제거
        Destroy(gameObject);
    }

    // 범위 확인용 기즈모
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}