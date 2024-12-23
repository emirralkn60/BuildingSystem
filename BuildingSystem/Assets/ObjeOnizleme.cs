using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjeOnizleme : MonoBehaviour
{
    ObjeBuild objebuild;
    private void Start()
    {
        objebuild = GameObject.FindWithTag("GameManager").GetComponent<ObjeBuild>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Platform"))
        {

            objebuild.EtkilesimVarmi = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Platform"))
        {

            objebuild.EtkilesimVarmi = false;

        }
    }
}
