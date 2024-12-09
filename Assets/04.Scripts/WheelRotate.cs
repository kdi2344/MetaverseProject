using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    [SerializeField] float RotateSpeed = 0.1f;
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * RotateSpeed);
    }
}
