using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    bool isGrabbing = false;

    GameObject grabbedObject;

    public LayerMask GraabedLayer;

    public float grabRang = 0.2f;


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
    }
}
