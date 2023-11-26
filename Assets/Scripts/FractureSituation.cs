using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureSituation : MonoBehaviour
{
    public float patientMoveSpeed = 1f; // 환자가 앞으로 갈 때의 속도

    public GameObject player; // 플레이어
    public GameObject patient; // 환자

    public bool isPatientDown;  // 환자가 쓰러진 상황인가요?
    public bool isPatientCons;  // 환자 의식을 파악해야 하나요?
    public bool isHelpOther;    // 다른 사람에게 도움을 요청해야 하는 상황?
    public bool didCall119; // 다른 사람에게 119 불러달라고 요청했는지?


    public bool finishFracture; // Fracture 상황 종료

    private Animator patientAnimator; //환자 Animator 컴포넌트
    public CapsuleCollider capsuleCollider; // 캡슐 콜라이더 컴포넌트


    private float time = 0f;

    void Start()
    {
        patientAnimator = patient.GetComponent<Animator>();
        capsuleCollider = patient.GetComponent<CapsuleCollider>();
    }


    void Update()
    {

    }

    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("Fracture Situation Start");

        yield return new WaitForSeconds(1f); // 1초 대기 후 시작


        //환자 걷는 애니메이션
        patientAnimator.SetBool("isWalking", true);


        while (time <= 3f) // 3초동안 실행 (환자가 앞으로 갑니다)
        {
            moveForwardPatient();

            time += Time.deltaTime;
            //Debug.Log(time.ToString());
            yield return null;
        }
        time = 0f;

        isPatientDown = true;


        //환자 쓰러지는 애니메이션
        patientFalling();


        yield return null;
        yield break;
    }





    private void patientFalling()
    {
        patientAnimator.SetBool("isFalling", true);
        capsuleCollider.direction = 2; //콜라이더의 방향 Z축으로 설정 (0: X축, 1: Y축, 2: Z축)
        capsuleCollider.center = new Vector3(0f, 0.2f, 0f); //중심 위치 조정
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
}





/*
    //Fracture UI
    public void setUIFracture()
    {
        // 환자가 쓰러졌다면
        if (fracture.isPatientDown && !fracture.isPatientCons)
        {
            situationMainTextPanel.SetActive(true);
            uiStr = "환자가 발생했습니다!\n가까이 다가가 상태를 파악해 주세요.";
            setText(situationMainText, uiStr);
        }

        //골절이 의심된 경우에는 항상 골절로 간주하고 처치. 함부로 눌러보거나 꺾어 보지 않는다.
        if (fracture.isPatientCons && !fracture.didCall119)
        {
            uiStr = "골절이 의심됩니다.\n함부로 눌러보거나 꺾어보지 마세요.\n주변 사람에게 119에 신고해 달라고 해주세요.";
            setText(situationMainText, uiStr);
        }

        // 119 신고 요청
        if (fracture.isHelpOther && !fracture.didCall119 && !cpr.didCallAED && playerScript.rayCollObject.name == "For Fracture 119" && playerScript.doAction())
        {
            uiStr = "~~~이신 분 119에 구조 요청 부탁드립니다.";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);
            cpr.didCall119 = true;
        }

            //골절이 의심된 경우에는 항상 골절로 간주하고 처치. 함부로 눌러보거나 꺾어 보지 않는다.
        if (fracture.isPatientCons && !fracture.didCall119)
        {
            uiStr = "골절이 의심됩니다.\n 함부로 눌러보거나 꺾어보지 마세요.\n주변 사람에게 119에 신고해 달라고 해주세요.(R키를 눌러 시행합니다.)";
            setText(situationMainText, uiStr);
        }



        //환자 다리 골절 확인
        if (fracture.isPatientCons && !fracture.didCall119)
        {
            uiStr = "환자가 다리를 움직이지 못합니다.\n 자세히 확인하기위해 \"C버튼\"을 눌러서 앉아주세요";
            setText(situationMainText, uiStr);
        }

        //손상 부위를 확인하기 위해 환부의 옷 제거
        if (fracture.isPatientCons && !cpr.didPatientCons && !fracture.isHelpOther && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
            uiStr = "환자의 옷을 제거하기위해 \"R버튼\"을 3초동안 눌러주세요";
            setText(situationMainText, uiStr);
        }

        //출혈 시 지혈
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "다리에 출혈이 있습니다.\n 지혈을 하기위해 \"P버튼\"을 3초동안 눌러주세요";
            setText(situationMainText, uiStr);
        }


        //부목 고정
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "우측에서 부목을 주워와 다리를 움직이지 않게 고정해주세요.\n 부목에 가까이 다가가면 습득할 수 있습니다.\n 부목을 대는 방법은 이미지UI를 참고해주세요.";
            setText(situationMainText, uiStr);
        }

        //냉찜질
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "붓기와 통증을 줄이기위해 냉찜질이 필요합니다.\n \"I버튼\"을 3초동안 눌러주세요";
            setText(situationMainText, uiStr);
        }
    
    }
    */