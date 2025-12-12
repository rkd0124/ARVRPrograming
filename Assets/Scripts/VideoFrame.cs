using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video; //<-�̰� �߿� ���迡 ����. ����Ƽ���� ���� ������ �̷��� using�������.

public class VideoFrame : MonoBehaviour
{
    VideoPlayer vp;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        //��������� ����
        vp.Stop(); //Video Frame�� �ִ� Video player�� ������ ���߰� ����
    }

    // Update is called once per frame
    void Update()
    {
        //if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        //{
        //    vp.Stop();
        //}


        //���� ���
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch))
        {
            
            if (vp.isPlaying)
            {
                Debug.Log("Pause");
                vp.Pause();
            }
            else
            {
                Debug.Log("Play");
                vp.Play();
            }
        }
    }
    
    public void CheckVideoFrame(bool Checker) //��������������Ʈ�ѿ��� ���� ����� ��Ʈ���ϱ� ���� �Լ�
    {
        if (Checker)
        {
            if (!vp.isPlaying) //������� �ʴ´ٸ� ��� (!)
            {
                vp.Play();
            }
        }
        else
        {
            vp.Stop();
        }
    }
}
