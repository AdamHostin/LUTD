﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DeletePlayerPrefs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        File.Delete(Application.persistentDataPath + "/LUTD.data");
    }


}
