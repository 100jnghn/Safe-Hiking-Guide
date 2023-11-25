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

    public AudioSource sound; // 헬리콥터 소리

    public float rotateSpeed = 300f; // 프로펠러 회전 속도

    void Start()
    {
        setStartPos(); // 시작 위치로 세팅
        sound.Play(); // 소리 재생
    }

    void Update()
    {
        moveHellicopter(); // 목표 위치로 이동
        spinPropeller(); // 프로펠러 회전
    }

    // 헬기를 출발 위치로 이동시킴
    public void setStartPos()
    {
        transform.position = startPos.position; // 위치 설정
        transform.rotation = startPos.rotation; // rotation 설정
    }

    // 프로펠러 회전
    public void spinPropeller()
    {
        propeller.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed); // 프로펠러 회전
        propellerBack.transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed); // 뒤쪽 프로펠러 회전
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
