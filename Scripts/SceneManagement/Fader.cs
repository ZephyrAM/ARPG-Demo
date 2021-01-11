using System.Collections;
using UnityEngine;

namespace ZAM.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        // Setup Variables \\
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;
        
        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>(); 
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(time, 1);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(time, 0);
        }

        public Coroutine Fade(float time, float target)
        {
            if (currentActiveFade != null) { StopCoroutine(currentActiveFade); }

            currentActiveFade = StartCoroutine(FadeCoroutine(time, target));
            return currentActiveFade;
        }

        private IEnumerator FadeCoroutine(float time, float target)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}