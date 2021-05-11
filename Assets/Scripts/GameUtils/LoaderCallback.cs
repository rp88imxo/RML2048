using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    bool firstUpdate;

    // Start is called before the first frame update
    void Start()
    {
        firstUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUpdate)
        {
            Loader.LoaderCallback();
            firstUpdate = false;
        }
    }
}
