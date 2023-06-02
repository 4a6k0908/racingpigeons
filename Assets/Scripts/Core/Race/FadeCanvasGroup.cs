using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvasGroup : MonoBehaviour
{
    public Button myButton;
    public CanvasGroup canvasGroup1;
    public CanvasGroup canvasGroup2;

    private void Start()
    {
        myButton.onClick.AddListener(Fade);
    }

    public void Fade()
    {
        StartCoroutine(FadeCanvasGroupCoroutine(canvasGroup1, 1, 0, 1));
        StartCoroutine(FadeCanvasGroupCoroutine(canvasGroup2, 0, 1, 1));
    }

    private IEnumerator FadeCanvasGroupCoroutine(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
