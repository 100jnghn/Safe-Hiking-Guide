using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1.3f; // 이동 속도
    public float jumpPower = 3.5f; // 점프할 때 리지드바디에 가해지는 힘
    public float camRotateSpeed = 300f; // 마우스 이동에 의한 카메라 회전 속도

    public bool canMove; // 플레이어가 움직일 수 있는 상태인가요?

    private Rigidbody rigidbody;

    public GameObject camera; // 플레이어 시점의 카메라
    public GameObject cprSituation; // CPR 상황을 관리 (Start Position CPR)
    private CPRSituation cpr; // cprSituation 오브젝트의 Component인 CPRSituation 스크립트를 가져옴

    private float xRotate, yRotate, xRotateMove, yRotateMove; // 마우스에 의한 카메라 회전에 관련된 변수들

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cpr = cprSituation.GetComponent<CPRSituation>();
    }

    void Update()
    {
        doMove(); // 플레이어 이동
        doRotateCamera(); // 마우스로 카메라 회전
    }

    // 플레이어 이동 함수
    void doMove()
    {
        if (canMove)
        {
            Vector3 dir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) // W -> 앞으로 이동
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                dir += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S)) // S -> 뒤로 이동
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
                dir += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A)) // A -> 왼쪽으로 이동
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                dir += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D)) // D -> 오른쪽으로 이동
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                dir += Vector3.right;
            }
            if (Input.GetKeyDown(KeyCode.Space)) // Space -> 점프
            {
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }    
    }

    // 카메라 회전하는 함수 
    // 마우스 상하 이동 -> 카메라만 회전 // 마우스 좌우 이동 -> 카메라, 플레이어 회전
    void doRotateCamera()
    {
        xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * camRotateSpeed; // 마우스 상하 이동
        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * camRotateSpeed; // 마우스 좌우 이동

        xRotate = camera.transform.eulerAngles.x + xRotateMove; // 상하 회전
        //xRotate = Mathf.Clamp(xRotate, -90, 90); // 위, 아래 각도를 제한

        yRotate = transform.eulerAngles.y + yRotateMove; // 좌우 회전

        camera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0); // 카메라 회전
        transform.eulerAngles = new Vector3(0, yRotate, 0); // 플레이어 회전
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 환자와의 충돌 발생
        if (collision.gameObject.tag == "Patient")
        {
            if (cpr.isPatientDown & !cpr.isPatientCons) // 환자가 쓰러진 상태
            {
                cpr.isPatientCons = true; // 환자의 의식을 확인해야 하는 상태 On
            }
        }
    }
}
