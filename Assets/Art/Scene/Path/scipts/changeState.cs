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
        pigeonAnim.Play("00待機");
    }
    public void takeOff()
    {
         pigeonAnim.Play("04起飛");
    }
    public void landing()
    {
         pigeonAnim.Play("06降落");
    }
     public void lookAround()
    {
         pigeonAnim.Play("07左右看");
    }

    public void walk()
    {
        pigeonAnim.Play("08走路");
    }
    public void eat()
    {
        pigeonAnim.Play("03吃東西");
    }
}
