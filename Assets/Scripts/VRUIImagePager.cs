using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUIImagePager : MonoBehaviour
{
    [Header("UI Images (Canvas 안에 있는 Image들)")]
    public List<Image> uiImages = new List<Image>();

    [Header("Input Settings")]
    public ARAVRInput.Button nextButton = ARAVRInput.Button.One; // A 버튼
    public ARAVRInput.Controller controller = ARAVRInput.Controller.RTouch;

    private int currentIndex = 0;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // A 버튼 눌렀을 때
        if (ARAVRInput.GetDown(nextButton, controller))
        {
            NextImage();
        }
    }

    void NextImage()
    {
        if (uiImages.Count == 0) return;

        currentIndex++;

        if (currentIndex >= uiImages.Count)
            currentIndex = 0;

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < uiImages.Count; i++)
        {
            if (uiImages[i] != null)
                uiImages[i].gameObject.SetActive(i == currentIndex);
        }
    }
}
