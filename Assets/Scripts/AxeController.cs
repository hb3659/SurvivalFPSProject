using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    // 활성화 여부
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
                if (hitInfo.transform.tag == "Grass")
                    hitInfo.transform.GetComponent<Grass>().Damage();
                else if (hitInfo.transform.tag == "Tree")
                    hitInfo.transform.GetComponent<TreeComponent>().Chop(hitInfo.point, transform.eulerAngles.y);

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
