using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceFadeCanvasGroup : MonoBehaviour
{
    public CanvasGroup canvasGroup1;
    public CanvasGroup canvasGroup2;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(FadeCanvasGroupCoroutine(canvasGroup1, 0, 1, 0.5f, true));
            StartCoroutine(FadeCanvasGroupCoroutine(canvasGroup2, 1, 0, 0.5f, false));
        }
    }

    private IEnumerator FadeCanvasGroupCoroutine(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, bool isFinalStateActive)
    {
        canvasGroup.gameObject.SetActive(true);
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        canvasGroup.gameObject.SetActive(isFinalStateActive);
    }
}
