using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivated = true;

    void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.animator;
    }

    void Update()
    {
        if (isActivated == true)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.tag == "Twig")
                    hitInfo.transform.GetComponent<Twig>().Damage(transform);

                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }

            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _axe)
    {
        base.CloseWeaponChange(_axe);

        isActivated = true;
    }
}
