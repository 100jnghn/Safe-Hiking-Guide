using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject startSceneUIPanel; // ���� ȭ�� UI
    public GameObject situationUIPanel; // ��Ȳ���� UI
    public Text situationMainText; // �� ��Ȳ������ ���� �ڸ� text
    public GameObject situationMainTextPanel; // �ڸ� �г�

    public GameObject startSceneCam; // ���� ȭ�� ī�޶�
    public GameObject playerCam; // �÷��̾� ������ ī�޶�
    public GameObject player; // �÷��̾� ��ü
    private Player playerScript; // �÷��̾� ��ũ��Ʈ

    public GameObject cprSituation; // CPR ��Ȳ�� ���� (Start Position CPR)
    private CPRSituation cpr; // cprSituation ������Ʈ�� Component�� CPRSituation ��ũ��Ʈ�� ������

    private float waitTimer = 0f; // �׼� �� ���� �ð��� ��ٸ��� ���� ���

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
        playerScript = player.GetComponent<Player>();
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

    // ���� �޴� ȭ�鿡�� ����һ��� ��Ȳ Ŭ��
    public void startCPR()
    {
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
        startSituation();

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
            situationMainText.text = "ȯ�ڰ� �߻��߽��ϴ�!\n������ �ٰ��� ���¸� �ľ��� �ּ���.";
        }

        // ȯ���� �ǽ��� �ľ��ؾ� �ϴ� ��Ȳ
        if (cpr.isPatientCons && !cpr.isHelpOther) 
        {
            situationMainText.text = "����� �ε帮��, \"����������?\"��� �����\n�ǽ��� �ľ��� �ּ���.";
        }

        // �ǽ��� �ľ��ؾ� �ϴ� ��Ȳ���� RŰ �׼��� ����
        if (cpr.isPatientCons && !cpr.isHelpOther && playerScript.doAction())
        {
            situationMainText.color = Color.yellow;
            situationMainText.text = "\"����������?\"";
            cpr.isHelpOther = true;
        } 

        // �ٸ� ������� ������ ��û�ؾ� �ϴ� ��Ȳ
        if (cpr.isHelpOther)
        {
            situationMainText.color = Color.white;
            situationMainText.text = "�ֺ��� �ѷ����� �ٸ� ����鿡�� ������ ��û�ؾ� �մϴ�.";
        }
    }

}
