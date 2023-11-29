using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureSituation : MonoBehaviour
{
    public float patientMoveSpeed = 1f; // ȯ�ڰ� ������ �� ���� �ӵ�

    public GameObject player; // �÷��̾�
    public GameObject patient; // ȯ��

    public bool isPatientDown;  // ȯ�ڰ� ������ ��Ȳ�ΰ���?
    public bool isPatientCons;  // ȯ�� �ǽ��� �ľ��ؾ� �ϳ���?
    public bool isHelpOther;    // �ٸ� ������� ������ ��û�ؾ� �ϴ� ��Ȳ?
    public bool didCall119; // �ٸ� ������� 119 �ҷ��޶�� ��û�ߴ���?
    public bool isTakeOff; //�ջ� ������ Ȯ���ϱ� ���� ȯ���� �� ����
    public bool didTakeOff; //ȯ���� �� ���ż���Ϸ�
    public bool isPress; // ���� �����ؾ��ϴ� ��Ȳ
    public bool didPress; // ���� ����Ϸ�
    public bool isSplint; // �θ� �����ؾ��ϴ� ��Ȳ
    public bool didPickUp; // �θ� �ݱ�
    public bool didSplint; //�θ� ����Ϸ�
    public bool isIcing; //������ �����ؾ��ϴ� ��Ȳ
    public bool didIcing; //������ ����Ϸ�



    public bool finishFracture; // Fracture ��Ȳ ����

    private Animator patientAnimator; //ȯ�� Animator ������Ʈ
    public CapsuleCollider capsuleCollider; // ĸ�� �ݶ��̴� ������Ʈ
    private Vector3 patientFallPosition;// ȯ�� ������ ��ġ
    private Quaternion patientFallRotation; //ȯ�� ������ ����
    public bool isFixed = false;//ȯ�� ����

    private float time = 0f;

    void Start()
    {
        patientAnimator = patient.GetComponent<Animator>();
        capsuleCollider = patient.GetComponent<CapsuleCollider>();
    }


    void Update()
    {
        if (isFixed)
        {
            FixPatientPosition(); // ȯ�ڰ� �����Ǿ� ���� �� ��ġ �� ȸ�� ����
        }
    }

    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("Fracture Situation Start");

        yield return new WaitForSeconds(2f); // 2�� ��� �� ����


        //ȯ�� �ȴ� �ִϸ��̼�
        patientAnimator.SetBool("isWalking", true);


        while (time <= 3f) // 3�ʵ��� ���� (ȯ�ڰ� ������ ���ϴ�)
        {
            moveForwardPatient();

            time += Time.deltaTime;
            //Debug.Log(time.ToString());
            yield return null;
        }
        time = 0f;

        isPatientDown = true;


        //ȯ�� �������� �ִϸ��̼�
        patientFalling();
        yield return new WaitForSeconds(0.5f);
        patientFallPosition = patient.transform.position;
        patientFallRotation = patient.transform.rotation;


        isFixed = true;

        yield return null;
        yield break;
    }





    private void patientFalling()
    {
        patientAnimator.SetBool("isFalling", true);
        capsuleCollider.direction = 2; //�ݶ��̴��� ���� Z������ ���� (0: X��, 1: Y��, 2: Z��)
        capsuleCollider.center = new Vector3(0f, 0.2f, 0f); //�߽� ��ġ ����
        capsuleCollider.radius = 0.3f; //radius ����
    }

    private void patientGetUp()//ȯ�� �Ͼ ��
    {
        patientAnimator.SetBool("isFalling", false);
        patientAnimator.SetBool("getUp", true);
        capsuleCollider.direction = 1; // �⺻ ���� (Y��)
        capsuleCollider.center = new Vector3(0f, 0.9f, 0f); // �⺻ �߽� ��ġ��
        capsuleCollider.height = 0.9f; // �⺻ ����
    }

    //���� �ð� ���� �ٽ� idle���·� �� ��(getUp �� ȸ���ϴ� ��Ȳ�� ���)
    private void patientIdle()
    {
        patientAnimator.SetBool("getUp", false);
        patientAnimator.SetBool("idle", true);
    }

    // ȯ�ڸ� ������ �̵���Ű�� �Լ�
    private void moveForwardPatient()
    {
        patient.transform.Translate(Vector3.forward * patientMoveSpeed * Time.deltaTime); // ȯ�� ������ �̵�
    }
    private void FixPatientPosition()
    {
        patient.transform.position = patientFallPosition; // ������ ��ġ�� ȯ�� �̵�
        patient.transform.rotation = patientFallRotation; // ������ ���� ȸ�� ������ ȯ�� ȸ��
    }
}





/*




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
*/