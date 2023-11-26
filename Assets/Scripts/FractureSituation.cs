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


    public bool finishFracture; // Fracture ��Ȳ ����

    private Animator patientAnimator; //ȯ�� Animator ������Ʈ
    public CapsuleCollider capsuleCollider; // ĸ�� �ݶ��̴� ������Ʈ


    private float time = 0f;

    void Start()
    {
        patientAnimator = patient.GetComponent<Animator>();
        capsuleCollider = patient.GetComponent<CapsuleCollider>();
    }


    void Update()
    {

    }

    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("Fracture Situation Start");

        yield return new WaitForSeconds(1f); // 1�� ��� �� ����


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
        if (fracture.isPatientCons && !fracture.didCall119)
        {
            uiStr = "������ �ǽɵ˴ϴ�.\n�Ժη� �������ų� ����� ������.\n�ֺ� ������� 119�� �Ű��� �޶�� ���ּ���.";
            setText(situationMainText, uiStr);
        }

        // 119 �Ű� ��û
        if (fracture.isHelpOther && !fracture.didCall119 && !cpr.didCallAED && playerScript.rayCollObject.name == "For Fracture 119" && playerScript.doAction())
        {
            uiStr = "~~~�̽� �� 119�� ���� ��û ��Ź�帳�ϴ�.";
            situationMainText.color = Color.yellow;
            setText(situationMainText, uiStr);
            cpr.didCall119 = true;
        }

            //������ �ǽɵ� ��쿡�� �׻� ������ �����ϰ� óġ. �Ժη� �������ų� ���� ���� �ʴ´�.
        if (fracture.isPatientCons && !fracture.didCall119)
        {
            uiStr = "������ �ǽɵ˴ϴ�.\n �Ժη� �������ų� ����� ������.\n�ֺ� ������� 119�� �Ű��� �޶�� ���ּ���.(RŰ�� ���� �����մϴ�.)";
            setText(situationMainText, uiStr);
        }



        //ȯ�� �ٸ� ���� Ȯ��
        if (fracture.isPatientCons && !fracture.didCall119)
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