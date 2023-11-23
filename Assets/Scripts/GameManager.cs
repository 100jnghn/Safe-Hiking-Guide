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

    private string uiStr; // �ڸ��� �� ����

    void Start()
    {
        cpr = cprSituation.GetComponent<CPRSituation>();
        //Fracture = FractureStituation.
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
        //player.transform.position = 
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
            uiStr = "ȯ�ڰ� �߻��߽��ϴ�!\n������ �ٰ��� ���¸� �ľ��� �ּ���.";
            setText(situationMainText, uiStr);
        }

        // ȯ���� �ǽ��� �ľ��ؾ� �ϴ� ��Ȳ
        if (cpr.isPatientCons && !cpr.isHelpOther) 
        {
            uiStr = "����� �ε帮��, \"����������?\"��� �����\n�ǽ��� �ľ��� �ּ���.";
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
        if (cpr.didPatientCons)
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
        if (cpr.isHelpOther)
        {
            uiStr = "���� ���� ��û�ؾ� �Ѵٴ� ����";
            situationMainText.color = Color.white;
            setText(situationMainText, uiStr);
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
