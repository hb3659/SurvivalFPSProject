using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // ũ�ν���� ���¿� ���� ���� ��Ȯ��
    private float gunAccuracy;

    // ũ�ν���� ��Ȱ��ȭ�� ���� �θ� ��ü
    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField]
    private GunController gunController;

    public void WalkingAnimation(bool flag)
    {
        animator.SetBool("Walking", flag);
        WeaponManager.currentWeaponAnimator.SetBool("Walk", flag);
    }
    public void RunningAnimation(bool flag)
    {
        animator.SetBool("Running", flag);
        WeaponManager.currentWeaponAnimator.SetBool("Run", flag);
    }
    public void JumpAnimation(bool flag)
    {
        animator.SetBool("Running", flag);
    }
    public void CrouchingAnimation(bool flag)
    {
        animator.SetBool("Crouching", flag);
    }
    public void FineSightAnimation(bool flag)
    {
        animator.SetBool("FineSight", flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
            animator.SetTrigger("WalkFire");
        else if (animator.GetBool("Crouching"))
            animator.SetTrigger("CrouchFire");
        else
            animator.SetTrigger("IdleFire");
    }

    public float GetAccuray()
    {
        if (animator.GetBool("Walking"))
            gunAccuracy = .06f;
        else if (animator.GetBool("Crouching"))
            gunAccuracy = .015f;
        else if (gunController.GetFineSightMode())
            gunAccuracy = .001f;
        else
            gunAccuracy = .035f;

        return gunAccuracy;
    }
}
