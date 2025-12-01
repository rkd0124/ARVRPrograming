using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float fallSpeed = 4f; // 낙하 속도
    int damage = 0; // 데미지 저장
    Transform target; // 타워 위치

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime; // 아래로 낙하
    }

    public void SetDamage(int dmg) // 데미지 전달받기
    {
        damage = dmg;
    }

    public void SetTarget(Transform t) // 타워 위치 전달받기
    {
        target = t;
    }

    public void AttackTowerBomb(Tower tower)
    {
        tower.TakeDamage(damage);
    }
}
