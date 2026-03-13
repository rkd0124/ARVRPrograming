using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class GazePointerCtrl : MonoBehaviour
{
    public Transform uiCanvas; //ĵ����
    public UnityEngine.UI.Image gazeImg; //ĵ������ �鰥 �̹���
    public Video360Play vp360; //360���Ǿ �߰��� ���� �÷��� ���
    public Transform vrCamera; //ī�޶� ����

    Vector3 defalutScale;
    public float uiScaleVal = 1f;

    bool isHitObj; //���ͷ����� �Ͼ�� ������Ʈ�� �ü��� ������ Ʈ��, ���� ������ �Ƚ�
    GameObject preHitObj; // ���� �������� �ü��� �ӹ����� ������Ʈ ���� ��� ����
    GameObject curHitObj; //���� �������� �ü��� �ӹ����� ������Ʈ ������ ��� ����
    //float curGazeTime;
    public float gazeChargeTime = 3.0f; //�ü��� �ӹ��� �ð� üũ
    float curGazeTime = 0f; //������ ������ �ð� (�ʱ�ȭ)

    // Start is called before the first frame update
    void Start()
    {
        defalutScale = uiCanvas.localScale;
        curGazeTime = 0f; //�ü�üũ ���� ���� ���������� �ʱ�ȭ
    }

    // Update is called once per frame
    void Update()
    {
        // ĵ���� ������Ʈ�� �������� �Ÿ��� ���� ����
        // 1. ī�޶� �������� ���� ������ ��ǥ ���� ��� (����)
        Vector3 dir = vrCamera.forward;
        Ray ray = new Ray(vrCamera.position, dir);
        RaycastHit hitInfo;
        // 3. ���̿� �΋H�� ��� �Ÿ����̿��� uiCanvas�� ũ�⸦ ����
        if (Physics.Raycast(ray, out hitInfo))
        {
            uiCanvas.localScale = defalutScale * uiScaleVal * hitInfo.distance;
            uiCanvas.position = vrCamera.position + vrCamera.forward * hitInfo.distance; // UI ��ġ�� �浹 ���� ��ó�� �̵� (ī�޶� ���ʿ� ��ġ)
            if (hitInfo.transform.tag == "GazeObj")
            {
                isHitObj = true;
            }
            curHitObj = hitInfo.transform.gameObject; // ���� �ü��� ���� ������Ʈ ����
        }
        else // 4. �浹 �߻� ���ϴ� ��� -> �⺻ ������ ������ uiCanvasũ�� ����
        {
            uiCanvas.localScale = defalutScale * uiScaleVal;
            uiCanvas.position = vrCamera.position + vrCamera.forward * 2.0f; 
        }
        // 5. uiCanvas�� ����ڸ� �ٶ󺼼� �ֵ��� ���� (���� ������ �ݴ�� �ٲٱ�)
        uiCanvas.rotation = vrCamera.rotation;


        //������ ó��
        if (isHitObj) //������Ʈ�� ���̰� �������
        {
            if (curHitObj == preHitObj) //�浹�� �ٶ󺸴°� ������ -> �ٶ󺸰������� ��������
            {
                curGazeTime = curGazeTime + Time.deltaTime; //�ٶ󺼶� �ð�����, ������ ����
            }
            else
            {
                preHitObj = curHitObj; //���� �������� ���� ���� ������Ʈ
            }
            HitObjChecker(curHitObj, true); // ���� �ٶ󺸴� ������Ʈ�� "�ü��� ��Ҵ�" ��ȣ ����
        }
        else //������Ʈ�� �ٶ󺸰� ���� ������
        {
            curGazeTime = 0;
            if(preHitObj != null)
            {
                HitObjChecker(preHitObj, false);
                preHitObj = null;
            }
        }

        curGazeTime = Mathf.Clamp(curGazeTime, 0, gazeChargeTime); //�ü��� �ӹ��� �ð��� �ּ� �ִ� ���� ��� / �ü� ���� �ð� ���� (0 ~ gazeChargeTime ���̷� ����)
        gazeImg.fillAmount = curGazeTime / gazeChargeTime; //0 ~ 100% ��ǥ��. ������ �������� ���

        //������ ���� �ļ� ��ġ
        isHitObj = false; //���� Ʈ�簡 ��� ���������� �����Ƿ�
        curHitObj = null; //���纸�� ������Ʈ ��� �����
    }

    void HitObjChecker(GameObject hitObj, bool isActive) //��Ʈ�� ������Ʈ Ÿ�Ժ��� �۵� ��� ���� / �浹�� ������Ʈ Ÿ�Կ� ���� ���� ����
    {
        if (hitObj.GetComponent<VideoFrame>())//hit�� ���� �÷��̾� ������Ʈ�� ���� �ִ��� Ȯ�� / ������Ʈ�� VideoFrame ������Ʈ�� ������ �ִٸ�,
        {
            if (isActive)
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(true); // �ü��� ����� �� ���� (��: ���� ���)
            }
            else
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(false); // �ü��� ������ �� ���� (��: ���� ����)
            }
        }

        if (gazeImg.fillAmount >= 1) //���� �������� ����� ���� ������ (�������� �� á����,)
        {
            vp360.SetVideoPlay(hitObj.transform.GetSiblingIndex()); //���� �ε��� �� �޾ƿ���
        }
    }
}
