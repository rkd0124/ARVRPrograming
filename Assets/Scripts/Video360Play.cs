using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Video;

public class Video360Play : MonoBehaviour
{
    VideoPlayer vp;

    public VideoClip[] vcList; //����Ʈ�� ���� ���� ���� �Ҵ� ����
    int curVCidx;
    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.clip = vcList[0];
        curVCidx = 0; //���� ����ǰ��ִ� ���� Ŭ���� �ε���
        vp.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket)) // [�� ������ ������ ���
        {
            curVCidx = curVCidx - 1; //�� ���� �̹Ƿ� -1�� �������.
            if (curVCidx < 0) //�ٵ� ������ ����Ҽ� ���⋚���� ������ �޾��ֱ�.
            {
                curVCidx = curVCidx + vcList.Length; //vsList�� �ִ� ��ü�� (3)�� ������ �� curVCidx�� ������. �׷��� 2��°�� ���
            }
            vp.clip = vcList[curVCidx]; //������ ����� ���� �ش��ϴ� ���� ���
        }
        if (Input.GetKeyDown(KeyCode.RightBracket)) // ]�� ������ �� ������ ���
        {
            curVCidx = curVCidx + 1;
            if (curVCidx >= vcList.Length) //���� ���ڰ� 3���� Ŀ����.....
            {
                curVCidx = curVCidx - vcList.Length; //vsList�� �ִ� ��ü�� (3)���� ���� �ٽ� ���ƿ���.
            }
            vp.clip = vcList[curVCidx];
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            curVCidx = (curVCidx - 1 + vcList.Length) % vcList.Length;//������ ����
        }
    }

    public void SetVideoPlay(int num)
    {
        if(curVCidx != num) //���� ������� �ε����� ���� ������, 
        {
            vp.Stop(); //�� ���� ���߰�
            vp.clip = vcList[num]; //�ٸ� �������� ��ȯ
            curVCidx = num;
            vp.Play(); //�ٲ� ������ ���
        }
    }
}
