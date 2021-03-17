using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButtonBehaviour : MonoBehaviour
{
    // yes it has to be there because gameManager is in Difrent scene
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(App.gameManager.ReInitPlayer);
    }

}
