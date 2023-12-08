using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSituation : MonoBehaviour
{

    public float patientMoveSpeed = 1f; // 환자가 앞으로 갈 때의 속도

    public GameObject player; // 플레이어
    public GameObject patient; // 환자

    public Transform playerStartPos; // 플레이어 시작 위치
    public Transform patientStartPos; // 환자 시작 위치
    public bool isPatientDown;  // 환자가 쓰러진 상황인가요?
    public bool isPatientCons;  // 환자 의식을 파악해야 하나요?
    public bool didPatientCons; // 환자 의식을 파악 완료했는지?
    public bool isCall119;
    public bool didCall119; // 다른 사람에게 119 불러달라고 요청했는지?
    public bool isCalmDown;//환자 안정시키기
    public bool didCalmDown;
    public bool isRemove;//순환을 방해하는 것 제거
    public bool didRemove;
    public bool isDown;//물린 부위 심장보다 아래로
    public bool didDown;
    public bool isTie;//붓는 경우 묶기
    public bool didTie;

    public bool finishSnake; // 상황 종료

    private Animator patientAnimator; //환자 Animator 컴포넌트
    public CapsuleCollider capsuleCollider; // 캡슐 콜라이더 컴포넌트
    private Vector3 patientFallPosition;// 환자 쓰러진 위치
    private Quaternion patientFallRotation; //환자 쓰러진 방향
    public bool isFixed = false;//환자 고정


    private float time = 0f;

    void Start()
    {
        patientAnimator = patient.GetComponent<Animator>();
        capsuleCollider = patient.GetComponent<CapsuleCollider>();
        
    }

    void Update()
    {
        if (isFixed)
        {
            FixPatientPosition(); // 환자가 고정되어 있을 때 위치 및 회전 고정
        }
    }


    // 상황 시작 // 여러 함수 사용과 대기시간을 위해 코루틴 사용(Test)
    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("Snake Situation Start");

        yield return new WaitForSeconds(2f); // 2초 대기 후 시작


        // 여기에 환자가 걷는 애니메이션 추가
        patientAnimator.SetBool("isWalking", true);



        while (time <= 3f) // 3초동안 실행 (환자가 앞으로 갑니다)
        {
            moveForwardPatient();

            time += Time.deltaTime;
            //Debug.Log(time.ToString()); ;
            yield return null;
        }
        time = 0f;

        isPatientDown = true;

        //환자 쓰러지는 애니메이션
        patientFalling();
        yield return new WaitForSeconds(0.5f);
        patientFallPosition = patient.transform.position;
        patientFallRotation = patient.transform.rotation;

        isFixed = true;

        yield return null;
        yield break;
    }

    private void patientFalling()
    {
        patientAnimator.SetBool("isFalling", true);
        capsuleCollider.direction = 2; //콜라이더의 방향 Z축으로 설정 (0: X축, 1: Y축, 2: Z축)
        capsuleCollider.center = new Vector3(0f, 0.3f, 0f); //중심 위치 조정
        capsuleCollider.radius = 0.3f; //radius 설정
    }

    private void patientGetUp()//환자 일어날 때
    {
        patientAnimator.SetBool("isFalling", false);
        patientAnimator.SetBool("getUp", true);
        capsuleCollider.direction = 1; // 기본 방향 (Y축)
        capsuleCollider.center = new Vector3(0f, 0.9f, 0f); // 기본 중심 위치로
        capsuleCollider.height = 0.9f; // 기본 높이
    }

    //일정 시간 이후 다시 idle상태로 갈 때(getUp 후 회복하는 상황일 경우)
    private void patientIdle()
    {
        patientAnimator.SetBool("getUp", false);
        patientAnimator.SetBool("idle", true);
    }


    // 환자를 앞으로 이동시키는 함수
    private void moveForwardPatient()
    {
        patient.transform.Translate(Vector3.forward * patientMoveSpeed * Time.deltaTime); // 환자 앞으로 이동
    }


    private void FixPatientPosition()
    {
        patient.transform.position = patientFallPosition; // 쓰러진 위치로 환자 이동
        patient.transform.rotation = patientFallRotation; // 쓰러진 때의 회전 값으로 환자 회전
    }



}
