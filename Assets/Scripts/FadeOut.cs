using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float fadeOutTime;

    private bool isFadeOut = false;
    private float alphavalue = 0f;

    void Start()
    {

    }

    void Update()
    {
        if(isFadeOut)
        {
            alphavalue += 0.05f;
        }
        else
        {
            alphavalue -= 0.05f;
        }

        alphavalue = Mathf.Clamp(alphavalue, 0.0f, 1.0f);
        //Debug.Log(alphavalue);
        this.GetComponentInChildren<Image>().color = new Color(0, 0, 0, alphavalue);
        Color oriTextColor = this.GetComponentInChildren<Text>().color;
        this.GetComponentInChildren<Text>().color = new Color(oriTextColor.r, oriTextColor.g, oriTextColor.b, alphavalue);
    }
    public void DoFadeOut()
    {
        StartFadeOut();
        Invoke("StopFadeOut", fadeOutTime);
    }
    public void StartFadeOut()
    {
        isFadeOut = true;
    }
    public void StopFadeOut()
    {
        isFadeOut = false;
    }
    public void SetText(string newHint)
    {
        this.GetComponentInChildren<Text>().text = newHint;
    }
}
