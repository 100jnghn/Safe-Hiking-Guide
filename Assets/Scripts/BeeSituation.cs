using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSituation : MonoBehaviour
{
    public Transform playerStartPos; // 플레이어 시작 위치
    public Transform beeStartPos; // 벌 시작 위치
    public Transform beeTarget; // 벌이 날아갈 위치

    public GameObject player; // 플레이어
    public GameObject beePlayerCam; // 플레이어 시점 카메라
    public GameObject bee; // 벌 객체

    private Animator beeAnimator; // 벌 애니메이터

    public bool isStart; // 상황 시작 했는지
    public bool isArrived; // 벌이 도착했는지
    public bool isAttacked; // 벌이 공격했는지
    public bool isCall119; // 119 불렀는지
    public bool isCard; // 카드로 침 뺄 차례
    public bool didCard; // 침 뺐음
    public bool isIcing; // 냉찜질 할 차례
    public bool didIcing; // 냉찜질 했는지
    public bool isKeepHigh; // 쏘인 부위 높게 유재 했는지
    
    void Start()
    {
        beeAnimator = bee.GetComponent<Animator>();
    }

    void Update()
    {
        if (isStart && !isArrived)
        {
            beeMoveToTarget();
        }
    }

    // 벌이 쏠 위치로 이동
    private void beeMoveToTarget()
    {
        bee.transform.position = Vector3.MoveTowards(bee.transform.position, beeTarget.position, 0.02f);
        bee.transform.LookAt(player.transform.position);
    }

    // 벌이 플레이어 공격
    private void beeAttack()
    {
        beeAnimator.SetTrigger("doAttack");
        Debug.Log("Bee Attack");
    }

    // 시작 위치로 이동
    private void setPosition()
    {
        player.transform.position = playerStartPos.position;
    }


    public IEnumerator startSituation()
    {
        setPosition();
        bee.SetActive(true);
        

        // 시점 변환
        beePlayerCam.SetActive(true);

        yield return new WaitForSeconds(1f); // 1초 대기

        isStart = true;
        yield return new WaitForSeconds(4f); // 4초 대기

        isArrived = true;
        beeAttack();
        yield return new WaitForSeconds(1f); // 0.5초 대기

        isAttacked = true;
        beeAnimator.SetBool("isIdle", true);



        yield return null;
    }
}
