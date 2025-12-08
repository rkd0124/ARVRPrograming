using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    public Gun gun;   // 기본 무기
    public Fire fire; // 전환 무기

    private enum WeaponType
    {
        Gun,
        Fire
    }

    private WeaponType currentWeapon = WeaponType.Gun; // 기본 무기 장착


    // Start is called before the first frame update
    void Start()
    {
        gun.gameObject.SetActive(true);
        fire.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleWeaponSwitch(); // A키로 모드 전환
        HandleWeaponFire();   // 마우스 우클릭 발사
    }

    void HandleWeaponSwitch()
    {
        // VR "A" 버튼 또는 키보드 A로 모드 변환
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentWeapon == WeaponType.Gun) // 지금 무기가 총이면
                SwitchWeapon(WeaponType.Fire); // 방사기로 바꾸고
            else
                SwitchWeapon(WeaponType.Gun); // 그게 아니면 총으로 바꾸고
        }
    }

    void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon; //무기 바꾸기

        // 활성화/비활성화 처리
        gun.gameObject.SetActive(currentWeapon == WeaponType.Gun);
        fire.gameObject.SetActive(currentWeapon == WeaponType.Fire);
    }

    void HandleWeaponFire()
    {
        // 마우스 우클릭으로 발사, 활성화된 무기만 작동
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            if (currentWeapon == WeaponType.Gun)
            {
                gun.PoisonFire(); // Gun 발사
            }
            else if (currentWeapon == WeaponType.Fire)
            {
                // Fire 무기는 isHolding을 0.5초 단위로 tick 처리 필요
                fire.isHolding = true;
            }
        }
        else
        {
            if (currentWeapon == WeaponType.Fire)
                fire.isHolding = false;
        }
    }

}
