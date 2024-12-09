using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPlayer : MonoBehaviour
{
    [SerializeField] private Transform characterBody;
    [SerializeField] private Transform cameraArm;
    Animator animator;

    [SerializeField] float moveSpeed = 2f;

    void Start()
    {
           animator = characterBody.GetComponent<Animator>();
    }

    void Update()
    {
        LookAround();  
        Move(); 
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("isWalk", isMove);
        if (isMove){
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if ( x < 180f) x = Mathf.Clamp(x, -1f, 70f);
        else x = Mathf.Clamp(x, 335f, 361f);

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

    }
}
