using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DeletePlayerPrefs : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        File.Delete(Application.persistentDataPath + "/LUTD.data");
    }

    private void Update()
    {
        
    }

}
