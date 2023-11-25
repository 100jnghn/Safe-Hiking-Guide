using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hellicopter : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject propeller; // �︮������ �����緯
    public GameObject propellerBack; // �︮���� ���� �����緯

    public Transform startPos; // �︮���� ���� ��ġ
    public Transform destinationCPR; // CPR ��Ȳ ���� ��ġ

    public float rotateSpeed = 300f; // �����緯 ȸ�� �ӵ�

    public bool canMove; // true�� �Ǹ� ��Ⱑ �̵��մϴ�.

    void Start()
    {
        setStartPos();
    }

    void Update()
    {
        moveHellicopter();
        spinPropeller();
    }

    // ��⸦ ��� ��ġ�� �̵���Ŵ
    public void setStartPos()
    {
        transform.position = startPos.position; // ��ġ ����
        transform.rotation = startPos.rotation; // rotation ����
    }

    // �����緯 ȸ�� + ��� �Ҹ�
    public void spinPropeller()
    {
        propeller.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        propellerBack.transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
    }

    // �︮���͸� ��ġ�� �̵���Ŵ
    public void moveHellicopter()
    {
        switch (gameManager.mode)
        {
            case GameManager.Mode.Nothing:
                break;

            case GameManager.Mode.CPR:
                transform.position = Vector3.Lerp(transform.position, destinationCPR.position, 0.003f);
                break;
        }
        
    }
}
