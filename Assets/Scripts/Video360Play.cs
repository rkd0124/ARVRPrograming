using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Video;

public class Video360Play : MonoBehaviour
{
    VideoPlayer vp;

    public VideoClip[] vcList; //리스트로 만들어서 여러 비디오 할당 가능
    int curVCidx;
    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.clip = vcList[0];
        curVCidx = 0; //현재 실행되고있는 비디오 클립의 인덱스
        vp.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket)) // [를 누르면 그전꺼 출력
        {
            curVCidx = curVCidx - 1; //그 전꺼 이므로 -1을 해줘야함.
            if (curVCidx < 0) //근데 음수를 출력할순 없기떄문에 조건을 달아주기.
            {
                curVCidx = curVCidx + vcList.Length; //vsList에 있는 전체값 (3)을 음수가 된 curVCidx에 더해줌. 그러면 2번째께 재생
            }
            vp.clip = vcList[curVCidx]; //위에서 계산한 값에 해당하는 비디오 재생
        }
        if (Input.GetKeyDown(KeyCode.RightBracket)) // ]를 누르면 그 다음꺼 출력
        {
            curVCidx = curVCidx + 1;
            if (curVCidx >= vcList.Length) //만약 숫자가 3보다 커지면.....
            {
                curVCidx = curVCidx - vcList.Length; //vsList에 있는 전체값 (3)에서 빼서 다시 돌아오게.
            }
            vp.clip = vcList[curVCidx];
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            curVCidx = (curVCidx - 1 + vcList.Length) % vcList.Length;//나머지 연산
        }
    }

    public void SetVideoPlay(int num)
    {
        if(curVCidx != num) //현재 재생중인 인덱스와 같지 않을때, 
        {
            vp.Stop(); //그 영상 멈추고
            vp.clip = vcList[num]; //다른 영상으로 교환
            curVCidx = num;
            vp.Play(); //바뀐 영상을 재생
        }
    }
}
