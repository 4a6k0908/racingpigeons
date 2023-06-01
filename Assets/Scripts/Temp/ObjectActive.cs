using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectActive : MonoBehaviour
{

    [SerializeField] private GameObject[] activeObj;
    [SerializeField] private GameObject[] deactivateObj;
    [SerializeField] private GameObject[] moveObj;
    //[SerializeField] private GameObject[] objectToDestroy;
    [SerializeField] private bool activateTimeline;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private bool activeAnimator;
    [SerializeField] private Animator animator;
    [SerializeField] private string animName;
    [SerializeField] private bool activeLegacyAnim;
    [SerializeField] private Animation anim;
    [SerializeField] private string legacyAnimName;





    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (activeObj != null)
            {
                for (int i = 0; i < activeObj.Length; i++)
                {
                    activeObj[i].SetActive(true);
                }
            }
             
            if(deactivateObj!= null)
            {
                for (int i = 0; i < deactivateObj.Length; i++)
                {
                    deactivateObj[i].SetActive(false);
                }
            }

            if (moveObj != null)
            {
                for (int i = 0; i < moveObj.Length; i++)
                {
                    moveObj[i].transform.position = Vector3.zero;
                }
            }

            //if (objectToDestroy != null)
            //{
            //    for (int i = 0; i < objectToDestroy.Length; i++)
            //    {
            //        Destroy(objectToDestroy[i]);
            //    }
            //}
            

            if (activateTimeline && timeline != null )
            {
                timeline.Play();
            }

            if (activeAnimator && animator != null)
            {
                animator.Play(animName);
            }

            if (activeLegacyAnim && anim != null)
            {
                anim.Play(legacyAnimName);
            }            
        }
    }
}
