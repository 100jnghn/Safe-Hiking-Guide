using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startSceneUIPanel; // ���� ȭ�� UI
    public GameObject situationUIPanel; // ��Ȳ���� UI
    public GameObject situationMainTextPanel; // �ڸ� �г�
    
    public Text situationMainText; // �� ��Ȳ������ ���� �ڸ� text

    public GameObject chestPressHintImg; // ���� �й� ��Ʈ �̹���
    public GameObject artificialResHintImg; // �ΰ�ȣ�� ��Ʈ �̹���
    public GameObject pressHintImg; // ���� ��Ʈ �̹���
    public GameObject splintHintImg;// �θ� ��� ��Ʈ �̹���

    public GameObject startSceneCam; // ���� ȭ�� ī�޶�
    public GameObject playerCam; // �÷��̾� ������ ī�޶�
    public GameObject player; // �÷��̾� ��ü
    private Player playerScript; // �÷��̾� ��ũ��Ʈ

    public GameObject hellicopter; // �︮���� ������Ʈ
    private Hellicopter hellicopterScript; // �︮���� ��ũ��Ʈ

    public GameObject cprSituation; // CPR ��Ȳ�� ���� (Start Position CPR)
    private CPRSituation cpr; // cprSituation ������Ʈ�� Component�� CPRSituation ��ũ��Ʈ�� ������

    public GameObject fractureSituation;// Fracture ��Ȳ�� ���� (Start Position Fracture)
    private FractureSituation fracture;// fractureSituation ������Ʈ�� Component�� FractureSituation ��ũ��Ʈ�� ������

    public GameObject beeSituation; // Bee ��Ȳ�� ����
    private BeeSituation bee; // beeSituation ��ũ��Ʈ

    public GameObject snakeSituation; // Snake ��Ȳ�� ����
    private BeeSituation snake; // snakeSituation ��ũ��Ʈ

    private string uiStr; // �ڸ��� �� ����

    public enum Mode { Nothing, CPR, Fracture, Snake, Bee };
    public Mode mode; // ���� � ������� ����

    private bool activeExitBtn; // esc ��ư�� ���� ���� ȭ������ �� �� �ִ��� �Ǵ��ϴ� ����

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
        fracture = fractureSituation.GetComponent<FractureSituation>();
        bee = beeSituation.GetComponent<BeeSituation>();

        playerScript = player.GetComponent<Player>();
        hellicopterScript = hellicopter.GetComponent<Hellicopter>();

        mode = Mode.Nothing; // ������ �ƹ��� ��尡 �ƴ� ����
        splintHintImg.SetActive(false);
        pressHintImg.SetActive(false);

    }

    void Update()
    {
        setUICPR(); // CPR ��Ȳ������ UI�� ����
        setUIFracture(); // Fracture ��Ȳ������ UI�� ����

        if (activeExitBtn && Input.GetKeyDown(KeyCode.Escape)) { finishSituation(); }
    }

    // ��Ȳ ����
    public void startSituation()
    {
        startSceneUIPanel.SetActive(false); // ���� ȭ�� UI ����
        startSceneCam.SetActive(false); // ���� ȭ�� ī�޶� ����
        player.SetActive(true); // �÷��̾� Ȱ��ȭ
        playerCam.SetActive(true); // �÷��̾� �������� ��ȯ

        situationUIPanel.SetActive(true); // Situation�� UI �ѱ�
    }

    // ��Ȳ ��
    public void finishSituation()
    {
        StopAllCoroutines(); // �������� �ڷ�ƾ ��� �����Ŵ


        // ��� �ʱ�ȭ
        mode = Mode.Nothing;

        // �� ��Ȳ�� ���� �ʱ�ȭ
        // �� ��Ȳ�� ������Ʈ ��ġ �ʱ�ȭ
        cpr.Reset();
        fracture.Reset();


        // �︮���� ��ġ �ʱ�ȭ + SetActive(false)
        hellicopterScript.Reset();

        // UI ����
        situationUIPanel.SetActive(false); // ��Ȳ���� UI
        situationMainTextPanel.SetActive(false); // �ڸ� �г�
        startSceneUIPanel.SetActive(true); // ���� ȭ�� UI

        // ī�޶� ����
        playerCam.SetActive(false); // �÷��̾� ������ ī�޶�
        startSceneCam.SetActive(true); // ���� ȭ�� ī�޶�

        Debug.Log("Finish Situations");
}

    // �︮���� �̵���Ű�� �Լ�
    public void moveHellicopter()
    {
        hellicopter.SetActive(true); // �︮���� Ȱ��ȭ
        // ���� ��忡 ���� �ش� ��ġ�� ��� �̵�
        
    }

    // ���� �޴� ȭ�鿡�� ����һ��� ��Ȳ Ŭ��
    public void startCPR()
    {
        mode = Mode.CPR; // ���� ��带 CPR ����

        player.transform.position = cprSituation.transform.position; // ���� ��ġ ����
        startSituation();
        cpr.StartCoroutine("startSituation"); // CPR ��Ȳ ����
    }

    // ���� �޴� ȭ�鿡�� �쿡 ������ ��Ȳ Ŭ��
    public void startSnake()
    {
        mode = Mode.Snake; // (�߰��� :) ���� ��带 �칰�� ����

        player.transform.position = snakeSituation.transform.position;// ���� ��ġ ����
        startSituation();
        snake.StartCoroutine("startSituation");

        startSituation();

    }

    // ���� �޴� ȭ�鿡�� ���� ��Ȳ Ŭ��
    public void startFracture()
    {
        mode = Mode.Fracture; // (�߰��� :) ���� ��带 ���� ����

        player.transform.position = fractureSituation.transform.position;// ���� ��ġ ����
        startSituation();
        fracture.StartCoroutine("startSituation");

    }

    // ���� �޴� ȭ�鿡�� ���� ���̴� ��Ȳ Ŭ��
    public void startBee()
    {
        mode = Mode.Bee;
        startSituation();
        playerCam.SetActive(false);
        bee.StartCoroutine("startSituation");
    }

    // CPR ��Ȳ���� UI ����
    public void setUICPR()
    {
        // ȯ�ڰ� �������ٸ�
        if (cpr.isPatientDown && !cpr.isPatientCons) 
        {
            situationMainTextPanel.SetActive(true);
            uiStr = "ȯ�ڰ� �߻��߽��ϴ�!\n������ �ٰ��� ���¸� �ľ��� �ּ���.";
            setText(situationMainText, uiStr);
        }

        // ȯ���� �ǽ��� �ľ��ؾ� �ϴ� ��Ȳ
        if (cpr.isPatientCons && !cpr.isHelpOther) 
        {
            uiStr = "����� �ε帮��, \"����������?\"��� �����\n�ǽİ� ȣ�� ���θ� �ľ��� �ּ���.\n(RŰ�� ���� �׼�)";
            setText(situationMainText, uiStr);
        }

        // �ǽ��� �ľ��ؾ� �ϴ� ��Ȳ���� RŰ �׼��� ����
        if (cpr.isPatientCons && !cpr.didPatientCons && !cpr.isHelpOther && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
            uiStr = "\"����������?\"";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);

            cpr.didPatientCons = true; // ȯ�� �ǽ� �ľ� �Ϸ���
        } 

        // �ǽ� �ľǱ��� �Ϸ��� ��Ȳ (�ڸ� ����)
        if (cpr.didPatientCons && !cpr.isHelpOther)
        {
            uiStr = "\"����������?\"";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);

            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                cpr.isHelpOther = true; // �ٸ� ����鿡�� ������ ��û�ؾ���
            }));

        }

        // �ٸ� ������� ������ ��û�ؾ� �ϴ� ��Ȳ
        if (cpr.isHelpOther && !cpr.didCall119 && !cpr.didCallAED)
        {
            uiStr = "ȯ�ڰ� �ǽ��� �����ϴ�.\n�ֺ� ����鿡�� ������ ��û�ϰ� CPR�� �����ϼ���.\n(RŰ�� ���� �׼�)";
            situationMainText.color = Color.white;
            setText(situationMainText, uiStr);
        }

        // 119 �ҷ��޶�� �ؾ� �ϴ� ����
        if (cpr.isHelpOther && !cpr.didCall119 && !cpr.didCallAED && playerScript.rayCollObject.name == "For CPR 119" && playerScript.doAction())
        {
            uiStr = "~~~�̽� �� 119�� ���� ��û ��Ź�帳�ϴ�.";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);
            cpr.didCall119 = true;
        }

        // 119 �ҷ����� �ڸ� ����
        if (cpr.isHelpOther && cpr.didCall119 && !cpr.didCallAED)
        {
            setText(situationMainText, uiStr);
        }

        // AED ��û�ؾ� �ϴ� ����
        if (cpr.isHelpOther && cpr.didCall119 && !cpr.didCallAED && playerScript.rayCollObject.name == "For CPR AED" && playerScript.doAction())
        {
            uiStr = "~~~�̽� �� AED�� ������ �ּ���.";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);
            cpr.didCallAED = true;
        }

        // AED ��û������ �ڸ� ����
        if (cpr.isHelpOther && cpr.didCall119 && cpr.didCallAED)
        {
            setText(situationMainText, uiStr);

            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                cpr.isChestPress = true; // �ٸ� ����鿡�� ������ ��û�ؾ���
            }));
        }

        // ���� �й� �����ؾ� �ϴ� ����
        if (cpr.isChestPress && !cpr.didChestPress)
        {
            uiStr = "�̹����� �����Ͽ� ��Ȯ�� �ڼ��� ���� �й��� �����ϼ���.\n(RŰ�� ���� �׼�)";
            situationMainText.color = Color.white;
            setText(situationMainText, uiStr);

            // ----- �̹��� ���� �ڵ� �ۼ� ----- //
            chestPressHintImg.SetActive(true);
        }

        // ���� �й��� ����
        if (cpr.isChestPress && !cpr.didChestPress && playerScript.rayCollObject.name == "Patient CPR" && playerScript.doAction())
        {
            uiStr = "���� �й��� �д� 100~120ȸ�� ���ϰ� ��Ģ���̰� �����ؾ� �մϴ�.";
            setText(situationMainText, uiStr);

            cpr.didChestPress = true;       
        }

        // ���� �й� ���� �Ϸ�
        if (cpr.isChestPress && cpr.didChestPress)
        {
            // ----- �̹��� �ٽ� �� ���̰� ----- //
            chestPressHintImg.SetActive(false);

            uiStr = "���� �й��� �д� 100~120ȸ�� ���ϰ� ��Ģ���̰� �����ؾ� �մϴ�.";
            setText(situationMainText, uiStr);

            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                cpr.isArtificialRes = true; // �ΰ�ȣ���� �����ؾ� ��
            }));
        }
        
        // �ΰ�ȣ���� �����ؾ� �ϴ� ����
        if (cpr.isArtificialRes && !cpr.didArtificialRes)
        {
            uiStr = "ȯ���� �Ӹ��� ���� �⵵�� Ȯ���ϰ�\n �̹����� �����Ͽ� ��Ȯ�� �ڼ��� �ΰ�ȣ���� �����ϼ���\n(RŰ�� ���� �׼�)";
            setText(situationMainText, uiStr);

            // ----- �̹����� ���� �ڵ� �ۼ� ----- //
            artificialResHintImg.SetActive(true);
        }

        // �ΰ�ȣ���� ����
        if (cpr.isArtificialRes && !cpr.didArtificialRes && playerScript.rayCollObject.name == "Patient CPR" && playerScript.doAction())
        {
            uiStr = "30ȸ �����й�, 2ȸ �ΰ�ȣ���� 119�� ������ ������ �ݺ� ������ �ּ���";
            setText(situationMainText, uiStr);

            cpr.didArtificialRes = true;
        }

        // �ΰ�ȣ�� ���� �Ϸ�
        if (cpr.isArtificialRes && cpr.didArtificialRes)
        {
            // ----- �̹��� �ٽ� �� ���̰� ----- //
            artificialResHintImg.SetActive(false);

            uiStr = "30ȸ �����й�, 2ȸ �ΰ�ȣ���� 119�� ������ ������ �ݺ� ������ �ּ���";
            setText(situationMainText, uiStr);

            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                cpr.finishCPR = true; // CPR ��Ȳ ��
            }));
        }

        // CPR ��Ȳ ��!
        if (cpr.finishCPR)
        {
            uiStr = "119 �����밡 �����մϴ�.";
            setText(situationMainText, uiStr);

            moveHellicopter();
            activeExitBtn = true; // esc ��ư Ȱ��ȭ
        }

    }









    //Fracture UI
    public void setUIFracture()
    {
        // ȯ�ڰ� �������ٸ�
        if (fracture.isPatientDown && !fracture.isPatientCons)
        {
            situationMainTextPanel.SetActive(true);
            uiStr = "ȯ�ڰ� �߻��߽��ϴ�!\n������ �ٰ��� ���¸� �ľ��� �ּ���.";
            setText(situationMainText, uiStr);
        }

        //������ �ǽɵ� ��쿡�� �׻� ������ �����ϰ� óġ. �Ժη� �������ų� ���� ���� �ʴ´�.
        if (fracture.isPatientCons && !fracture.didCall119)
        {
            uiStr = "������ �ǽɵ˴ϴ�.\n�Ժη� �������ų� ����� ������.\n�ֺ� ������� 119�� �Ű��� �޶�� ���ּ���.(RŰ�� ���� �����մϴ�.)";
            setText(situationMainText, uiStr);
        }

        // 119 �Ű� ��û
        if (!fracture.didCall119 && playerScript.rayCollObject != null && playerScript.rayCollObject.name == "For Fracture 119" && playerScript.doAction())
        {
            uiStr = "~~~�̽� �� 119�� ���� ��û ��Ź�帳�ϴ�.";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);
            fracture.didCall119 = true;

            // 2�� ��� �� ������ ����
            StartCoroutine(DelayedAction(2f, () =>
            {
                fracture.isTakeOff = true; // �� �����ؾ���
            }));
        }

        //�ջ� ������ Ȯ���ϱ� ���� ȯ���� �� ����
        if (fracture.didCall119&&fracture.isTakeOff && !fracture.isPress &&!fracture.didTakeOff)
        {
            situationMainText.color = Color.white;
            uiStr = "ȯ���� ���¸� �ڼ��� Ȯ���ϱ����� \"RŰ\"�� 3�ʵ��� ���� ȯ���� ���� �������ּ���";
            setText(situationMainText, uiStr);
        }

        //ȯ�� �� ���� ���� �Ϸ�
        if (fracture.didCall119&&fracture.isTakeOff && !fracture.isPress&& playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
                fracture.didTakeOff = true;
                
                Debug.Log("��: " + fracture.didTakeOff);
            
        }

        if (!fracture.isPress && fracture.didTakeOff)
        {
            uiStr = "(���� �����߽��ϴ�.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.isPress = true;
            }));
        }

        /////

        //���� �� ����
        if (fracture.isPress && !fracture.didPress)
        {
            pressHintImg.SetActive(true);
            situationMainText.color = Color.white;
            uiStr = "�ٸ��� ������ �ֽ��ϴ�.\n ������ �ϱ����� \"PŰ\"�� 3�ʵ��� �����ּ���";
            setText(situationMainText, uiStr);
        }

        //���� ���� �Ϸ�
        if (fracture.isPress && !fracture.didPress && playerScript.rayCollObject.tag == "Patient" && playerScript.doPress())
        {
                fracture.didPress = true;
        }
        if (fracture.didPress && !fracture.isSplint)
        {
            uiStr = "(�����߽��ϴ�.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.isSplint = true;
            }));
        }


        // ���� ���� �Ϸ� & �θ���
        if (fracture.isSplint && !fracture.didSplint)
        {
            pressHintImg.SetActive(false);// �����̹��� �� ���̰�
            splintHintImg.SetActive(true);// �θ��̹����� ��ü

            situationMainText.color = Color.white;
            uiStr = "�̹���UI�� �����Ͽ� RŰ�� ���� �θ��� ���, �ٸ��� �������� �ʰ� �������ּ���.";
            setText(situationMainText, uiStr);
        }

        //�θ��� ���� �Ϸ�
        if (fracture.isSplint && !fracture.didSplint && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
            splintHintImg.SetActive(false);// �θ��̹��� �� ���̰�            
            fracture.didSplint = true;
        }
        if (fracture.didSplint && !fracture.isIcing)
        {
            uiStr = "(�θ��� ������ϴ�.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.isIcing = true;
            }));
        }

        //������ ����
        if (fracture.isIcing && !fracture.didIcing && !fracture.finishFracture)
        {
            situationMainText.color = Color.white;
            uiStr = "�ױ�� ������ ���̱����� �������� �ʿ��մϴ�.\n \"I��ư\"�� 3�ʵ��� �����ּ���";
            setText(situationMainText, uiStr);
        }


        //������ ���� �Ϸ�
        if (fracture.didSplint && fracture.isIcing && playerScript.rayCollObject.tag == "Patient" && playerScript.doIcing())
        {
            fracture.didIcing = true;
        }
        if (fracture.didIcing)
        {
            uiStr = "(�������� �߽��ϴ�.)";
            setText(situationMainText, uiStr);
            situationMainText.color = Color.yellow;
            // 3�� ��� �� ������ ����
            StartCoroutine(DelayedAction(3f, () =>
            {
                fracture.finishFracture = true;
            }));
        }

        // Fracture ��Ȳ ��
        if (fracture.finishFracture)
        {
            situationMainText.color = Color.white;
            uiStr = "119 �����밡 �����մϴ�.";
            setText(situationMainText, uiStr);

            moveHellicopter();
            activeExitBtn = true; // esc ��ư Ȱ��ȭ
        }

    }


    // delay��ŭ ����ϴ� �ڷ�ƾ
    private IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    // Text�� ������ ����
    private void setText(Text text, string str)
    {
        text.text = str;
    }

}
