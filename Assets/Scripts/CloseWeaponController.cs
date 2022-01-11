using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상 클래스
public abstract class CloseWeaponController : MonoBehaviour
{
    // 현재 장책된 Hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격 중?
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    // 필요한 컴포넌트
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                if (CheckObject())
                {
                    if (currentCloseWeapon.isAxe && hitInfo.transform.tag == "Tree")
                    {
                        StartCoroutine(player.TreeLookCoroutine(hitInfo.transform.GetComponent<TreeComponent>().GetTreeCenterPosition()));
                        StartCoroutine(AttackCoroutine("Chop",
                                                        currentCloseWeapon.workDelayA,
                                                        currentCloseWeapon.workDelayB,
                                                        currentCloseWeapon.workDelay));
                        return;
                    }

                    StartCoroutine(AttackCoroutine("Attack",
                                                    currentCloseWeapon.attackDelayA,
                                                    currentCloseWeapon.attackDelayB,
                                                    currentCloseWeapon.attackDelay));
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine(string _swingType, float _delayA, float _delayB, float _delayC)
    {
        isAttack = true;
        currentCloseWeapon.animator.SetTrigger(_swingType);

        yield return new WaitForSeconds(_delayA);
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(_delayB);
        isSwing = false;

        yield return new WaitForSeconds(_delayC - _delayA - _delayB);

        isAttack = false;
    }

    // 추상 코루틴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
            return true;

        return false;
    }

    // 가상 함수
    // 완성 함수이지만, 추가 편집이 가능한 함수
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

