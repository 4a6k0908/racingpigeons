using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Specialized;
using RootMotion.FinalIK;

public class LookAtAngleDetect : MonoBehaviour
{
    public Transform lookObj = null;
    public float maxDist = 2.5f;
    public float minDist = 0.5f;
    public float maxAngle = -0.5f;
    public float minAngle = -8.5f;

    GameObject objPivot;

    private LookAtController lookAtController;

    private void Start()
    {
        objPivot = new GameObject("DummyPivot");
        objPivot.transform.parent = transform;
        objPivot.transform.localPosition = new Vector3(0, 1.41f, 0);

        lookAtController = GetComponent<LookAtController>();
    }

    private void Update()
    {
        objPivot.transform.LookAt(lookObj);

        float pivotRotY = objPivot.transform.localRotation.y;
        float pivotPosX = objPivot.transform.localPosition.x;
        float pivotPosZ = objPivot.transform.localPosition.z;


        float dist = Vector3.Distance(objPivot.transform.position, lookObj.position);

        if (pivotRotY < maxAngle && pivotRotY > minAngle && dist < maxDist && dist >= minDist)
        {
            lookAtController.weight = 1f;
        }
        else
        {
            lookAtController.weight = 0.001f;
        }
    }
}
