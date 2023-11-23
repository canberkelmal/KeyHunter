using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSc : MonoBehaviour
{
    public bool breaked = false;

    // Update is called once per frame
    void Update()
    {
        if (breaked)
        {
            breaked = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

        }
    }
}
