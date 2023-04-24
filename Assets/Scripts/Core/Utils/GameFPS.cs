using UnityEngine;

namespace Core.Utils
{
    public class GameFPS : MonoBehaviour
    {
        private float  updateInterval = 0.5f;
        private float  nextUpdate;
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
            
            if (!(Time.realtimeSinceStartup > nextUpdate))
                return;
            
            fps        =  frameCount / updateInterval;
            fpsText    =  $"FPS: {fps:F2}";
            frameCount =  0;
            nextUpdate += updateInterval;
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle {
                fontSize = 24,
                normal   = {
                    textColor = Color.white,
                },
            };
            
            GUI.Label(new Rect(10, 10, 200, 100), fpsText, style);
        }
    }
}