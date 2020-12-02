using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{

    public delegate void TakeAction();
    public event TakeAction OnFullFade;
    
    [SerializeField] float fadeDuration;
    [SerializeField] Image imageToFade;

    public void FadeIn()
    {
        StartCoroutine(ActualFadeIn());
    }

    public void ResetAlpha()
    {
        imageToFade.canvasRenderer.SetAlpha(0.0f);
    }

    // Start is called before the first frame update
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
        yield return new WaitForSecondsRealtime(0.6f);
        ResetAlpha();
        Debug.Log("Alpha reset");
    }

}
