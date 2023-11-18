using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRSituation : MonoBehaviour
{
    public GameObject player; // 플레이어(CPR을 하는 사람)
    public GameObject patient; // 환자(쓰러져서 CPR 받는 사람)

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 상황 시작
    public void startSituation()
    {
        patient.SetActive(true);
        Debug.Log("CPR Situation Start");
    }
}
