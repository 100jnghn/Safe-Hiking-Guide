using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRSituation : MonoBehaviour
{
    public float patientMoveSpeed = 1f; // ȯ�ڰ� ������ �� ���� �ӵ�

    public GameObject player; // �÷��̾�(CPR�� �ϴ� ���)
    public GameObject patient; // ȯ��(�������� CPR �޴� ���)

    public bool isPatientDown; // ȯ�ڰ� ������ ��Ȳ�ΰ���?

    private float time = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ��Ȳ ���� // ���� �Լ� ���� ���ð��� ���� �ڷ�ƾ ���(Test)
    public IEnumerator startSituation()
    {
        patient.SetActive(true);
        Debug.Log("CPR Situation Start");

        yield return new WaitForSeconds(1f); // 1�� ��� �� ����


        // ���⿡ ȯ�ڰ� �ȴ� �ִϸ��̼� �߰�


        while(time <= 2f) // 2�ʵ��� ���� (ȯ�ڰ� ������ ���ϴ�)
        {
            moveForwardPatient();

            time += Time.deltaTime;
            //Debug.Log(time.ToString()); ;
            yield return null;
        }
        time = 0f;


        isPatientDown = true;
        // ���⿡ ȯ�ڰ� �������� �ִϸ��̼� �߰�


        yield return null;
    }

    // ȯ�ڸ� ������ �̵���Ű�� �Լ�
    private void moveForwardPatient()
    {
        patient.transform.Translate(Vector3.forward * patientMoveSpeed * Time.deltaTime); // ȯ�� ������ �̵�
    }
}
