using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBarConnector : MonoBehaviour
{
    [SerializeField] private BarController waveBar;

    private void OnEnable()
    {
        App.levelManager.waveBar = waveBar;
    }
}
