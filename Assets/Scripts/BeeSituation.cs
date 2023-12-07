using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSituation : MonoBehaviour
{
    public Transform playerStartPos; // 플레이어 시작 위치
    public Transform beeStartPos; // 벌 시작 위치
    public Transform beeTarget; // 벌이 쏠 위치

    public GameObject player; // 플레이어
    public GameObject beePlayerCam; // 플레이어 시점 카메라
    public GameObject bee; // 벌 프리팹 객체 (instantiate 해서 생성)

    public Animator beeAnimator; // 벌 애니메이터
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 벌이 쏠 위치로 이동
    private void beeMoveToTarget()
    {
        bee.transform.position = Vector3.Lerp(transform.position, beeTarget.position, 0.003f);
    }

    // 벌이 플레이어 공격
    private void beeAttack()
    {
        //beeAnimator.SetTrigger("");
    }

    // 시작 위치로 이동
    private void setPosition()
    {
        player.transform.position = playerStartPos.position;
    }


    public IEnumerator startSituation()
    {
        setPosition();

        // 시점 변환
        beePlayerCam.SetActive(true);

        bee = Instantiate(bee, beeStartPos); // 벌 생성
        yield return new WaitForSeconds(1f); // 1초 대기

        beeMoveToTarget(); // 벌이 앞으로 이동
        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        beeAttack();
        yield return new WaitForSeconds(0.5f); // 0.5초 대기



        yield return null;
    }
}
