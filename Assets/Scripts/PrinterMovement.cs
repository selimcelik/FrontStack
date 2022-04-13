using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrinterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrinterMove());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PrinterMove()
    {
        gameObject.transform.DOMoveY(-38.2f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        gameObject.transform.DOMoveY(-30f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(PrinterMove());
        
    }
}
