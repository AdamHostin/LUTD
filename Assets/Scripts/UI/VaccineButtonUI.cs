using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaccineButtonUI : MonoBehaviour
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
        gameObject.GetComponent<Button>().onClick.AddListener(App.player.StartVaccinating);        
    }

    private void OnEnable()
    {
        App.player.vaccinationEndedEvent.AddListener(CancelVaccination);
    }

    public void CancelVaccination()
    {
        isPressed = false;
        image.color = normalColor;
    }

    public void OnClick()
    {
        if (isPressed) image.color = normalColor;
        else image.color = highlightColor;

        isPressed = !isPressed;
    }
}
