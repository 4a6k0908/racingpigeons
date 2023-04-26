using UnityEngine;

namespace Core.Utils
{
    // 用來顯示遊戲中目前的偵數
    public class GameFPS : MonoBehaviour
    {
        private float  updateInterval = 0.1f;
        private float  nextUpdate;
        private float  deltaTime;
        private int    frameCount;
        private float  fps;
        private string fpsText;

        private void Start()
        {
            nextUpdate = Time.realtimeSinceStartup + updateInterval;
        }

        private void Update()
        {
            frameCount++;
            deltaTime += Time.deltaTime;

            if (!(Time.realtimeSinceStartup > nextUpdate))
                return;

            fps        =  frameCount / deltaTime;
            fpsText    =  $"FPS: {fps:F2}";
            deltaTime  =  frameCount = 0;
            nextUpdate += updateInterval;
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle {
                fontSize = 24,
                normal = {
                    textColor = Color.white,
                },
            };

            GUI.Label(new Rect(10, 10, 200, 100), fpsText, style);
        }
    }
}