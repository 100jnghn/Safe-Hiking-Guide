using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startSceneUIPanel; // 시작 화면 UI
    public GameObject situationUIPanel; // 상황들의 UI
    public GameObject situationMainTextPanel; // 자막 패널
    
    public Text situationMainText; // 각 상황에서의 메인 자막 text

    public GameObject chestPressHintImg; // 가슴 압박 힌트 이미지
    public GameObject artificialResHintImg; // 인공호흡 힌트 이미지

    public GameObject startSceneCam; // 시작 화면 카메라
    public GameObject playerCam; // 플레이어 시점의 카메라
    public GameObject player; // 플레이어 객체
    private Player playerScript; // 플레이어 스크립트

    public GameObject hellicopter; // 헬리콥터 오브젝트

    public GameObject cprSituation; // CPR 상황을 관리 (Start Position CPR)
    private CPRSituation cpr; // cprSituation 오브젝트의 Component인 CPRSituation 스크립트를 가져옴

    public GameObject fractureSituation;// Fracture 상황을 관리 (Start Position Fracture)
    private FractureSituation fracture;// fractureSituation 오브젝트의 Component인 FractureSituation 스크립트를 가져옴

    private string uiStr; // 자막에 들어갈 내용

    public enum Mode { Nothing, CPR, Fracture, Snake, Bee };
    public Mode mode; // 현재 어떤 모드인지 저장

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
        fracture = fractureSituation.GetComponent<FractureSituation>();
        playerScript = player.GetComponent<Player>();

        mode = Mode.Nothing; // 시작은 아무런 모드가 아닌 상태
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

    // 상황 끝
    public void finishSituation()
    {
        moveHellicopter(); // 엔딩 : 헬기 이동

        // 모드 초기화
        // 각 상황들 변수 초기화
        // 각 상황들 오브젝트 위치 초기화
        // 헬리콥터 위치 초기화 + SetActive(false)
        // UI 세팅
        // 카메라 세팅
    }

    // 헬리콥터 이동시키는 함수
    public void moveHellicopter()
    {
        hellicopter.SetActive(true); // 헬리콥터 활성화
        // 현재 모드에 따라 해당 위치로 헬기 이동
        
    }

    // 시작 메뉴 화면에서 심폐소생술 상황 클릭
    public void startCPR()
    {
        mode = Mode.CPR; // 현재 모드를 CPR 모드로

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
        mode = Mode.Fracture; // (추가됨 :) 현재 모드를 골절 모드로

        player.transform.position = fractureSituation.transform.position;// 시작 위치 설정
        startSituation();
        fracture.StartCoroutine("startSituation");

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
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "골절이 의심됩니다.\n 함부로 눌러보거나 꺾어보지 마세요.";
            setText(situationMainText, uiStr);
        }

        //환자 다리 골절 확인
        if (fracture.isPatientCons && !fracture.isHelpOther)
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
