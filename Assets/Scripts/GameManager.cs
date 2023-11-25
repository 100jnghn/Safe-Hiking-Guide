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

    public GameObject startSceneCam; // ���� ȭ�� ī�޶�
    public GameObject playerCam; // �÷��̾� ������ ī�޶�
    public GameObject player; // �÷��̾� ��ü
    private Player playerScript; // �÷��̾� ��ũ��Ʈ

    public GameObject hellicopter; // �︮���� ������Ʈ

    public GameObject cprSituation; // CPR ��Ȳ�� ���� (Start Position CPR)
    private CPRSituation cpr; // cprSituation ������Ʈ�� Component�� CPRSituation ��ũ��Ʈ�� ������

    public GameObject fractureSituation;// Fracture ��Ȳ�� ���� (Start Position Fracture)
    private FractureSituation fracture;// fractureSituation ������Ʈ�� Component�� FractureSituation ��ũ��Ʈ�� ������

    private string uiStr; // �ڸ��� �� ����

    public enum Mode { Nothing, CPR, Fracture, Snake, Bee };
    public Mode mode; // ���� � ������� ����

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
        fracture = fractureSituation.GetComponent<FractureSituation>();
        playerScript = player.GetComponent<Player>();

        mode = Mode.Nothing; // ������ �ƹ��� ��尡 �ƴ� ����
    }

    void Update()
    {
        setUICPR(); // CPR ��Ȳ������ UI�� ����
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
        moveHellicopter(); // ���� : ��� �̵�

        // ��� �ʱ�ȭ
        // �� ��Ȳ�� ���� �ʱ�ȭ
        // �� ��Ȳ�� ������Ʈ ��ġ �ʱ�ȭ
        // �︮���� ��ġ �ʱ�ȭ + SetActive(false)
        // UI ����
        // ī�޶� ����
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
        startSituation();

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
            uiStr = "����� �ε帮��, \"����������?\"��� �����\n�ǽİ� ȣ�� ���θ� �ľ��� �ּ���.";
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
            uiStr = "ȯ�ڰ� �ǽ��� �����ϴ�.\n�ֺ� ����鿡�� ������ ��û�ϰ� CPR�� �����ϼ���.";
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
            uiStr = "�̹����� �����Ͽ� ��Ȯ�� �ڼ��� ���� �й��� �����ϼ���.";
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
            uiStr = "ȯ���� �Ӹ��� ���� �⵵�� Ȯ���ϰ�\n �̹����� �����Ͽ� ��Ȯ�� �ڼ��� �ΰ�ȣ���� �����ϼ���";
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

            finishSituation();
        }

    }


    /*
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
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "������ �ǽɵ˴ϴ�.\n �Ժη� �������ų� ����� ������.";
            setText(situationMainText, uiStr);
        }

        //ȯ�� �ٸ� ���� Ȯ��
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "ȯ�ڰ� �ٸ��� �������� ���մϴ�.\n �ڼ��� Ȯ���ϱ����� \"C��ư\"�� ������ �ɾ��ּ���";
            setText(situationMainText, uiStr);
        }

        //�ջ� ������ Ȯ���ϱ� ���� ȯ���� �� ����
        if (fracture.isPatientCons && !cpr.didPatientCons && !fracture.isHelpOther && playerScript.rayCollObject.tag == "Patient" && playerScript.doAction())
        {
            uiStr = "ȯ���� ���� �����ϱ����� \"R��ư\"�� 3�ʵ��� �����ּ���";
            setText(situationMainText, uiStr);
        }

        //���� �� ����
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "�ٸ��� ������ �ֽ��ϴ�.\n ������ �ϱ����� \"P��ư\"�� 3�ʵ��� �����ּ���";
            setText(situationMainText, uiStr);
        }


        //�θ� ����
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "�������� �θ��� �ֿ��� �ٸ��� �������� �ʰ� �������ּ���.\n �θ� ������ �ٰ����� ������ �� �ֽ��ϴ�.\n �θ��� ��� ����� �̹���UI�� �������ּ���.";
            setText(situationMainText, uiStr);
        }

        //������
        if (fracture.isPatientCons && !fracture.isHelpOther)
        {
            uiStr = "�ױ�� ������ ���̱����� �������� �ʿ��մϴ�.\n \"I��ư\"�� 3�ʵ��� �����ּ���";
            setText(situationMainText, uiStr);
        }
    
    }
    */









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
