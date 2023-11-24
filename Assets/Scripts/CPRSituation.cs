using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRSituation : MonoBehaviour
{
    public float patientMoveSpeed = 1f; // 환자가 앞으로 갈 때의 속도

    public GameObject player; // 플레이어(CPR을 하는 사람)
    public GameObject patient; // 환자(쓰러져서 CPR 받는 사람)

    public bool isPatientDown;  // 환자가 쓰러진 상황인가요?
    public bool isPatientCons;  // 환자 의식을 파악해야 하나요?
    public bool didPatientCons; // 환자 의식을 파악 완료했는지?
    public bool isHelpOther;    // 다른 사람에게 도움을 요청해야 하는 상황?
    public bool didCall119; // 다른 사람에게 119 불러달라고 요청했는지?
    public bool didCallAED; // 다른 사람에게 AED 갖다달라고 요청했는지?
    public bool isChestPress; // 가슴 압박을 해야 하는 순서?

    private Animator patientAnimator; //환자 Animator 컴포넌트


    private float time = 0f;

    void Start()
    {
        patientAnimator = patient.GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    // 상황 시작 // 여러 함수 사용과 대기시간을 위해 코루틴 사용(Test)
    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("CPR Situation Start");

        yield return new WaitForSeconds(1f); // 1초 대기 후 시작


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

        // 여기에 환자가 쓰러지는 애니메이션 추가
        patientAnimator.SetBool("isFalling", true);
        

        yield return null;
        yield break;
    }





    private void patientGetUp()
    {
        patientAnimator.SetBool("isFalling", false);
        patientAnimator.SetBool("getUp", true);
    }

    //일정 시간 이후 다시 idle상태로 갈 때.(getUp 후 회복하는 상황일 경우)
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
