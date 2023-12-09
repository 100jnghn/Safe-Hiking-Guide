using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSituation : MonoBehaviour
{
    public Transform playerStartPos; // �÷��̾� ���� ��ġ
    public Transform beeStartPos; // �� ���� ��ġ
    public Transform beeTarget; // ���� ���ư� ��ġ

    public GameObject player; // �÷��̾�
    public GameObject beePlayerCam; // �÷��̾� ���� ī�޶�
    public GameObject bee; // �� ��ü

    private Animator beeAnimator; // �� �ִϸ�����

    public bool isStart; // ��Ȳ ���� �ߴ���
    public bool isArrived; // ���� �����ߴ���
    public bool isAttacked; // ���� �����ߴ���
    public bool isCall119; // 119 �ҷ�����
    public bool isCard; // ī��� ħ �� ����
    public bool didCard; // ħ ����
    public bool isIcing; // ������ �� ����
    public bool didIcing; // ������ �ߴ���
    public bool isKeepHigh; // ���� ���� ���� ���� �ߴ���
    
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

    // ���� �� ��ġ�� �̵�
    private void beeMoveToTarget()
    {
        bee.transform.position = Vector3.MoveTowards(bee.transform.position, beeTarget.position, 0.02f);
        bee.transform.LookAt(player.transform.position);
    }

    // ���� �÷��̾� ����
    private void beeAttack()
    {
        beeAnimator.SetTrigger("doAttack");
        Debug.Log("Bee Attack");
    }

    // ���� ��ġ�� �̵�
    private void setPosition()
    {
        player.transform.position = playerStartPos.position;
    }


    public IEnumerator startSituation()
    {
        setPosition();
        bee.SetActive(true);
        

        // ���� ��ȯ
        beePlayerCam.SetActive(true);

        yield return new WaitForSeconds(1f); // 1�� ���

        isStart = true;
        yield return new WaitForSeconds(4f); // 4�� ���

        isArrived = true;
        beeAttack();
        yield return new WaitForSeconds(1f); // 0.5�� ���

        isAttacked = true;
        beeAnimator.SetBool("isIdle", true);



        yield return null;
    }
}
