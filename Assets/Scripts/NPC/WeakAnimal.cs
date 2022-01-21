using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    public override void Damage(int _damage, Vector3 _targetPosition)
    {
        base.Damage(_damage, _targetPosition);
        if (!isDead)
            Run(_targetPosition);
    }

    public void Run(Vector3 _targetPosition)
    {
        // �ڽŰ� �÷��̾��� �ݴ���� ���� ���Ѵ�.
        destination = new Vector3(transform.position.x - _targetPosition.x, 0f, transform.position.z - _targetPosition.z).normalized;

        currentChaseTime = runTime;
        isWalk = false;
        isRun = true;
        agent.speed = runSpeed;
        animator.SetBool("Run", isRun);
    }
}
