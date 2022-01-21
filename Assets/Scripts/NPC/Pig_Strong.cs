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
            // 코루틴 중복 실행 방지
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }
}
