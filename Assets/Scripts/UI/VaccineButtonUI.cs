using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaccineButtonUI : MonoBehaviour
{
    private bool isPressed = false;

    private Image image;
    private Sprite originalImage;
    [SerializeField] private Sprite pressedImage;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        originalImage = image.sprite;
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
        image.sprite = originalImage;
    }

    public void OnClick()
    {
        App.audioManager.Play("UIButtonClicked");
        if (isPressed) image.sprite = originalImage;
        else image.sprite = pressedImage;
        App.player.healingEndedEvent.Invoke();

        isPressed = !isPressed;
    }
}
