using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;           // 아이템 유형

    public string itemName;             // 아이템 이름
    [TextArea]
    public string itemDescription;
    public Sprite itemSprite;           // 아이템 스프라이트
    public GameObject itemPrefab;       // 아이템 프리팹

    public string weaponType;           // 무기 유형
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
    }
}
