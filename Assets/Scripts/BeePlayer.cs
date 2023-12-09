using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BeePlayer : MonoBehaviour
{
    public GameObject camera;

    public BeeSituation beeSituation;

    public float camRotateSpeed = 300f; // ���콺 �̵��� ���� ī�޶� ȸ�� �ӵ�
    public float inputTimer = 0f; // QŰ �Է� �ð��� ���� // �ൿ�� ���� �׼� �Ǻ� �뵵
    private float xRotate, yRotate, xRotateMove, yRotateMove; // ���콺�� ���� ī�޶� ȸ���� ���õ� ������

    void Start()
    {
        
    }

    void Update()
    {
        doRotateCamera();
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

    // ! Ű�� ���� �׼� (3�� �̻� ������ true)
    public bool doAction()
    {
        bool action = false;
        if (Input.GetKey(KeyCode.Q))
        {
            inputTimer += Time.deltaTime;
            if (inputTimer > 3f)
            {
                action = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q)) // R Ű���� ���� inputTimer �ʱ�ȭ
        {
            action = false;
            inputTimer = 0f;
        }
        return action;
    }
}
