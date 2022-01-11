using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;                      // 습득 가능한 최대 거리

    private bool pickupActivated = false;     // 습득 가능할 시 true

    private RaycastHit hitInfo;               // 충돌체 정보 저장

    // 아이템 레이어에만 반응하도록 레이어 마스크를 설정
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private TextMeshProUGUI actionText;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
        }
        CheckItem();
        TryAction();
    }

    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickup();
        }
    }

    void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                AppearItemInfo();
            }
        }
        else
            DisappearItemInfo();
    }

    void CanPickup()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "을 획득했습니다.");
                Destroy(hitInfo.transform.gameObject);
                DisappearItemInfo();
            }
        }
    }

    void AppearItemInfo()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }

    void DisappearItemInfo()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
