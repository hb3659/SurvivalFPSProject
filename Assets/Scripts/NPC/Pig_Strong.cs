using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig_Strong : StrongAnimal
{
    protected override void Update()
    {
        base.Update();

        if (fov.View() && !isDead && !isAttacking)
        {
            // �ڷ�ƾ �ߺ� ���� ����
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }
}
