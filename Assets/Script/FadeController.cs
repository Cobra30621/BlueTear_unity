using UnityEngine;
using System.Collections;
using Script;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public AudioController audioController;
    

    // 開始時淡入
    void Start()
    {
        StartCoroutine(FadeIn(1f));
    }

    // 調用此方法來觸發淡出
    public void StartFadeOut()
    {
        StartCoroutine(FadeOut(1f));
    }

    // 淡入協程
    private IEnumerator FadeIn(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            var value = 1 - (Time.time - startTime) / duration;
            canvasGroup.alpha = value;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    // 淡出協程
    private IEnumerator FadeOut(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            var value = (Time.time - startTime) / duration;
            canvasGroup.alpha = value;
            audioController.FadeVolume(1 - value);
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene("SampleScene");
    }
}