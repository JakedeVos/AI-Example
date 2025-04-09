using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeTrigger : MonoBehaviour
{
    private AIController ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponentInParent<AIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Set is in vision cone to true
            ai.SetPlayerInVisionCone(true);
            Debug.Log("Player Is In Cone!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Set is in vision cone to false
            ai.SetPlayerInVisionCone(false);
        }
        
    }
}
