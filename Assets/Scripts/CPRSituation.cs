using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRSituation : MonoBehaviour
{
    public GameObject player; // �÷��̾�(CPR�� �ϴ� ���)
    public GameObject patient; // ȯ��(�������� CPR �޴� ���)

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ��Ȳ ����
    public void startSituation()
    {
        patient.SetActive(true);
        Debug.Log("CPR Situation Start");
    }
}
