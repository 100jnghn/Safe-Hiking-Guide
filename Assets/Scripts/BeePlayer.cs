using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BeePlayer : MonoBehaviour
{
    public GameObject camera;

    public BeeSituation beeSituation;

    public float camRotateSpeed = 300f; // 마우스 이동에 의한 카메라 회전 속도
    public float inputTimer = 0f; // Q키 입력 시간을 저장 // 행동에 대한 액션 판별 용도
    private float xRotate, yRotate, xRotateMove, yRotateMove; // 마우스에 의한 카메라 회전에 관련된 변수들

    void Start()
    {
        
    }

    void Update()
    {
        doRotateCamera();
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

    // ! 키를 눌러 액션 (3초 이상 누르면 true)
    public bool doAction()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.Q))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q)) // R 키에서 떼면 inputTimer 초기화
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
    }
}
