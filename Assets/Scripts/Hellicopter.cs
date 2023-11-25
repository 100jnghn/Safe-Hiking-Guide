using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hellicopter : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject propeller; // 헬리콥터의 프로펠러
    public GameObject propellerBack; // 헬리콥터 뒤쪽 프로펠러

    public Transform startPos; // 헬리콥터 시작 위치
    public Transform destinationCPR; // CPR 상황 도착 위치

    public float rotateSpeed = 300f; // 프로펠러 회전 속도

    public bool canMove; // true가 되면 헬기가 이동합니다.

    void Start()
    {
        setStartPos();
    }

    void Update()
    {
        moveHellicopter();
        spinPropeller();
    }

    // 헬기를 출발 위치로 이동시킴
    public void setStartPos()
    {
        transform.position = startPos.position; // 위치 설정
        transform.rotation = startPos.rotation; // rotation 설정
    }

    // 프로펠러 회전 + 헬기 소리
    public void spinPropeller()
    {
        propeller.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        propellerBack.transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
    }

    // 헬리콥터를 위치로 이동시킴
    public void moveHellicopter()
    {
        switch (gameManager.mode)
        {
            case GameManager.Mode.Nothing:
                break;

            case GameManager.Mode.CPR:
                transform.position = Vector3.Lerp(transform.position, destinationCPR.position, 0.003f);
                break;
        }
        
    }
}
