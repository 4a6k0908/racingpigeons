using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFPS : MonoBehaviour
{
    public TMP_Text UI_FPS;

    private int   framesCount = 0;
    private float accumTime = 0.0f;
    

    void show_FPS()
    {
        if (UI_FPS == null)
            return;

        accumTime+= Time.unscaledDeltaTime;
        ++framesCount;

        //多久更新一次
        if (accumTime > 0.5f)
        {
            float fps = framesCount / accumTime;

            UI_FPS.text = string.Format("FPS: {0:f0}", fps);

            accumTime = 0.0f;
            framesCount = 0;
        }
    }

    private void Awake()
    {
        //設定每秒張數 30/60
        Application.targetFrameRate = 30;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        show_FPS();
    }
}
