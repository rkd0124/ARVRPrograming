using System.Collections;
using UnityEngine;
using TMPro;

public class Player_BombLauncher : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    public Transform throwPoint;

    [Header("Use Count")]
    public int maxUses = 3;
    public int currentUses = 0;

    [Header("Cooldown")]
    public float cooldown = 1.5f;
    private bool isCooldown = false;

    [Header("Gaze Interaction")]
    public float interactionDistance = 10f;
    public float requiredLookTime = 3.0f;
    private float lookTimer = 0f;
    private Transform mainCameraTransform;

    [Header("UI")]
    public TextMeshProUGUI gazePercentText; // ⭐ 퍼센트 표시
    public TextMeshProUGUI bombCountText;   // ⭐ 폭탄 횟수 표시

    private int bombLayer;

    void Start()
    {
        if (Camera.main != null)
            mainCameraTransform = Camera.main.transform;
        else
            Debug.LogError("Main Camera가 씬에 없습니다!");

        bombLayer = LayerMask.NameToLayer("Bomb");

        UpdateBombCountUI();
        UpdateGazeUI();
    }

    void Update()
    {
        CheckBombMaker();

        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptThrow();
        }
    }

    void AttemptThrow()
    {
        if (currentUses >= maxUses) return;
        if (isCooldown) return;

        SpawnBomb();
    }

    void CheckBombMaker()
    {
        Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("BombMaker"))
            {
                lookTimer += Time.deltaTime;
                UpdateGazeUI();

                if (lookTimer >= requiredLookTime)
                {
                    AttemptThrow();
                    lookTimer = 0f;
                    UpdateGazeUI();
                }
                return;
            }
        }

        lookTimer = 0f;
        UpdateGazeUI();
    }

    void SpawnBomb()
    {
        isCooldown = true;
        currentUses++;
        UpdateBombCountUI();

        StartCoroutine(CooldownRoutine());

        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
        if (bombLayer != -1)
            bomb.layer = bombLayer;
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }

    public void ResetBombs()
    {
        currentUses = 0;
        isCooldown = false;
        UpdateBombCountUI();
    }

    // ===============================
    // UI 업데이트
    // ===============================

    void UpdateGazeUI()
    {
        if (gazePercentText == null) return;

        float percent = Mathf.Clamp01(lookTimer / requiredLookTime) * 100f;
        gazePercentText.text = $"{percent:0}%";
    }

    void UpdateBombCountUI()
    {
        if (bombCountText == null) return;

        int remain = Mathf.Max(0, maxUses - currentUses);
        bombCountText.text = $"Bomb : {remain} / {maxUses}";
    }
}
