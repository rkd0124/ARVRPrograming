using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwap : MonoBehaviour
{
    [Header("Weapons Scripts")]
    public List<MonoBehaviour> weaponScripts = new List<MonoBehaviour>();

    [Header("Weapon Models")]
    public List<GameObject> weaponModels = new List<GameObject>();

    [Header("Weapon UI")]
    public Canvas weaponUICanvas;                 // UI 캔버스
    public TextMeshProUGUI weaponNameText;         // 무기 이름 텍스트
    public List<string> weaponUIText = new List<string>(); // 무기별 표시 텍스트

    private int currentWeaponIndex = 0;

    void Start()
    {
        ApplyWeapon(currentWeaponIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || ARAVRInput.GetDown(ARAVRInput.Button.One))
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= weaponScripts.Count)
                currentWeaponIndex = 0;

            ApplyWeapon(currentWeaponIndex);
        }
    }

    void ApplyWeapon(int index)
    {
        // ===== 무기 스크립트 =====
        for (int i = 0; i < weaponScripts.Count; i++)
        {
            if (weaponScripts[i] != null)
                weaponScripts[i].enabled = (i == index);
        }

        // ===== 무기 모델 =====
        for (int i = 0; i < weaponModels.Count; i++)
        {
            if (weaponModels[i] != null)
                weaponModels[i].SetActive(i == index);
        }

        // ===== UI 텍스트 =====
        if (weaponNameText != null && index < weaponUIText.Count)
        {
            weaponNameText.text = weaponUIText[index];
        }

        Debug.Log($"무기 변경: Index {index}");
    }
}

