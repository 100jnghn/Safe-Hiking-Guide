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
    public GameObject pressHintImg; // 지혈 힌트 이미지
    public GameObject splintHintImg;// 부목 대기 힌트 이미지
    public GameObject tieHintImg;//뱀물림 묶기 힌트 이미지
    public GameObject cardHintImg; // 카드 벌침 제거 힌트 이미지
    public GameObject icingHingImg; // 냉찜질 힌트 이미지

    public GameObject startSceneCam; // 시작 화면 카메라
    public GameObject playerCam; // 플레이어 시점의 카메라
    public GameObject player; // 플레이어 객체
    private Player playerScript; // 플레이어 스크립트

    public GameObject beePlayer; // 벌 플레이어 객체
    private BeePlayer beePlayerScript; // 스크립트

    public GameObject hellicopter; // 헬리콥터 오브젝트
    private Hellicopter hellicopterScript; // 헬리콥터 스크립트

    public GameObject cprSituation; // CPR 상황을 관리 (Start Position CPR)
    private CPRSituation cpr; // cprSituation 오브젝트의 Component인 CPRSituation 스크립트를 가져옴

    public GameObject fractureSituation;// Fracture 상황을 관리 (Start Position Fracture)
    private FractureSituation fracture;// fractureSituation 오브젝트의 Component인 FractureSituation 스크립트를 가져옴

    public GameObject beeSituation; // Bee 상황을 관리
    private BeeSituation bee; // beeSituation 스크립트

    public GameObject snakeSituation; // Snake 상황을 관리
    private SnakeSituation snake; // snakeSituation 스크립트

    private string uiStr; // 자막에 들어갈 내용

    public enum Mode { Nothing, CPR, Fracture, Snake, Bee };
    public Mode mode; // 현재 어떤 모드인지 저장

    private bool activeExitBtn; // esc 버튼을 눌러 메인 화면으로 갈 수 있는지 판단하는 변수

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
        fracture = fractureSituation.GetComponent<FractureSituation>();
        bee = beeSituation.GetComponent<BeeSituation>();
        snake = snakeSituation.GetComponent<SnakeSituation>();

        playerScript = player.GetComponent<Player>();
        beePlayerScript = beePlayer.GetComponent<BeePlayer>();
        hellicopterScript = hellicopter.GetComponent<Hellicopter>();

        mode = Mode.Nothing; // 시작은 아무런 모드가 아닌 상태
        splintHintImg.SetActive(false);
        pressHintImg.SetActive(false);

    }

    void Update()
    {
        setUICPR(); // CPR 상황에서의 UI를 관리
        setUIFracture(); // Fracture 상황에서의 UI를 관리
        setUISnake(); // Snake 상황에서의 UI를 관리
        setUIBee(); // Bee 상황에서 UI 관리

        if (activeExitBtn && Input.GetKeyDown(KeyCode.Escape)) { finishSituation(); }
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
        StopAllCoroutines(); // 실행중인 코루틴 모두 종료시킴


        // 모드 초기화
        mode = Mode.Nothing;

        // 각 상황들 변수 초기화
        // 각 상황들 오브젝트 위치 초기화
        cpr.Reset();
        fracture.Reset();


        // 헬리콥터 위치 초기화 + SetActive(false)
        hellicopterScript.Reset();

        // UI 세팅
        situationUIPanel.SetActive(false); // 상황들의 UI
        situationMainTextPanel.SetActive(false); // 자막 패널
        startSceneUIPanel.SetActive(true); // 시작 화면 UI

        // 카메라 세팅
        playerCam.SetActive(false); // 플레이어 시점의 카메라
        startSceneCam.SetActive(true); // 시작 화면 카메라

        Debug.Log("Finish Situations");
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
        mode = Mode.Snake; // (추가됨 :) 현재 모드를 뱀물림 모드로

        player.transform.position = snakeSituation.transform.position;// 시작 위치 설정
        startSituation();
        snake.StartCoroutine("startSituation");
        player.transform.rotation = snake.playerStartPos.rotation;
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
        mode = Mode.Bee;
        startSituation();
        playerCam.SetActive(false);
        bee.StartCoroutine("startSituation");
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
            uiStr = "어깨를 두드리며, \"괜찮으세요?\"라고 물어보고\n의식과 호흡 여부를 파악해 주세요.\n(R키를 눌러 액션)";
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
            uiStr = "환자가 의식이 없습니다.\n주변 사람들에게 도움을 요청하고 CPR을 수행하세요.\n(R키를 눌러 액션)";
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
            uiStr = "이미지를 참고하여 정확한 자세로 가슴 압박을 수행하세요.\n(R키를 눌러 액션)";
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
            uiStr = "환자의 머리를 젖혀 기도를 확보하고\n 이미지를 참고하여 정확한 자세로 인공호흡을 수행하세요\n(R키를 눌러 액션)";
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

            moveHellicopter();
            activeExitBtn = true; // esc 버튼 활성화
        }

    }





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
            uiStr = "골절이 의심됩니다.\n함부로 눌러보거나 꺾어보지 마세요.\n주변 사람에게 119에 신고해 달라고 해주세요.(R키를 눌러 시행합니다.)";
            setText(situationMainText, uiStr);
        }

        // 119 신고 요청
        //if (!fracture.didCall119 && playerScript.rayCollObject != null && playerScript.rayCollObject.name == "For Fracture 119" && playerScript.doAction())
        //{
        //    uiStr = "~~~이신 분 119에 구조 요청 부탁드립니다.";
        //    situationMainText.color = Color.yellow;
        //    setText(situationMainText, uiStr);
        //    fracture.didCall119 = true;

        //    // 2초 대기 후 실행할 내용
        //    StartCoroutine(DelayedAction(2f, () =>
        //    {
        //        fracture.isTakeOff = true; // 옷 제거해야함
        //    }));
        //}

        //손상 부위를 확인하기 위해 환부의 옷 제거
        if (fracture.didCall119&&fracture.isTakeOff && !fracture.isPress &&!fracture.didTakeOff)
        {
            situationMainText.color = Color.white;
            uiStr = "환자의 상태를 자세히 확인하기위해 \"R키\"을 3초동안 눌러 환자의 옷을 제거해주세요";
            setText(situationMainText, uiStr);
        }

        //환자 옷 제거 수행 완료
        if (fracture.didCall119&&fracture.isTakeOff && !fracture.isPress&& playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
                fracture.didTakeOff = true;
                
                Debug.Log("값: " + fracture.didTakeOff);
            
        }

        if (!fracture.isPress && fracture.didTakeOff)
        {
            uiStr = "(옷을 제거했습니다.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.isPress = true;
            }));
        }

        /////

        //출혈 시 지혈
        if (fracture.isPress && !fracture.didPress)
        {
            pressHintImg.SetActive(true);
            situationMainText.color = Color.white;
            uiStr = "다리에 출혈이 있습니다.\n 지혈을 하기위해 \"P키\"를 3초동안 눌러주세요";
            setText(situationMainText, uiStr);
        }

        //지혈 수행 완료
        if (fracture.isPress && !fracture.didPress && playerScript.rayCollObject.tag == "Patient" && playerScript.doPress())
        {
                fracture.didPress = true;
        }
        if (fracture.didPress && !fracture.isSplint)
        {
            uiStr = "(지혈했습니다.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.isSplint = true;
            }));
        }


        // 지혈 수행 완료 & 부목대기
        if (fracture.isSplint && !fracture.didSplint)
        {
            pressHintImg.SetActive(false);// 지혈이미지 안 보이게
            splintHintImg.SetActive(true);// 부목이미지로 대체

            situationMainText.color = Color.white;
            uiStr = "이미지UI를 참고하여 R키를 통해 부목을 대고, 다리가 움직이지 않게 고정해주세요.";
            setText(situationMainText, uiStr);
        }

        //부목대기 수행 완료
        if (fracture.isSplint && !fracture.didSplint && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
            splintHintImg.SetActive(false);// 부목이미지 안 보이게            
            fracture.didSplint = true;
        }
        if (fracture.didSplint && !fracture.isIcing)
        {
            uiStr = "(부목을 대었습니다.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.isIcing = true;
            }));
        }

        //냉찜질 수행
        if (fracture.isIcing && !fracture.didIcing && !fracture.finishFracture)
        {
            situationMainText.color = Color.white;
            uiStr = "붓기와 통증을 줄이기위해 냉찜질이 필요합니다.\n \"I버튼\"을 3초동안 눌러주세요";
            setText(situationMainText, uiStr);
        }


        //냉찜질 수행 완료
        if (fracture.didSplint && fracture.isIcing && playerScript.rayCollObject.tag == "Patient" && playerScript.doIcing())
        {
            fracture.didIcing = true;
        }
        if (fracture.didIcing)
        {
            uiStr = "(냉찜질을 했습니다.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.finishFracture = true;
            }));
        }

        // Fracture 상황 끝
        if (fracture.finishFracture)
        {
            situationMainText.color = Color.white;
            uiStr = "119 구조대가 도착합니다.";
            setText(situationMainText, uiStr);

            moveHellicopter();
            activeExitBtn = true; // esc 버튼 활성화
        }

    }






    // Snake 상황에서 UI 관리
    public void setUISnake()
    {
        // 환자가 쓰러졌다면
        if (snake.isPatientDown && !snake.isPatientCons)
        {
            situationMainTextPanel.SetActive(true);
            uiStr = "환자가 발생했습니다!\n가까이 다가가 상태를 파악해 주세요.";
            setText(situationMainText, uiStr);
        }
 

        // 환자의 의식을 파악해야 하는 상황
        if (snake.isPatientCons && !snake.isCall119)
        {
            uiStr = "환자가 뱀에 물렸습니다.\n환자를 최대한 움직이지 않게 해주세요.\n(R키를 눌러 액션)";
            setText(situationMainText, uiStr);
        }

        // 의식을 파악해야 하는 상황에서 R키 액션을 취함
        if (snake.isPatientCons && !snake.didPatientCons && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
            uiStr = "\"괜찮으세요? 움직이지 마세요.\"";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);

            snake.didPatientCons = true; // 환자 의식 파악 완료함
        }

        // 의식 파악까지 완료한 상황 (자막 유지)
        if (snake.didPatientCons && !snake.isCall119)
        {
            uiStr = "\"괜찮으세요? 움직이지 마세요.\"";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);

            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                snake.isCall119 = true; // 다른 사람들에게 도움을 요청해야함
            }));

        }

        // 다른 사람에게 도움을 요청해야 하는 상황
        if (snake.isCall119 && !snake.didCall119)
        {
            uiStr = "주변 사람에게 119 구조 요청을 부탁하세요\n(R키를 눌러 액션)";
            situationMainText.color = Color.white;
            setText(situationMainText, uiStr);
        }

        // 119 불러달라고 해야 하는 순서
        if (snake.isCall119 && !snake.didCall119 && playerScript.rayCollObject.name == "For Snake  119" && playerScript.doAction())
        {
            uiStr = "~~~이신 분 119에 구조 요청 부탁드립니다.";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);
            snake.didCall119 = true;
            // 2초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(2f, () =>
                {
                    snake.isCalmDown = true;
                }));
        }

        // 119 불렀으면 자막 유지
        if (snake.didCall119 && snake.isCalmDown && !snake.didCalmDown)
        {
            situationMainText.color = Color.white;
            uiStr = "흥분하면 독이 더 빨리 퍼지므로 환자를 안정시키세요.\n(R키를 눌러 액션)";
            setText(situationMainText, uiStr);

        }

        //환자 안정
        if (snake.isCalmDown && !snake.didCalmDown && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
                snake.didCalmDown = true;
        }
        if (snake.didCalmDown && !snake.isRemove)
        {
            situationMainText.color = Color.yellow;
            uiStr = "흥분하면 독이 더 빨리 퍼집니다. 심호흡을 해서 안정을 취하세요.";
            setText(situationMainText, uiStr);
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                snake.isRemove = true;
            }));
        }

        // 순환을 방해하는 것 제거
        if (snake.isRemove && !snake.didRemove)
        {
            situationMainText.color = Color.white;
            uiStr = "반지나 시계 등 부어오르면서 혈액 순환을 방해할 수 있는 물건을 제거해주세요.\n(R키를 눌러 액션)";
            setText(situationMainText, uiStr);
        }
        if (snake.isRemove && !snake.didRemove && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
                snake.didRemove = true;
        }
        if (snake.didRemove && !snake.isDown)
        {
            situationMainText.color = Color.yellow;
            uiStr = "(혈액 순환을 방해할 수 있는 물건을 제거했습니다.)";
            setText(situationMainText, uiStr);
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                snake.isDown = true;
            }));
        }

        //물린 부위 심장보다 아래로
        if (snake.isDown && !snake.didDown)
        {
            situationMainText.color = Color.white;
            uiStr = "물린 부위를 심장보다 아래로 가게 해주세요.\n(R키를 눌러 액션)";
            setText(situationMainText, uiStr);
        }
        if (snake.isDown && !snake.didDown && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
                snake.didDown = true;
        }
        if (snake.didDown && !snake.isTie)
        {
            situationMainText.color = Color.yellow;
            uiStr = "(물린 부위를 심장보다 아래로 가게 두었습니다.)";
            setText(situationMainText, uiStr);
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                snake.isTie = true;
            }));
        }

        //붓는 경우 묶기
        if (snake.isTie && !snake.didTie)
        {
            tieHintImg.SetActive(true);
            situationMainText.color = Color.white;
            uiStr = "이미지 UI를 참고하여 물린 부위에서 \n5~10cm정도 심장 쪽에서 가까운 부위를 묶어주세요.\n너무 꽉 조일 경우 2차 손상을 가져올 수 있으니 주의해주세요.\n(R키를 눌러 액션)";
            setText(situationMainText, uiStr);
        }
        if (snake.isTie && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
            snake.didTie = true;
        }
        if (snake.didTie && !snake.finishSnake)
        {
            situationMainText.color = Color.yellow;
            uiStr = "(손수건으로 5~10cm정도 심장 쪽에서 가까운 부위를 묶었습니다.)";
            setText(situationMainText, uiStr);
            // 3초 대기 후 실행할 내용
            StartCoroutine(DelayedAction(3f, () =>
            {
                snake.finishSnake = true;
            }));
        }
        

        // Snake 상황 끝!
        if (snake.finishSnake)
        {
            tieHintImg.SetActive(false);

            uiStr = "119 구조대가 도착합니다.";
            setText(situationMainText, uiStr);

            moveHellicopter();
            activeExitBtn = true; // esc 버튼 활성화
        }

    }

    // Bee 상황에서 UI 관리
    public void setUIBee()
    {
        // 벌에 쏘임
        if (bee.isAttacked && !bee.isCall119)
        {
            situationMainText.color = Color.white;
            situationMainTextPanel.SetActive(true);
            uiStr = "벌에 쏘였습니다!\n119에 연락을 취하고 응급처치를 시작하세요\n(Q를 눌러 액션).";
            setText(situationMainText, uiStr);

            if (beePlayerScript.doAction())
            {
                bee.isCall119 = true;

            }
        }

        // 119 부름
        if (bee.isCall119 && !bee.isCard)
        {
            situationMainText.color = Color.yellow;
            uiStr = "119를 불렀습니다.";
            setText(situationMainText, uiStr);

            StartCoroutine(DelayedAction(3f, () =>
            {
                bee.isCard = true;
            }));
        }

        // 침 빼야함
        if (bee.isCard && !bee.didCard)
        {
            situationMainText.color = Color.white;
            uiStr = "이미지를 참고하여 벌침을 제거하세요.";
            setText(situationMainText, uiStr);
            cardHintImg.SetActive(true);

            if (beePlayerScript.doAction())
            {
                bee.didCard = true;
            }
        }

        if (bee.didCard && !bee.isIcing)
        {
            situationMainText.color = Color.yellow;
            uiStr = "침을 제거했습니다.";
            setText(situationMainText, uiStr);
            cardHintImg.SetActive(false);

            StartCoroutine(DelayedAction(3f, () =>
            {
                bee.isIcing = true;
            }));
        }

        // 냉찜질 할 차례
        if (bee.isIcing && !bee.didIcing)
        {
            situationMainText.color = Color.white;
            uiStr = "이미지를 참고하여 냉찜질을 진행하세요.";
            setText(situationMainText, uiStr);
            icingHingImg.SetActive(true);

            if (beePlayerScript.doAction())
            {
                bee.didIcing = true;
            }
        }

        // 냉찜질 함
        if (bee.didIcing && !bee.isKeepHigh)
        {
            situationMainText.color = Color.yellow;
            uiStr = "냉찜질은 완료했습니다.";
            setText(situationMainText, uiStr);
            icingHingImg.SetActive(false);

            StartCoroutine(DelayedAction(3f, () =>
            {
                bee.isKeepHigh = true;
            }));
        }

        if (bee.isKeepHigh)
        {
            icingHingImg.SetActive(false);
            uiStr = "119가 도착할 때까지 쏘인 부위를. 높이 유지하세요";
            setText(situationMainText, uiStr);

            moveHellicopter();
            activeExitBtn = true; // esc 버튼 활성화
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
