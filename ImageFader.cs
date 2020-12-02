using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{

    public delegate void TakeAction();
    public event TakeAction OnFullFade;
    
    [SerializeField] float fadeDuration;
    [SerializeField] float resetDuration;
    [SerializeField] Image imageToFade;

    public void FadeIn()
    {
        StartCoroutine(ActualFadeIn());
    }

    public void ResetAlpha(float duration = 0)
    {
        if(duration == 0)
        {
            duration = resetDuration;
        }

        imageToFade.CrossFadeAlpha(0, duration, true);
    }

    void ResetAlpha()
    {
        imageToFade.canvasRenderer.SetAlpha(0.0f);
    }

    void Start()
    {
        ResetAlpha();
    }

    IEnumerator ActualFadeIn()
    {
        imageToFade.CrossFadeAlpha(1, fadeDuration, true);
        yield return new WaitForSecondsRealtime(fadeDuration);
        OnFullFade();
        Debug.Log("Fullfade");
    }

}
