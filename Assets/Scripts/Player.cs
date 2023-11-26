using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1.3f; // 이동 속도
    public float jumpPower = 3.5f; // 점프할 때 리지드바디에 가해지는 힘
    public float camRotateSpeed = 300f; // 마우스 이동에 의한 카메라 회전 속도
    public float inputTimer = 0f; // R키 입력 시간을 저장 // 행동에 대한 액션 판별 용도

    public bool canMove; // 플레이어가 움직일 수 있는 상태인가요?
    public bool isSit; // 플레이어가 앉아있나요?

    private Rigidbody rigidbody;

    public Camera mainCamera;
    private Ray ray;
    private RaycastHit hit; // Raycast 충돌 감지
    GameObject gazeTimerObject; // 물체를 가리키고 있는지 확인하는 변수
    float gazeTimer = 0f; // 가리키고 있는 시간을 측정하는 변수

    public GameObject camera; // 플레이어 시점의 카메라
    public GameObject cprSituation; // CPR 상황을 관리 (Start Position CPR)
    public GameObject fractureSituation;// Fracture 상황을 관리 (Start Position Fracture)
    public GameObject rayPosition; // ray를 발사할 위치
    public GameObject rayCollObject; // ray와 충돌한 오브젝트
    public GameObject standPosition; // 플레이어가 서있을 때 시점이 되는 위치
    public GameObject sitPosition; // 플레이어가 앉았을 때 시점이 되는 위치
    private CPRSituation cpr; // cprSituation 오브젝트의 Component인 CPRSituation 스크립트를 가져옴
    private FractureSituation fracture;// fractureSituation 오브젝트의 Component인 FractureSituation 스크립트를 가져옴

    private float xRotate, yRotate, xRotateMove, yRotateMove; // 마우스에 의한 카메라 회전에 관련된 변수들
    public float rayDistance = 15f; // Ray의 길이

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cpr = cprSituation.GetComponent<CPRSituation>();
        fracture = fractureSituation.GetComponent<FractureSituation>();

        Cursor.lockState = CursorLockMode.Locked; // 커서 화면 중앙에 고정
        Cursor.visible = false; // 커서가 보이지 않게 함
    }

    void Update()
    {
        doMove(); // 플레이어 이동
        doRotateCamera(); // 마우스로 카메라 회전
        doAction(); // 액션 키 입력(R키)
        shotRay(); // 레이를 쏘아주는 함수
        rayCollider();
        doSit(); // 플레이어가 앉는 기능 (C키 입력)
        doPress();
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

    // 플레이어가 앉는 기능
    void doSit()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isSit)
            {
                camera.transform.position = sitPosition.transform.position;
                isSit = true;
            }
            else
            {
                camera.transform.position = standPosition.transform.position;
                isSit = false;
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

    void shotRay()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawLine(ray.origin, hit.point, Color.red);
    }

    // ray의 충돌을 처리하는 함수
    void rayCollider()
    {
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            rayCollObject = hit.transform.gameObject; // rayCollObject를 ray가 충돌한 오브젝트로 선언
        }
    }

    // R 키를 눌러 액션 (3초 이상 누르면 true)
    public bool doAction()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.R))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action =  true;
            }
        }
        if (Input.GetKeyUp(KeyCode.R)) // R 키에서 떼면 inputTimer 초기화
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
    }

    // P 키를 눌러 지혈 (3초 이상 누르면 true)
    public bool doPress()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.P))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.P)) // P 키에서 떼면 inputTimer 초기화
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
    }


    //냉찜질하기
    public bool doIcing()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.I))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.I)) // I 키에서 떼면 inputTimer 초기화
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
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
            if (fracture.isPatientDown & !fracture.isPatientCons) // 환자가 쓰러진 상태
            {
                fracture.isPatientCons = true; // 구조요청을 해야하는 상태 On
            }
        }
    }
}
