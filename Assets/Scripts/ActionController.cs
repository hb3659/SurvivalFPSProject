using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;                      // ���� ������ �ִ� �Ÿ�

    private bool pickupActivated = false;     // ���� ������ �� true

    private RaycastHit hitInfo;               // �浹ü ���� ����

    // ������ ���̾�� �����ϵ��� ���̾� ����ũ�� ����
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
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "�� ȹ���߽��ϴ�.");
                Destroy(hitInfo.transform.gameObject);
                DisappearItemInfo();
            }
        }
    }

    void AppearItemInfo()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " ȹ�� " + "<color=yellow>" + "(E)" + "</color>";
    }

    void DisappearItemInfo()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
