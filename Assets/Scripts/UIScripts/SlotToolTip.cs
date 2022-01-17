using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private TextMeshProUGUI text_itemName;
    [SerializeField]
    private TextMeshProUGUI text_itemDesc;
    [SerializeField]
    private TextMeshProUGUI text_HowToUse;

    public void ShowToolTip(Item _item, Vector3 _position)
    {
        go_Base.SetActive(true);
        _position += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * .7f, -go_Base.GetComponent<RectTransform>().rect.height * .7f, 0f);
        go_Base.transform.position = _position;

        text_itemName.text = _item.itemName;
        text_itemDesc.text = _item.itemDescription;

        if (_item.itemType == Item.ItemType.Equipment)
            text_HowToUse.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            text_HowToUse.text = "우클릭 - 먹기";
        else
            text_HowToUse.text = "";
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
