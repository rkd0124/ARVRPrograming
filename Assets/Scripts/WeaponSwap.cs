using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [Header("Weapons.cs")] // 무기 스크립트
    public Gun poisonScript; // 독 무기 스크립트 넣으면 됨 (gun.cs)
    public Fire fireScript; // 방사형 무기 스크립트 넣기 (Fire.cs)

    [Header("Models")] // 무기 모델링
    public GameObject poisonModel; //독총
    public GameObject fireModel; //방사총

    private bool isPoisonMode = true; // 지금 무기상태가 독무기인지 확인
    //트루이면 독, 그게 아니면 방사형

    // Start is called before the first frame update
    void Start()
    {
        // 스크립트가 널이면자동으로 스크립트 연결
        if (poisonScript == null) poisonScript = GetComponent<Gun>(); 
        if (fireScript == null) fireScript = GetComponent<Fire>();
        SetWeaponMode(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || ARAVRInput.GetDown(ARAVRInput.Button.One))
        {
            isPoisonMode = !isPoisonMode; // 상태 반전 = true -> false or false -> true
            SetWeaponMode(isPoisonMode);  // 무기 적용
        }
    }

    void SetWeaponMode(bool usePoison)
    {
        if (usePoison)
        {
            // 1. 독 모드 활성화
            if (poisonScript != null) poisonScript.enabled = true; // 스크립트 켜기
            if (fireScript!= null) fireScript.enabled = false;    // 스크립트 끄기
            
            // (모델이 있다면)
            if (poisonModel != null) poisonModel.SetActive(true);
            if (fireModel != null) fireModel.SetActive(false);

            Debug.Log("무기 교체: 독 (Poison)");
        }
        else
        {
            // 2. 방사 모드 활성화
            if (poisonScript != null) poisonScript.enabled = false;
            if (fireScript!= null) fireScript.enabled = true;

            // (모델이 있다면)
            if (poisonModel != null) poisonModel.SetActive(false);
            if (fireModel != null) fireModel.SetActive(true);

            Debug.Log("무기 교체: 방사 (Fire)");
        }
    }
}
