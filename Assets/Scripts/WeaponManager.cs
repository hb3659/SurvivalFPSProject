using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // ���� �ڿ�, Ŭ���� ���� == ���� ����
    // ���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon = false;

    //  ���� ����� ���� ������ �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnimator;

    // ���� ������ Ÿ��
    [SerializeField]
    private string currentWeaponType;

    // ���� ��ü ������
    [SerializeField]
    private float changeWeaponDelayTime;
    // ���� ��ü�� ������ ���� ����
    [SerializeField]
    private float changeWeaponEndDelayTime;

    // ���� ������ ���� ����
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;

    // ���� �������� ���� ���� ������ �����ϵ��� ����
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();

    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;
    [SerializeField]
    private AxeController axeController;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
            gunDictionary.Add(guns[i].gunName, guns[i]);
        for (int i = 0; i < hands.Length; i++)
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        for (int i = 0; i < axes.Length; i++)
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "Nothing"));
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
        }
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
    }
}
