using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{
    // Ȱ��ȭ ����
    public static bool isActivated = false;

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
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }

            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _hand)
    {
        base.CloseWeaponChange(_hand);

        isActivated = true;
    }
}
