using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bomb : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 5f; // 폭발 범위
    public int explosionDamage = 10; // 폭발 데미지
    public LayerMask targetLayer; // 적 레이어 (Enemy)
    public GameObject explosionEffect; // 폭발 이펙트 프리팹

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 'Player' 태그를 가지고 있다면 무시
        if (other.CompareTag("Player"))
        {
            return; 
        }

        // 플레이어가 아니라면 (적, 바닥, 벽 등) 폭발
        Explode();
    }

    void Explode()
    {
        // 1. 폭발 이펙트 생성
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 2. 범위 내 적 감지
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            var enemyTk = hit.GetComponent<Enemy_Tk>();
            var enemyNK = hit.GetComponent<Enemy_NK>();
            var enemyFly = hit.GetComponent<Enemy_fly>();

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

        // 3. 폭탄 오브젝트 제거
        Destroy(gameObject);
    }
    // 범위 확인용 기즈모
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
