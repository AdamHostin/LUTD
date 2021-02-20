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
    [SerializeField] Image frontBar;
    [SerializeField] Image backBar;
    [SerializeField] Color32 addColor;
    [SerializeField] Color32 subColor;
    [SerializeField] float Speed;
    [SerializeField] TextMeshProUGUI text;
    private float currentVal;

    private void Start()
    {
        if (isFullOnStart)
        {
            frontBar.fillAmount = 1f;
            backBar.fillAmount = 1f;
            currentVal = 1f;
        }
        else
        {
            frontBar.fillAmount = 0f;
            backBar.fillAmount = 0f;
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
        text.text = newVal + " / " + maxVal;        
    }

    private void SubBarAmmount(float newBarVal)
    {
        backBar.color = subColor;
        frontBar.fillAmount = Mathf.Clamp(newBarVal, 0f, 1f);
        StartCoroutine(SubBarOverTime());
    }

    IEnumerator SubBarOverTime()
    {
        while (backBar.fillAmount > frontBar.fillAmount)
        {           
            backBar.fillAmount = Mathf.Lerp(backBar.fillAmount, 0, Speed * Time.deltaTime);
            yield return 1;
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
            yield return 1;
        }        
    }
}
