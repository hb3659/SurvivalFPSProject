using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputNumber : MonoBehaviour
{
    private bool activated;

    [SerializeField]
    private TextMeshProUGUI previewText;
    [SerializeField]
    private TextMeshProUGUI inputText;
    [SerializeField]
    private TMP_InputField ifText;

    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private ActionController controller;

    void Update()
    {
        if (activated = true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))
                Cancle();
        }
    }

    public void Call()
    {
        go_Base.SetActive(true);
        activated = true;
        ifText.text = "";
        previewText.text = DragSlot.instance.dragSlot.itemCount.ToString();
    }

    public void Cancle()
    {
        activated = false;
        go_Base.SetActive(false);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        DragSlot.instance.SetColor(0);
        int num;

        if (inputText.text != "")
        {
            if (CheckNumber(inputText.text))
            {
                //num = int.Parse(inputText.text);
                int.TryParse(inputText.GetParsedText(), out num);

                if (num > DragSlot.instance.dragSlot.itemCount)
                    // 입력된 수가 소유하고 있는 수보다 크다면
                    // 최대 개수로 맞춘다.
                    num = DragSlot.instance.dragSlot.itemCount;
            }
            else
                // 문자가 입력됐다면 수를 1로 맞춘다.
                num = 1;
        }
        else
            // 아무것도 입력되지 않으면 최대 개수로 맞춘다.
            int.TryParse(previewText.GetParsedText(), out num);

        StartCoroutine(DropItemCoroutine(num));
    }

    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            // 형체가 없는 아이템은 파괴한다.
            if (DragSlot.instance.dragSlot.item.itemPrefab != null)
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, controller.transform.position + controller.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1);

            yield return new WaitForSeconds(.05f);
        }

        DragSlot.instance.dragSlot = null;
        go_Base.SetActive(false);
    }

    private bool CheckNumber(string _argString)
    {
        char[] tempCharArray = _argString.ToCharArray();
        bool isNumber = true;

        for (int i = 0; i < tempCharArray.Length; i++)
        {
            if (tempCharArray[i] >= 48 && tempCharArray[i] <= 57)
                continue;

            isNumber = false;
        }

        return isNumber;
    }
}
