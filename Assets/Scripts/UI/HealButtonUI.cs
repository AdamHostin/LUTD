using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealButtonUI : MonoBehaviour
{
    [SerializeField] Color32 highlightColor;
    private bool isPressed = false;
    private Color32 normalColor;
    private Image image;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        normalColor = image.color;
    }

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(App.player.StartHealing);
    }

    private void OnEnable()
    {
        App.player.healingEndedEvent.AddListener(CancelHealing);
    }

    public void CancelHealing()
    {
        isPressed = false;
        image.color = normalColor;
    }

    public void OnClick()
    {
        App.audioManager.Play("UIButtonClicked");
        if (isPressed) image.color = normalColor;
        else image.color = highlightColor;
        App.player.vaccinationEndedEvent.Invoke();

        isPressed = !isPressed;
    }
}