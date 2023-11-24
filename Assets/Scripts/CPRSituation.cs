using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRSituation : MonoBehaviour
{
    public float patientMoveSpeed = 1f; // ȯ�ڰ� ������ �� ���� �ӵ�

    public GameObject player; // �÷��̾�(CPR�� �ϴ� ���)
    public GameObject patient; // ȯ��(�������� CPR �޴� ���)

    public bool isPatientDown;  // ȯ�ڰ� ������ ��Ȳ�ΰ���?
    public bool isPatientCons;  // ȯ�� �ǽ��� �ľ��ؾ� �ϳ���?
    public bool didPatientCons; // ȯ�� �ǽ��� �ľ� �Ϸ��ߴ���?
    public bool isHelpOther;    // �ٸ� ������� ������ ��û�ؾ� �ϴ� ��Ȳ?
    public bool didCall119; // �ٸ� ������� 119 �ҷ��޶�� ��û�ߴ���?
    public bool didCallAED; // �ٸ� ������� AED ���ٴ޶�� ��û�ߴ���?
    public bool isChestPress; // ���� �й��� �ؾ� �ϴ� ����?

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

    // ��Ȳ ���� // ���� �Լ� ���� ���ð��� ���� �ڷ�ƾ ���(Test)
    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("CPR Situation Start");

        yield return new WaitForSeconds(2f); // 2�� ��� �� ����


        // ���⿡ ȯ�ڰ� �ȴ� �ִϸ��̼� �߰�
        patientAnimator.SetBool("isWalking", true);



        while (time <= 3f) // 3�ʵ��� ���� (ȯ�ڰ� ������ ���ϴ�)
        {
            moveForwardPatient();

            time += Time.deltaTime;
            //Debug.Log(time.ToString()); ;
            yield return null;
        }
        time = 0f;

        isPatientDown = true;

        // ���⿡ ȯ�ڰ� �������� �ִϸ��̼� �߰�
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