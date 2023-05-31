using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ObjectActiveWithTag : MonoBehaviour
{

    [SerializeField] private GameObject[] activeObj;
    [SerializeField] private GameObject[] deactivateObj;
    [SerializeField] private string tagName;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
             for(int i = 0; i < activeObj.Length;i++)
             {
                 activeObj[i].SetActive(true);
             }  
             for(int i = 0; i < deactivateObj.Length;i++)
             {
                 deactivateObj[i].SetActive(false);
             }
        }
    }
}
