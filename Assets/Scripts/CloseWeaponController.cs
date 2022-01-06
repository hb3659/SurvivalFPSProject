using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �߻� Ŭ����
public abstract class CloseWeaponController : MonoBehaviour
{
    // ���� ��å�� Hand�� Ÿ�� ����
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // ���� ��?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                // �ڷ�ƾ ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.animator.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        // ���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);

        isAttack = false;
    }

    // �߻� �ڷ�ƾ
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
            return true;

        return false;
    }

    // ���� �Լ�
    // �ϼ� �Լ�������, �߰� ������ ������ �Լ�
    public virtual void CloseWeaponChange(CloseWeapon _hand)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _hand;

        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>()
            ;
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.animator;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}

