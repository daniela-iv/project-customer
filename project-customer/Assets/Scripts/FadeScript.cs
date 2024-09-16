using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private float timeBeforeFadeOut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (canvasGroup.alpha == 0f)
            {
                fadeIn();
            }
            else
            {
                fadeOut();
            }
        }
    }

    public void fadeIn()
    {
        StartCoroutine("fadeImageIn");
    }

    private IEnumerator fadeImageIn()
    {

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += fadeSpeed;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        StartCoroutine("Timer");
        StopCoroutine("fadeImageIn");
        
    }
    
    private IEnumerator Timer()
    { 
        float timer = timeBeforeFadeOut;

        while (timer > 0f)
        {
            timer -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        fadeOut();
        StopCoroutine("Timer");
        
    }


    public void fadeOut()
    {
        StartCoroutine("fadeImageOut");
    }

    private IEnumerator fadeImageOut()
    {
        while(canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeSpeed;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        StopCoroutine("fadeImageOut");
        
    }
}
