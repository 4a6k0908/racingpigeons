using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Specialized;

[RequireComponent(typeof(Animator))]

public class LookAt : MonoBehaviour
{

    protected Animator animator;
    public bool ikActive = false;
    public Transform lookObj = null;
    public float lookWeight = 0.5f;
    public float bodyWeight = 0f;
    public float headWeight = 1f;
    public float eyesWeight = 1f;
    public float desireDist;
    public float maxAngle = -0.5f;
    public float minAngle = -8.5f;

    GameObject objPivot;


    void Start()
    {
        animator = GetComponent<Animator>();
        objPivot = new GameObject("DummyPivot");
        objPivot.transform.parent = transform;
        objPivot.transform.localPosition = new Vector3(0, 1.41f, 0);
    }


    void Update()
    {
        objPivot.transform.LookAt(lookObj);
        float pivotRotY = objPivot.transform.localRotation.y;

        float dist = Vector3.Distance(objPivot.transform.position, lookObj.position);

        if (pivotRotY < maxAngle && pivotRotY > minAngle && dist < desireDist)
        {
            lookWeight = Mathf.Lerp(lookWeight, 1, Time.deltaTime * 1.5f);
        }
        else
        {
            lookWeight = Mathf.Lerp(lookWeight, 0, Time.deltaTime * 1.5f);
        }
    }
    //a callback for calculating IK
    void OnAnimatorIK()
    {

        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(lookWeight, bodyWeight, headWeight, eyesWeight);
                    animator.SetLookAtPosition(lookObj.position);
                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}