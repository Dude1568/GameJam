using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testscript : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoldTracker.GainGold(1000);
            Debug.Log("gain gold");
            
        }
    }
}
