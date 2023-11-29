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
    public bool isTakeOff; //손상 부위를 확인하기 위해 환부의 옷 제거
    public bool didTakeOff; //환부의 옷 제거수행완료
    public bool isPress; // 지혈 수행해야하는 상황
    public bool didPress; // 지혈 수행완료
    public bool isSplint; // 부목 수행해야하는 상황
    public bool didPickUp; // 부목 줍기
    public bool didSplint; //부목 수행완료
    public bool isIcing; //냉찜질 수행해야하는 상황
    public bool didIcing; //냉찜질 수행완료



    public bool finishFracture; // Fracture 상황 종료

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

    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("Fracture Situation Start");

        yield return new WaitForSeconds(2f); // 2초 대기 후 시작


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
    private void FixPatientPosition()
    {
        patient.transform.position = patientFallPosition; // 쓰러진 위치로 환자 이동
        patient.transform.rotation = patientFallRotation; // 쓰러진 때의 회전 값으로 환자 회전
    }
}





/*




// CPR 상황에서 UI 관리
public void setUICPR()
{
    // 환자가 쓰러졌다면
    if (cpr.isPatientDown && !cpr.isPatientCons)
    {
        situationMainTextPanel.SetActive(true);
        uiStr = "환자가 발생했습니다!\n가까이 다가가 상태를 파악해 주세요.";
        setText(situationMainText, uiStr);
    }

    // 환자의 의식을 파악해야 하는 상황
    if (cpr.isPatientCons && !cpr.isHelpOther)
    {
        uiStr = "어깨를 두드리며, \"괜찮으세요?\"라고 물어보고\n의식과 호흡 여부를 파악해 주세요.";
        setText(situationMainText, uiStr);
    }

    // 의식을 파악해야 하는 상황에서 R키 액션을 취함
    if (cpr.isPatientCons && !cpr.didPatientCons && !cpr.isHelpOther && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
    {
        uiStr = "\"괜찮으세요?\"";
        situationMainText.color = Color.yellow;
        setText(situationMainText, uiStr);

        cpr.didPatientCons = true; // 환자 의식 파악 완료함
    }

    // 의식 파악까지 완료한 상황 (자막 유지)
    if (cpr.didPatientCons && !cpr.isHelpOther)
    {
        uiStr = "\"괜찮으세요?\"";
        situationMainText.color = Color.yellow;
        setText(situationMainText, uiStr);

        // 3초 대기 후 실행할 내용
        StartCoroutine(DelayedAction(3f, () =>
        {
            cpr.isHelpOther = true; // 다른 사람들에게 도움을 요청해야함
        }));

    }

    // 다른 사람에게 도움을 요청해야 하는 상황
    if (cpr.isHelpOther && !cpr.didCall119 && !cpr.didCallAED)
    {
        uiStr = "환자가 의식이 없습니다.\n주변 사람들에게 도움을 요청하고 CPR을 수행하세요.";
        situationMainText.color = Color.white;
        setText(situationMainText, uiStr);
    }

    // 119 불러달라고 해야 하는 순서
    if (cpr.isHelpOther && !cpr.didCall119 && !cpr.didCallAED && playerScript.rayCollObject.name == "For CPR 119" && playerScript.doAction())
    {
        uiStr = "~~~이신 분 119에 구조 요청 부탁드립니다.";
        situationMainText.color = Color.yellow;
        setText(situationMainText, uiStr);
        cpr.didCall119 = true;
    }

    // 119 불렀으면 자막 유지
    if (cpr.isHelpOther && cpr.didCall119 && !cpr.didCallAED)
    {
        setText(situationMainText, uiStr);
    }

    // AED 요청해야 하는 순서
    if (cpr.isHelpOther && cpr.didCall119 && !cpr.didCallAED && playerScript.rayCollObject.name == "For CPR AED" && playerScript.doAction())
    {
        uiStr = "~~~이신 분 AED를 가져다 주세요.";
        situationMainText.color = Color.yellow;
        setText(situationMainText, uiStr);
        cpr.didCallAED = true;
    }

    // AED 요청했으면 자막 유지
    if (cpr.isHelpOther && cpr.didCall119 && cpr.didCallAED)
    {
        setText(situationMainText, uiStr);

        // 3초 대기 후 실행할 내용
        StartCoroutine(DelayedAction(3f, () =>
        {
            cpr.isChestPress = true; // 다른 사람들에게 도움을 요청해야함
        }));
    }

    // 가슴 압박 수행해야 하는 순서
    if (cpr.isChestPress && !cpr.didChestPress)
    {
        uiStr = "이미지를 참고하여 정확한 자세로 가슴 압박을 수행하세요.";
        situationMainText.color = Color.white;
        setText(situationMainText, uiStr);

        // ----- 이미지 띄우는 코드 작성 ----- //
        chestPressHintImg.SetActive(true);
    }

    // 가슴 압박을 수행
    if (cpr.isChestPress && !cpr.didChestPress && playerScript.rayCollObject.name == "Patient CPR" && playerScript.doAction())
    {
        uiStr = "가슴 압박은 분당 100~120회로 강하고 규칙적이게 수행해야 합니다.";
        setText(situationMainText, uiStr);

        cpr.didChestPress = true;
    }

    // 가슴 압박 수행 완료
    if (cpr.isChestPress && cpr.didChestPress)
    {
        // ----- 이미지 다시 안 보이게 ----- //
        chestPressHintImg.SetActive(false);

        uiStr = "가슴 압박은 분당 100~120회로 강하고 규칙적이게 수행해야 합니다.";
        setText(situationMainText, uiStr);

        // 3초 대기 후 실행할 내용
        StartCoroutine(DelayedAction(3f, () =>
        {
            cpr.isArtificialRes = true; // 인공호흡을 수행해야 함
        }));
    }

    // 인공호흡을 수행해야 하는 순서
    if (cpr.isArtificialRes && !cpr.didArtificialRes)
    {
        uiStr = "환자의 머리를 젖혀 기도를 확보하고\n 이미지를 참고하여 정확한 자세로 인공호흡을 수행하세요";
        setText(situationMainText, uiStr);

        // ----- 이미지를 띄우는 코드 작성 ----- //
        artificialResHintImg.SetActive(true);
    }

    // 인공호흡을 수행
    if (cpr.isArtificialRes && !cpr.didArtificialRes && playerScript.rayCollObject.name == "Patient CPR" && playerScript.doAction())
    {
        uiStr = "30회 가슴압박, 2회 인공호흡을 119가 도착할 때까지 반복 시행해 주세요";
        setText(situationMainText, uiStr);

        cpr.didArtificialRes = true;
    }

    // 인공호흡 수행 완료
    if (cpr.isArtificialRes && cpr.didArtificialRes)
    {
        // ----- 이미지 다시 안 보이게 ----- //
        artificialResHintImg.SetActive(false);

        uiStr = "30회 가슴압박, 2회 인공호흡을 119가 도착할 때까지 반복 시행해 주세요";
        setText(situationMainText, uiStr);

        // 3초 대기 후 실행할 내용
        StartCoroutine(DelayedAction(3f, () =>
        {
            cpr.finishCPR = true; // CPR 상황 끝
        }));
    }

    // CPR 상황 끝!
    if (cpr.finishCPR)
    {
        uiStr = "119 구조대가 도착합니다.";
        setText(situationMainText, uiStr);

        finishSituation();
    }

}
*/