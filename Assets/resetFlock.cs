using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetFlock : MonoBehaviour
{
    public GameObject StartPos;

    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity += transform.forward*1;
    }

    private void FixedUpdate()
    {
        //gameObject.GetComponent<Rigidbody>().velocity += transform.forward/20;
    }

 
    private void OnTriggerEnter(Collider other)
    {
        gameObject.transform.localPosition = StartPos.transform.localPosition;
    }

}
