using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;     // ������ �̸�
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY �� �����մϴ�.")]
    public string[] parts;         // ����
    public int[] num;             // ��ġ
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    // �ʿ��� ������Ʈ
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
            // ����
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
                                Debug.Log("�߸��� Status ����");
                                Debug.Log("HP, SP, DP, HUNGRY, THIRSTY, SATISFY �� �����մϴ�.");
                                break;
                        }

                        Debug.Log(_item.itemName + " �� ����߽��ϴ�.");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� itemName�� �����ϴ�.");
        }
    }
}
