using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1.3f; // �̵� �ӵ�
    public float jumpPower = 3.5f; // ������ �� ������ٵ� �������� ��
    public float camRotateSpeed = 300f; // ���콺 �̵��� ���� ī�޶� ȸ�� �ӵ�
    public float inputTimer = 0f; // RŰ �Է� �ð��� ���� // �ൿ�� ���� �׼� �Ǻ� �뵵

    public bool canMove; // �÷��̾ ������ �� �ִ� �����ΰ���?
    public bool isSit; // �÷��̾ �ɾ��ֳ���?

    private Rigidbody rigidbody;

    public Camera mainCamera;
    private Ray ray;
    private RaycastHit hit; // Raycast �浹 ����
    GameObject gazeTimerObject; // ��ü�� ����Ű�� �ִ��� Ȯ���ϴ� ����
    float gazeTimer = 0f; // ����Ű�� �ִ� �ð��� �����ϴ� ����

    public GameObject camera; // �÷��̾� ������ ī�޶�
    public GameObject cprSituation; // CPR ��Ȳ�� ���� (Start Position CPR)
    public GameObject fractureSituation;// Fracture ��Ȳ�� ���� (Start Position Fracture)
    public GameObject rayPosition; // ray�� �߻��� ��ġ
    public GameObject rayCollObject; // ray�� �浹�� ������Ʈ
    public GameObject standPosition; // �÷��̾ ������ �� ������ �Ǵ� ��ġ
    public GameObject sitPosition; // �÷��̾ �ɾ��� �� ������ �Ǵ� ��ġ
    private CPRSituation cpr; // cprSituation ������Ʈ�� Component�� CPRSituation ��ũ��Ʈ�� ������
    private FractureSituation fracture;// fractureSituation ������Ʈ�� Component�� FractureSituation ��ũ��Ʈ�� ������

    private float xRotate, yRotate, xRotateMove, yRotateMove; // ���콺�� ���� ī�޶� ȸ���� ���õ� ������
    public float rayDistance = 15f; // Ray�� ����

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cpr = cprSituation.GetComponent<CPRSituation>();
        fracture = fractureSituation.GetComponent<FractureSituation>();

        Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ȭ�� �߾ӿ� ����
        Cursor.visible = false; // Ŀ���� ������ �ʰ� ��
    }

    void Update()
    {
        doMove(); // �÷��̾� �̵�
        doRotateCamera(); // ���콺�� ī�޶� ȸ��
        doAction(); // �׼� Ű �Է�(RŰ)
        shotRay(); // ���̸� ����ִ� �Լ�
        rayCollider();
        doSit(); // �÷��̾ �ɴ� ��� (CŰ �Է�)
        doPress();
    }

    // �÷��̾� �̵� �Լ�
    void doMove()
    {
        if (canMove)
        {
            Vector3 dir = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) // W -> ������ �̵�
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                dir += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S)) // S -> �ڷ� �̵�
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
                dir += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A)) // A -> �������� �̵�
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                dir += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D)) // D -> ���������� �̵�
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                dir += Vector3.right;
            }
            if (Input.GetKeyDown(KeyCode.Space)) // Space -> ����
            {
                rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }    
    }

    // �÷��̾ �ɴ� ���
    void doSit()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isSit)
            {
                camera.transform.position = sitPosition.transform.position;
                isSit = true;
            }
            else
            {
                camera.transform.position = standPosition.transform.position;
                isSit = false;
            }
        }      
    }

    // ī�޶� ȸ���ϴ� �Լ� 
    // ���콺 ���� �̵� -> ī�޶� ȸ�� // ���콺 �¿� �̵� -> ī�޶�, �÷��̾� ȸ��
    void doRotateCamera()
    {
        xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * camRotateSpeed; // ���콺 ���� �̵�
        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * camRotateSpeed; // ���콺 �¿� �̵�

        xRotate = camera.transform.eulerAngles.x + xRotateMove; // ���� ȸ��
        //xRotate = Mathf.Clamp(xRotate, -90, 90); // ��, �Ʒ� ������ ����

        yRotate = transform.eulerAngles.y + yRotateMove; // �¿� ȸ��

        camera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0); // ī�޶� ȸ��
        transform.eulerAngles = new Vector3(0, yRotate, 0); // �÷��̾� ȸ��
    }

    void shotRay()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawLine(ray.origin, hit.point, Color.red);
    }

    // ray�� �浹�� ó���ϴ� �Լ�
    void rayCollider()
    {
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            rayCollObject = hit.transform.gameObject; // rayCollObject�� ray�� �浹�� ������Ʈ�� ����
        }
    }

    // R Ű�� ���� �׼� (3�� �̻� ������ true)
    public bool doAction()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.R))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action =  true;
            }
        }
        if (Input.GetKeyUp(KeyCode.R)) // R Ű���� ���� inputTimer �ʱ�ȭ
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
    }

    // P Ű�� ���� ���� (3�� �̻� ������ true)
    public bool doPress()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.P))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.P)) // P Ű���� ���� inputTimer �ʱ�ȭ
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
    }


    //�������ϱ�
    public bool doIcing()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.I))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.I)) // I Ű���� ���� inputTimer �ʱ�ȭ
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
    }




    private void OnCollisionEnter(Collision collision)
    {
        // ȯ�ڿ��� �浹 �߻�
        if (collision.gameObject.tag == "Patient")
        {
            if (cpr.isPatientDown & !cpr.isPatientCons) // ȯ�ڰ� ������ ����
            {
                cpr.isPatientCons = true; // ȯ���� �ǽ��� Ȯ���ؾ� �ϴ� ���� On
            }
            if (fracture.isPatientDown & !fracture.isPatientCons) // ȯ�ڰ� ������ ����
            {
                fracture.isPatientCons = true; // ������û�� �ؾ��ϴ� ���� On
            }
        }
    }
}
