using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DancerManager : MonoBehaviour
{
    void OnEnable(){
        StartCoroutine(Dance());
    }   
    IEnumerator Dance(){
        StartCoroutine(UsingLerp(transform.GetChild(0), transform.GetChild(0).localPosition + new Vector3(0, 0, 3), transform.GetChild(0).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(1), transform.GetChild(1).localPosition + new Vector3(0, 0, 6), transform.GetChild(1).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(2), transform.GetChild(2).localPosition + new Vector3(0, 0, 9), transform.GetChild(2).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(3), transform.GetChild(3).localPosition + new Vector3(0, 0, 6), transform.GetChild(3).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(4), transform.GetChild(4).localPosition + new Vector3(0, 0, 3), transform.GetChild(4).localEulerAngles, 0.7f));
        yield return new WaitForSeconds(1.5f);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Dance");
        transform.GetChild(1).GetComponent<Animator>().SetTrigger("Dance");
        transform.GetChild(2).GetComponent<Animator>().SetTrigger("Dance");
        transform.GetChild(3).GetComponent<Animator>().SetTrigger("Dance");
        transform.GetChild(4).GetComponent<Animator>().SetTrigger("Dance");
        yield return new WaitForSeconds(590/30f);
        StartCoroutine(UsingLerp(transform.GetChild(0), transform.GetChild(0).localPosition, transform.GetChild(0).localEulerAngles + new Vector3(0, 180, 0), 0.1f));
        StartCoroutine(UsingLerp(transform.GetChild(1), transform.GetChild(1).localPosition, transform.GetChild(1).localEulerAngles + new Vector3(0, 180, 0), 0.1f));
        StartCoroutine(UsingLerp(transform.GetChild(2), transform.GetChild(2).localPosition, transform.GetChild(2).localEulerAngles + new Vector3(0, 180, 0), 0.1f));
        StartCoroutine(UsingLerp(transform.GetChild(3), transform.GetChild(3).localPosition, transform.GetChild(3).localEulerAngles + new Vector3(0, 180, 0), 0.1f));
        StartCoroutine(UsingLerp(transform.GetChild(4), transform.GetChild(4).localPosition, transform.GetChild(4).localEulerAngles + new Vector3(0, 180, 0), 0.1f));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(UsingLerp(transform.GetChild(0), transform.GetChild(0).localPosition + new Vector3(0, 0, -3), transform.GetChild(0).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(1), transform.GetChild(1).localPosition + new Vector3(0, 0, -6), transform.GetChild(1).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(2), transform.GetChild(2).localPosition + new Vector3(0, 0, -9), transform.GetChild(2).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(3), transform.GetChild(3).localPosition + new Vector3(0, 0, -6), transform.GetChild(3).localEulerAngles, 0.7f));
        StartCoroutine(UsingLerp(transform.GetChild(4), transform.GetChild(4).localPosition + new Vector3(0, 0, -3), transform.GetChild(4).localEulerAngles, 0.7f));
        yield return new WaitForSeconds(1.5f);
        transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
        transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 0);
        transform.GetChild(2).localEulerAngles = new Vector3(0, 0, 0);
        transform.GetChild(3).localEulerAngles = new Vector3(0, 0, 0);
        transform.GetChild(4).localEulerAngles = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }

    IEnumerator UsingLerp(Transform obj, Vector3 destPos, Vector3 destRot, float destTime)
    {
    WaitForSeconds wtf = new WaitForSeconds(Time.deltaTime / 2);
    float pastTime = 0f;
    Vector3 originPos = obj.localPosition;
    Vector3 originRot = obj.localEulerAngles;
    Vector3 originScale = obj.localScale;
    while (pastTime < destTime)
    {
        obj.localPosition = Vector3.Lerp(originPos, destPos, pastTime / destTime);
        obj.localEulerAngles = Vector3.Lerp(originRot, destRot, pastTime / destTime);
        pastTime += Time.deltaTime / 2;
        yield return wtf;
    }
    obj.localPosition = destPos;
    obj.localEulerAngles = destRot;
    }
}
