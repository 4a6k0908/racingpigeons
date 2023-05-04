using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeState : MonoBehaviour
{
    public Animator pigeonAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Idle()
    {
        pigeonAnim.Play("00�ݾ�");
    }
    public void takeOff()
    {
         pigeonAnim.Play("04�_��");
    }
    public void landing()
    {
         pigeonAnim.Play("06����");
    }
     public void lookAround()
    {
         pigeonAnim.Play("07���k��");
    }

    public void walk()
    {
        pigeonAnim.Play("08����");
    }
    public void eat()
    {
        pigeonAnim.Play("03�Y�F��");
    }
}
