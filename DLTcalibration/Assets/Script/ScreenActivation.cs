using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenActivation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // GameObject[] objs = GameObject.FindGameObjectsWithTag("ScreenActivation");
        // Debug.Log ("displays connectd: " + Display.displays.Length);
        // if (objs.Length > 1) {
        //     Destroy(gameObject);
        // }

        // DontDestroyOnLoad(gameObject); 
        foreach (var disp in Display.displays){
            disp.Activate(disp.systemWidth, disp.systemHeight, 60);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // foreach (var disp in Display.displays){
        //     disp.Activate(disp.systemWidth, disp.systemHeight, 60);
        // }
    }
}
