using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;           // ������ ����

    public string itemName;             // ������ �̸�
    [TextArea]
    public string itemDescription;
    public Sprite itemSprite;           // ������ ��������Ʈ
    public GameObject itemPrefab;       // ������ ������

    public string weaponType;           // ���� ����
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
    }
}
