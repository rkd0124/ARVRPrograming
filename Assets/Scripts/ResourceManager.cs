using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int maxBombCount = 3;  // 폭탄 최대 개수
    public int bombCount = 3; // 현재 폭탄 개수

    public float maxIceGauge = 100f;   // 얼음 게이지 최대값
    public float iceGauge = 100f;  // 현재 얼음 게이지

    public WeaponSystem[] weaponSystems; //무기들     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 웨이브 승리 시 모든 리소스 초기화
    public void RechargeAllResources()
    {
        bombCount = maxBombCount; // 폭탄 충전
        IceItemManager iceManager = FindObjectOfType<IceItemManager>(); //얼음
        if (iceManager != null)
        {
            iceManager.FullCharge(); //얼음 호출
            iceGauge = maxIceGauge; 
        }
        foreach (var w in weaponSystems) w.ResetWeapon(); // 무기 초기화
    }
    
}

[System.Serializable]
public class WeaponSystem
{
    public string weaponName; // 무기 이름
    public float cooldown = 5f; // 쿨타임 예시
    public float currentCooldown = 0f; // 현재 쿨타임

    // 무기 초기화(쿨/게이지 리셋)
    public void ResetWeapon()
    {
        currentCooldown = 0f; // 쿨타임 초기화
        // 만약 게이지가 있으면 여기서 초기화
    }
}
