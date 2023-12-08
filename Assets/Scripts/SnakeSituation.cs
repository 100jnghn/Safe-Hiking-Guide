using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSituation : MonoBehaviour
{

    public float patientMoveSpeed = 1f; // ȯ�ڰ� ������ �� ���� �ӵ�

    public GameObject player; // �÷��̾�
    public GameObject patient; // ȯ��

    public Transform playerStartPos; // �÷��̾� ���� ��ġ
    public Transform patientStartPos; // ȯ�� ���� ��ġ
    public bool isPatientDown;  // ȯ�ڰ� ������ ��Ȳ�ΰ���?
    public bool isPatientCons;  // ȯ�� �ǽ��� �ľ��ؾ� �ϳ���?
    public bool didPatientCons; // ȯ�� �ǽ��� �ľ� �Ϸ��ߴ���?
    public bool isCall119;
    public bool didCall119; // �ٸ� ������� 119 �ҷ��޶�� ��û�ߴ���?
    public bool isCalmDown;//ȯ�� ������Ű��
    public bool didCalmDown;
    public bool isRemove;//��ȯ�� �����ϴ� �� ����
    public bool didRemove;
    public bool isDown;//���� ���� ���庸�� �Ʒ���
    public bool didDown;
    public bool isTie;//�״� ��� ����
    public bool didTie;

    public bool finishSnake; // ��Ȳ ����

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


    // ��Ȳ ���� // ���� �Լ� ���� ���ð��� ���� �ڷ�ƾ ���(Test)
    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("Snake Situation Start");

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
        capsuleCollider.center = new Vector3(0f, 0.3f, 0f); //�߽� ��ġ ����
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
