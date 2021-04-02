using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealButtonUI : MonoBehaviour
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
        gameObject.GetComponent<Button>().onClick.AddListener(App.player.StartHealing);
    }

    private void OnEnable()
    {
        App.player.healingEndedEvent.AddListener(CancelHealing);
    }

    public void CancelHealing()
    {
        isPressed = false;
        image.sprite = originalImage;
    }

    public void OnClick()
    {
        App.audioManager.Play("UIButtonClicked");
        if (isPressed) image.sprite = originalImage;
        else image.sprite = pressedImage;
        App.player.vaccinationEndedEvent.Invoke();

        isPressed = !isPressed;
    }
}