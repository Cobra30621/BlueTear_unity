using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    /// <summary>
    /// 負責讀取場景的淡入淡出
    /// </summary>
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
            Debug.Log("StartFadeOut");
            StartCoroutine(FadeOut(1f));
        }

        // 淡入協程
        private IEnumerator FadeIn(float duration)
        {
            yield return new WaitForSeconds(0.2f);
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
}