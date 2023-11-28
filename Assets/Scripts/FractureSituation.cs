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