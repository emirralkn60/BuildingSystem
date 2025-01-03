using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketKontrol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            Destroy(gameObject);
        }
    }
}