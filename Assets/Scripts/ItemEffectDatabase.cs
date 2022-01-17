using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;     // 아이템 이름
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY 만 가능합니다.")]
    public string[] parts;         // 부위
    public int[] num;             // 수치
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    // 필요한 컴포넌트
    [SerializeField]
    private StatusController statusController;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private SlotToolTip slotToolTip;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void ShowToolTip(Item _item, Vector3 _position)
    {
        slotToolTip.ShowToolTip(_item, _position);
    }

    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            // 장착
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].parts.Length; y++)
                    {
                        switch (itemEffects[x].parts[y])
                        {
                            case HP:
                                statusController.IncreseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                statusController.IncreseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                statusController.IncreseDP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                statusController.IncreseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                statusController.IncreseThirsty(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위");
                                Debug.Log("HP, SP, DP, HUNGRY, THIRSTY, SATISFY 만 가능합니다.");
                                break;
                        }

                        Debug.Log(_item.itemName + " 을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase에 일치하는 itemName이 없습니다.");
        }
    }
}
