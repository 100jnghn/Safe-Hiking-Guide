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

    public GameObject cprSituation; // CPR ��Ȳ�� ���� (Start Position CPR)
    private CPRSituation cpr; // cprSituation ������Ʈ�� Component�� CPRSituation ��ũ��Ʈ�� ������

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
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
        if (cpr.isPatientDown) // ȯ�ڰ� �������ٸ�
        {
            situationMainTextPanel.SetActive(true);
            situationMainText.text = "ȯ�ڰ� �߻��߽��ϴ�!\n������ �ٰ��� ���¸� �ľ��� �ּ���.";
        }

        if (cpr.isPatientCons) // ȯ���� �ǽ��� �ľ��ؾ� �ϴ� ��Ȳ
        {
            situationMainText.text = "����� �ε帮��, \"����������?\"��� �����\n�ǽ��� �ľ��� �ּ���.";
        }
    }
}
