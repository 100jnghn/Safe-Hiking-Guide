using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startSceneUIPanel; // 시작 화면 UI
    public GameObject situationUIPanel; // 상황들의 UI
    public Text situationMainText; // 각 상황에서의 메인 자막 text
    public GameObject situationMainTextPanel; // 자막 패널

    public GameObject startSceneCam; // 시작 화면 카메라
    public GameObject playerCam; // 플레이어 시점의 카메라
    public GameObject player; // 플레이어 객체
    private Player playerScript; // 플레이어 스크립트

    public GameObject cprSituation; // CPR 상황을 관리 (Start Position CPR)
    private CPRSituation cpr; // cprSituation 오브젝트의 Component인 CPRSituation 스크립트를 가져옴

    private float waitTimer = 0f; // 액션 후 일정 시간을 기다리기 위해 사용

    private string uiStr; // 자막에 들어갈 내용

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
        //Fracture = FractureStituation.
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        setUICPR(); // CPR 상황에서의 UI를 관리
    }

    // 상황 시작
    public void startSituation()
    {
        startSceneUIPanel.SetActive(false); // 시작 화면 UI 끄기
        startSceneCam.SetActive(false); // 시작 화면 카메라 끄기
        player.SetActive(true); // 플레이어 활성화
        playerCam.SetActive(true); // 플레이어 시점으로 전환

        situationUIPanel.SetActive(true); // Situation의 UI 켜기
    }

    // 시작 메뉴 화면에서 심폐소생술 상황 클릭
    public void startCPR()
    {
        player.transform.position = cprSituation.transform.position; // 시작 위치 설정
        startSituation();
        cpr.StartCoroutine("startSituation"); // CPR 상황 시작
    }

    // 시작 메뉴 화면에서 뱀에 물리는 상황 클릭
    public void startSnake()
    {
        startSituation();

    }

    // 시작 메뉴 화면에서 골절 상황 클릭
    public void startFracture()
    {
        //player.transform.position = 
        startSituation();

    }

    // 시작 메뉴 화면에서 벌에 쏘이는 상황 클릭
    public void startBee()
    {
        startSituation();

    }

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
            uiStr = "어깨를 두드리며, \"괜찮으세요?\"라고 물어보고\n의식을 파악해 주세요.";
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
        if (cpr.didPatientCons)
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
        if (cpr.isHelpOther)
        {
            uiStr = "대충 도움 요청해야 한다는 내용";
            situationMainText.color = Color.white;
            setText(situationMainText, uiStr);
        }

    }

    // delay만큼 대기하는 코루틴
    private IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    // Text의 내용을 변경
    private void setText(Text text, string str)
    {
        text.text = str;
    }

}
