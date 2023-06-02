using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CloseFadeCanvasGroup : MonoBehaviour
{
    public CanvasGroup canvasGroup2;
    public string sceneToLoad; // 要加载的Scene的名称
    public string sceneToUnload; // 要卸载的Scene的名称

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
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

        // 当淡出完成后，加载新的Scene
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);

        // 如果已经设定了要卸载的Scene，那么卸载这个Scene
        if (!string.IsNullOrEmpty(sceneToUnload))
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
}