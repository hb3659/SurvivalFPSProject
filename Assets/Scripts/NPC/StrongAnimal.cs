using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected int attackDamage;           // ���� ������
    [SerializeField]
    protected float attackDelay;            // ���� ������
    [SerializeField]
    protected LayerMask targetMask;         // Ÿ�� ����ũ

    [SerializeField]
    protected float chaseTime;              // �� �߰� �ð�
    protected float currentChaseTime;       // ���
    [SerializeField]
    protected float chaseDelayTime;         // �߰� ������

    public override void Damage(int _damage, Vector3 _targetPosition)
    {
        base.Damage(_damage, _targetPosition);
        if (!isDead)
            Chase(_targetPosition);
    }

    public void Chase(Vector3 _targetPosition)
    {
        isChasing = true;
        isRun = true;

        // �÷��̾ �������� ���Ѵ�.
        destination = _targetPosition;

        agent.speed = runSpeed;
        animator.SetBool("Run", isRun);
        agent.SetDestination(destination);
    }
    protected IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;

        while (currentChaseTime < chaseTime)
        {
            Chase(fov.GetTargetPosition());
            // ����� ������ �ְ�,
            if (Vector3.Distance(transform.position, fov.GetTargetPosition()) <= 3f)
            {
                // �� �տ� ���� ���
                if (fov.View())
                {
                    Debug.Log("���� �õ�");
                    StartCoroutine(AttackCoroutine());
                }
            }

            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }

        isChasing = false;
        isRun = false;
        animator.SetBool("Run", isRun);
        agent.ResetPath();
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        agent.ResetPath();
        // �ڷ�ƾ�� ��� ���� ���� ����
        currentChaseTime = chaseTime;

        yield return new WaitForSeconds(.5f);
        transform.LookAt(fov.GetTargetPosition());

        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(.5f);           // ������ ����Ǳ� ���� ������

        RaycastHit _hit;
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out _hit, 3, targetMask))
        {
            Debug.Log("�÷��̾� ����!");
            playerStatus.DecreseHP(attackDamage);
        }
        else
        {
            Debug.Log("���� ������!");
        }

        yield return new WaitForSeconds(attackDelay);
        // ������ �ð� ���� ���� ���� false �� �ٲ� �߰��ڵ���� �����ϵ��� �����.
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());
    }
}
