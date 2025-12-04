using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int hp = 100; //타워 체력
    public int maxHP = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log("타워 체력: "+hp); //디버그용

        if(hp<=0){
            Destroy(gameObject);
        }
    }

    public void RestoreToFull()
    {
        
        hp = maxHP;   // 최대치로 회복
        Debug.Log("타워 체력: "+hp); //디버그용
        // 타워 UI 갱신 등 추가 로직 여기에
    }
}
