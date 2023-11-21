using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRSituation : MonoBehaviour
{
    public float patientMoveSpeed = 1f; // 환자가 앞으로 갈 때의 속도

    public GameObject player; // 플레이어(CPR을 하는 사람)
    public GameObject patient; // 환자(쓰러져서 CPR 받는 사람)

    public bool isPatientDown; // 환자가 쓰러진 상황인가요?

    private float time = 0f;

    void Start()
    {
        
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


        while(time <= 2f) // 2초동안 실행 (환자가 앞으로 갑니다)
        {
            moveForwardPatient();

            time += Time.deltaTime;
            //Debug.Log(time.ToString()); ;
            yield return null;
        }
        time = 0f;


        isPatientDown = true;
        // 여기에 환자가 쓰러지는 애니메이션 추가


        yield return null;
    }

    // 환자를 앞으로 이동시키는 함수
    private void moveForwardPatient()
    {
        patient.transform.Translate(Vector3.forward * patientMoveSpeed * Time.deltaTime); // 환자 앞으로 이동
    }
}
