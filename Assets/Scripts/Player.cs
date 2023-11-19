using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1.3f; // �̵� �ӵ�
    public float jumpPower = 3.5f; // ������ �� ������ٵ� �������� ��
    public float camRotateSpeed = 300f; // ���콺 �̵��� ���� ī�޶� ȸ�� �ӵ�

    public GameObject camera; // �÷��̾� ������ ī�޶�

    private Rigidbody rigidbody;

    private float xRotate, yRotate, xRotateMove, yRotateMove; // ���콺�� ���� ī�޶� ȸ���� ���õ� ������


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        doMove(); // �÷��̾� �̵�
        doRotateCamera(); // ���콺�� ī�޶� ȸ��
    }

    // �÷��̾� �̵� �Լ�
    void doMove()
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
}
