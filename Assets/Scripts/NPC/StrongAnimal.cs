using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected int attackDamage;           // 공격 데미지
    [SerializeField]
    protected float attackDelay;            // 공격 딜레이
    [SerializeField]
    protected LayerMask targetMask;         // 타겟 마스크

    [SerializeField]
    protected float chaseTime;              // 총 추격 시간
    protected float currentChaseTime;       // 계산
    [SerializeField]
    protected float chaseDelayTime;         // 추격 딜레이

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

        // 플레이어를 목적지로 정한다.
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
            // 충분히 가까이 있고,
            if (Vector3.Distance(transform.position, fov.GetTargetPosition()) <= 3f)
            {
                // 눈 앞에 있을 경우
                if (fov.View())
                {
                    Debug.Log("공격 시도");
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
        // 코루틴이 계속 도는 것을 방지
        currentChaseTime = chaseTime;

        yield return new WaitForSeconds(.5f);
        transform.LookAt(fov.GetTargetPosition());

        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(.5f);           // 공격이 적용되기 위한 딜레이

        RaycastHit _hit;
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out _hit, 3, targetMask))
        {
            Debug.Log("플레이어 적중!");
            playerStatus.DecreseHP(attackDamage);
        }
        else
        {
            Debug.Log("공격 빗나감!");
        }

        yield return new WaitForSeconds(attackDelay);
        // 딜레이 시간 이후 공격 중을 false 로 바꿔 추격코드부터 시작하도록 만든다.
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());
    }
}
