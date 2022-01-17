using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField]
    private Slot[] quickSlots;              // Äü½½·Ôµé
    [SerializeField]
    private Transform tf_parent;            // Äü½½·ÔÀÇ ºÎ¸ð °´Ã¼

    private int selectedSlot;               // ¼±ÅÃµÈ ½½·Ô (0~7)

    // ÇÊ¿äÇÑ ÄÄÆ÷³ÍÆ®
    [SerializeField]
    private GameObject go_SelectedImage;    // ¼±ÅÃµÈ Äü½½·ÔÀÇ ÀÌ¹ÌÁö
    [SerializeField]
    private WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
    }

    void TryInputNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            ChangeSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            ChangeSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            ChangeSlot(7);
    }

    void ChangeSlot(int _num)
    {
        SelectedSlot(_num);

        Execute();
    }

    void SelectedSlot(int _num)
    {
        // ¼±ÅÃµÈ ½½·Ô
        selectedSlot = _num;

        // ¼±ÅÃµÈ ½½·ÔÀ¸·Î ÀÌ¹ÌÁö ÀÌµ¿
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    void Execute()
    {
        if (quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)
                StartCoroutine(weaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used) 
                StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "Nothing"));
            else
                StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "Nothing"));
        }
        else
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "Nothing"));
        }
    }
}
