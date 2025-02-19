using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hellicopter : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject hellicopter; // �︮���� ��ü
    public GameObject propeller; // �︮������ �����緯
    public GameObject propellerBack; // �︮���� ���� �����緯

    public Transform startPos; // �︮���� ���� ��ġ
    public Transform destinationCPR; // CPR ��Ȳ ���� ��ġ
    public Transform destinationFracture; // ���� ��Ȳ ���� ��ġ
    public Transform destinationSnake; // �칰�� ��Ȳ ���� ��ġ
    public Transform destinationBee; // �� ��Ȳ ���� ��ġ

    public AudioSource sound; // �︮���� �Ҹ�

    public float rotateSpeed = 300f; // �����緯 ȸ�� �ӵ�

    void Start()
    {
        setStartPos(); // ���� ��ġ�� ����
        sound.Play(); // �Ҹ� ���
    }

    void Update()
    {
        moveHellicopter(); // ��ǥ ��ġ�� �̵�
        spinPropeller(); // �����緯 ȸ��
    }

    // ��⸦ ��� ��ġ�� �̵���Ŵ
    public void setStartPos()
    {
        transform.position = startPos.position; // ��ġ ����
        transform.rotation = startPos.rotation; // rotation ����
    }

    // �����緯 ȸ��
    public void spinPropeller()
    {
        propeller.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed); // �����緯 ȸ��
        propellerBack.transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed); // ���� �����緯 ȸ��
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

            case GameManager.Mode.Fracture:
                transform.position = Vector3.Lerp(transform.position, destinationFracture.position, 0.003f);
                break;

            case GameManager.Mode.Snake:
                transform.position = Vector3.Lerp(transform.position, destinationSnake.position, 0.003f);
                break;

            case GameManager.Mode.Bee:
                transform.position = Vector3.Lerp(transform.position, destinationBee.position, 0.003f);
                break;
        }

    }

}
