using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;



public class BarController : MonoBehaviour
{
    [SerializeField] bool isFullOnStart;
    [SerializeField] bool isTextVisible;
    [SerializeField] bool isXpBar;
    [SerializeField] string postFix;
    [SerializeField] float speed;
    [SerializeField] Image frontBar;
    [SerializeField] Image backBar;
    [SerializeField] Color32 addColor;
    [SerializeField] Color32 subColor;
    [SerializeField] TextMeshProUGUI text;    
    [SerializeField] Gradient frontBarColorGradient;
    [Tooltip("in Frames")]
    [SerializeField] int barChangeFrequency = 1;
    private float currentVal;

    private void Start()
    {
        if (isFullOnStart)
        {
            frontBar.fillAmount = 1f;
            backBar.fillAmount = 1f;
            frontBar.color = frontBarColorGradient.Evaluate(frontBar.fillAmount);
            currentVal = 1f;
        }
        else
        {
            frontBar.fillAmount = 0f;
            backBar.fillAmount = 0f;
            frontBar.color = frontBarColorGradient.Evaluate(frontBar.fillAmount);
            currentVal = 0f;
        }
    }

    public void OnUIUpdate(float newBarVal , int newVal = 0, int maxVal = 0)
    {
        if(isTextVisible) ChangeText(newVal, maxVal);
        if (newBarVal > currentVal)
        {
            AddBarAmmount(newBarVal);
        }
        else
        {
            SubBarAmmount(newBarVal);
        }
        currentVal = newBarVal;

    }

    private void ChangeText(int newVal, int maxVal)
    {
        text.text = newVal + " / " + maxVal + " " + postFix;        
    }

    private void SubBarAmmount(float newBarVal)
    {
        backBar.color = subColor;
        frontBar.fillAmount = Mathf.Clamp(newBarVal, 0f, 1f);
        frontBar.color = frontBarColorGradient.Evaluate(frontBar.fillAmount);
        StartCoroutine(SubBarOverTime());
    }

    IEnumerator SubBarOverTime()
    {
        while (backBar.fillAmount > frontBar.fillAmount)
        {           
            backBar.fillAmount = Mathf.Lerp(backBar.fillAmount, 0, Speed * Time.deltaTime);
            yield return barChangeFrequency;
        }
    }

    private void AddBarAmmount(float newBarVal)
    {
        backBar.color = addColor;
        backBar.fillAmount = Mathf.Clamp(newBarVal,0f,1f);
        StartCoroutine(AddBarOverTime());

    }

    IEnumerator AddBarOverTime()
    {
        while (backBar.fillAmount > frontBar.fillAmount)
        {
            frontBar.fillAmount = Mathf.Lerp(frontBar.fillAmount, 1, Speed * Time.deltaTime);
            frontBar.color = frontBarColorGradient.Evaluate(frontBar.fillAmount);
            yield return barChangeFrequency;
        }        
    }
}
