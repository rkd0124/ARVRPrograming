using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    bool isGrabbing = false;

    GameObject grabbedObject;

    public LayerMask grabbedLayer;

    public float grabRange = 0.2f;

    Vector3 prevPos;

    float throwPower = 10;

    Quaternion prevRot;
    public float rotPower = 5;

    public bool isRemoteGrab = true;

    public float remoteGrabDistance = 20;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing == false)
        {
            TryGrab();
        }

        else
        {
            TryUngrab();
        }
    }

    private void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.LHandPosition, grabRange, grabbedLayer);
            int closest = -1;
            float closestDistance = float.MaxValue; //가장 가까운 폭탄
            Debug.Log("폭탄 잡았음");

            if(isRemoteGrab)
            {
                Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);

                RaycastHit hitInfo;

                if(Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance, grabbedLayer))
                {
                    isGrabbing = true;

                    grabbedObject = hitInfo.transform.gameObject;

                    StartCoroutine(GrabbingAnimation());
                }

                return;
            }

            for (int i = 0; i < hitObjects.Length; i++){
                var rigid = hitObjects[i].GetComponent<Rigidbody>();
                if (rigid == null) continue;

                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.LHandPosition);

                if (nextDistance < closestDistance)
                {
                    closest = i;
                    closestDistance = nextDistance;
                }
            }

            if (closest >-1)
            {
                isGrabbing = true;

                grabbedObject = hitObjects[closest].gameObject;

                grabbedObject.transform.parent = ARAVRInput.LHand;

                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                prevPos = ARAVRInput.LHandPosition;
                prevRot = ARAVRInput.LHand.rotation;
            }
        }
    }

    IEnumerator GrabbingAnimation()
    {
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

        prevPos = ARAVRInput.LHandPosition;

        prevRot = ARAVRInput.LHand.rotation;

        Vector3 startLocation = grabbedObject.transform.position;

        Vector3 targetLocation = ARAVRInput.LHandPosition + ARAVRInput.LHandPosition * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;

        float elapsedRate = currentTime /finishTime;
        while (elapsedRate<1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation,elapsedRate);

            yield return null;
        }

        grabbedObject.transform.position = targetLocation;
        grabbedObject.transform.parent = ARAVRInput.LHand;

    }

    private void TryUngrab()
    {
        Vector3 throwDirection = (ARAVRInput.LHandPosition - prevPos);
        prevPos = ARAVRInput.LHandPosition;

        Quaternion deltaRotation = ARAVRInput.LHand.rotation * Quaternion.Inverse(prevRot);

        prevRot = ARAVRInput.LHand.rotation;

        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            isGrabbing = false;

            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;

            grabbedObject.transform.parent = null;

            grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

            float angle;
            Vector3 axis;

            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime)*angle*axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;


            grabbedObject = null;
        }
    }
}
