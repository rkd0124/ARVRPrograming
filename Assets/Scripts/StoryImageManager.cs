using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryImageManager : MonoBehaviour
{
    [Header("UI Components")]
    public Image displayImage; // 화면에 보여줄 UI Image 컴포넌트

    [Header("Resources")]
    public Sprite[] storyImages; // 보여줄 이미지 4장 (순서대로 넣으세요)

    [Header("Scene Settings")]
    public string nextSceneName = "MainWaveScene"; // 다음으로 넘어갈 씬 이름

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
       UpdateImage(); //시작하자마자 이미지 1번째꺼 보여주기 
    }

    // Update is called once per frame
    void Update()
    {
        // 컴터 테스트용 스페이스바 랑 a버튼
        if (Input.GetKeyDown(KeyCode.Space) || ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch))
        {
            NextSlide();
        }
    }

    void NextSlide()
    {
        // 인덱스를 1 증가시킴 (다음 장으로)
        currentIndex++;

        // 아직 보여줄 이미지가 남았다면 (0, 1, 2, 3번 인덱스까지)
        if (currentIndex < storyImages.Length)
        {
            UpdateImage();
        }
        // 이미지가 끝났다면 (4번 인덱스가 되면) -> 씬 전환
        else
        {
            Debug.Log("이미지끝");
            SceneManager.LoadScene(nextSceneName);
        }
    }
    void UpdateImage()
    {
        // 이미지가 연결되어 있는지 확인
        if (displayImage != null && storyImages.Length > 0 && currentIndex < storyImages.Length)
        {
            displayImage.sprite = storyImages[currentIndex];
        }
    }
}
