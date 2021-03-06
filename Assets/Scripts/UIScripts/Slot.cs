using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;               // ȹ???? ??????
    public int itemCount;           // ȹ???? ???????? ????
    public Image itemImage;         // ?????? ?̹???

    // ?ʿ??? ??????Ʈ
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    [SerializeField]
    private RectTransform baseRect;          // ?κ??丮 ????
    [SerializeField]
    private RectTransform quickSlotBaseRect;    // ?????? ????
    private InputNumber inputNumber;
    private ItemEffectDatabase itemEffectDatabase;

    void Start()
    {
        inputNumber = FindObjectOfType<InputNumber>();
        itemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }

    // ?̹??? ?????? ????
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // ?????? ȹ??
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemSprite;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1f);
    }

    // ?????? ???? ????
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // ???? ?ʱ?ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                // ?Ҹ?
                itemEffectDatabase.UseItem(item);
                if (item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1);
            }
        }
    }
    // ?ٸ? ???? ?????? ?巡?װ? ?????? ??
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }

    // ?巡?װ? ?????? ?? ȣ??
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!((DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin &&
            DragSlot.instance.transform.localPosition.x < baseRect.rect.xMax &&
            DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin &&
            DragSlot.instance.transform.localPosition.y < baseRect.rect.yMax) || 
            (DragSlot.instance.transform.localPosition.x > quickSlotBaseRect.rect.xMin &&
            DragSlot.instance.transform.localPosition.x < quickSlotBaseRect.rect.xMax &&
            DragSlot.instance.transform.localPosition.y > quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMax &&
            DragSlot.instance.transform.localPosition.y < quickSlotBaseRect.transform.localPosition.y - quickSlotBaseRect.rect.yMin)))
        {
            // ?????? ????
            if (DragSlot.instance.dragSlot != null)
                inputNumber.Call();
        }
        else
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }

    // ???콺?? ???Կ? ??? ?? ?ߵ?
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            itemEffectDatabase.ShowToolTip(item, transform.position);
    }

    // ???콺?? ???Կ??? ???? ???? ?? ?ߵ?
    public void OnPointerExit(PointerEventData eventData)
    {
        itemEffectDatabase.HideToolTip();
    }
}
