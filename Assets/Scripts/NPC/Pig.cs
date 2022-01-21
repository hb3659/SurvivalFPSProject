using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
    protected override void Update()
    {
        base.Update();

        if(fov.View() && !isDead)
        {
            Run(fov.GetTargetPosition());
        }
    }

    protected override void ResetAction()
    {
        base.ResetAction();
        RandomAction();
    }

    void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 4);
        // ���, Ǯ���, ���, �ȱ�

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    void Wait()
    {
        currentChaseTime = waitTime;
        Debug.Log("���");
    }
    void Eat()
    {
        currentChaseTime = waitTime;
        animator.SetTrigger("Eat");
        Debug.Log("Ǯ ���");
    }
    void Peek()
    {
        currentChaseTime = waitTime;
        animator.SetTrigger("Peek");
        Debug.Log("���");
    }
}
