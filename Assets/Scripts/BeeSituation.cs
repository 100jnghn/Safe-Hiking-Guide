using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSituation : MonoBehaviour
{
    public Transform playerStartPos; // �÷��̾� ���� ��ġ
    public Transform beeStartPos; // �� ���� ��ġ
    public Transform beeTarget; // ���� �� ��ġ

    public GameObject player; // �÷��̾�
    public GameObject beePlayerCam; // �÷��̾� ���� ī�޶�
    public GameObject bee; // �� ������ ��ü (instantiate �ؼ� ����)

    public Animator beeAnimator; // �� �ִϸ�����
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ���� �� ��ġ�� �̵�
    private void beeMoveToTarget()
    {
        bee.transform.position = Vector3.Lerp(transform.position, beeTarget.position, 0.003f);
    }

    // ���� �÷��̾� ����
    private void beeAttack()
    {
        //beeAnimator.SetTrigger("");
    }

    // ���� ��ġ�� �̵�
    private void setPosition()
    {
        player.transform.position = playerStartPos.position;
    }


    public IEnumerator startSituation()
    {
        setPosition();

        // ���� ��ȯ
        beePlayerCam.SetActive(true);

        bee = Instantiate(bee, beeStartPos); // �� ����
        yield return new WaitForSeconds(1f); // 1�� ���

        beeMoveToTarget(); // ���� ������ �̵�
        yield return new WaitForSeconds(0.5f); // 0.5�� ���

        beeAttack();
        yield return new WaitForSeconds(0.5f); // 0.5�� ���



        yield return null;
    }
}
