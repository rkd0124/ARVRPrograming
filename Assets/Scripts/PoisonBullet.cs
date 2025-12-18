using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    [Header("Damage")]
    public int firstDamage = 5;
    public int dotDamage = 1;
    public float dotDuration = 3f;
    public float dotInterval = 1f;

    [Header("Bullet")]
    public float speed = 20f;
    public float lifeTime = 2f;
    public float maxDistance = 10f;

    [Header("Hit VFX")]
    public GameObject hitVFXPrefab;   // ⭐ 인스펙터에서 넣을 이펙트
    public float vfxLifeTime = 2f;    // 이펙트 자동 삭제 시간

    private Vector3 startPos;
    private float elapsedTime = 0f;

    public void Activate(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.forward = direction;
        startPos = position;
        elapsedTime = 0f;
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= lifeTime || Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // ===== 💥 히트 이펙트 생성 =====
            SpawnHitVFX(transform.position);

            // ===== 데미지 처리 =====
            var enemyTk = other.GetComponent<Enemy_Tk>();
            var enemyNK = other.GetComponent<Enemy_NK>();
            var enemyFly = other.GetComponent<Enemy_fly>();

            if (enemyTk != null)
                enemyTk.ApplyPoison(firstDamage, dotDamage, dotDuration, dotInterval);
            else if (enemyNK != null)
                enemyNK.ApplyPoison(firstDamage, dotDamage, dotDuration, dotInterval);
            else if (enemyFly != null)
                enemyFly.ApplyPoison(firstDamage, dotDamage, dotDuration, dotInterval);

            gameObject.SetActive(false);
        }
    }

    // ===============================
    // 히트 이펙트 생성 (1회)
    // ===============================
    void SpawnHitVFX(Vector3 hitPosition)
    {
        if (hitVFXPrefab == null) return;

        GameObject vfx = Instantiate(
            hitVFXPrefab,
            hitPosition,
            Quaternion.identity
        );

        Destroy(vfx, vfxLifeTime); // ⭐ 자동 정리
    }
}

