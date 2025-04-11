using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    public GameObject introtext;

    // Start is called before the first frame update
    void Start()
    {
        introtext.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        introtext.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        introtext.SetActive(false);
    }
}
