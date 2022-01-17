using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 공유 자원, 클래스 변수 == 정적 변수
    // 무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    //  현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnimator;

    // 현재 무기의 타입
    [SerializeField]
    private string currentWeaponType;

    // 무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;
    // 무기 교체가 완전히 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    // 무기 종류들 전부 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;
    [SerializeField]
    private AxeController axeController;
    [SerializeField]
    private PickaxeController pickaxeController;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
            gunDictionary.Add(guns[i].gunName, guns[i]);
        for (int i = 0; i < hands.Length; i++)
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        for (int i = 0; i < axes.Length; i++)
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        for (int i = 0; i < pickaxes.Length; i++)
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnimator.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CanclePreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    void CanclePreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                gunController.CancleFineSight();
                gunController.CancleReload();
                GunController.isActivated = false;
                break;
            case "HAND":
                HandController.isActivated = false;
                break;
            case "AXE":
                AxeController.isActivated = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivated = false;
                break;
        }
    }

    void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            gunController.GunChange(gunDictionary[_name]);
        else if(_type == "HAND")
            handController.CloseWeaponChange(handDictionary[_name]);
        else if (_type == "AXE")
            axeController.CloseWeaponChange(axeDictionary[_name]);
        else if (_type == "PICKAXE")
            pickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
    }
}
