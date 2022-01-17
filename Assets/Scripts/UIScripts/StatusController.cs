using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // ü��
    [SerializeField]
    private int hp;
    private int currentHP;

    // ���¹̳�
    [SerializeField]
    private int sp;
    private int currentSP;

    // ���¹̳� ������
    [SerializeField]
    private int SPIncreseSpeed;

    // ���¹̳� ��ȸ�� ������
    [SerializeField]
    private int spRechargeTime;
    private int currentSPReChargeTime;

    // ���¹̳� ���� ����
    private bool SPUsed;

    // ����
    [SerializeField]
    private int dp;
    private int currentDP;

    // ��� ��ġ
    [SerializeField]
    private int hungry;
    private int currentHungry;

    // ��� �ϰ� �ӵ�
    [SerializeField]
    private int hungryDecreseTime;
    private int currentHungryDecreseTime;

    // ���� �ϰ� �ӵ�
    [SerializeField]
    private int thirstyDecreseTime;
    private int currentThirstyDecreseTime;

    // ���� ��ġ
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // �ʿ��� �̹���
    [SerializeField]
    private Image[] imageGroup;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = hp;
        currentDP = dp;
        currentSP = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();

        GaugeUpdate();
    }

    void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreseTime <= hungryDecreseTime)
                currentHungryDecreseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreseTime = 0;
            }
        }
        else
            Debug.Log("��� ��ġ�� 0 �� �Ǿ����ϴ�.");
    }
    void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreseTime <= thirstyDecreseTime)
                currentThirstyDecreseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreseTime = 0;
            }
        }
        else
            Debug.Log("���� ��ġ�� 0 �� �Ǿ����ϴ�.");
    }

    void GaugeUpdate()
    {
        imageGroup[HP].fillAmount = (float)currentHP / hp;
        imageGroup[DP].fillAmount = (float)currentDP / dp;
        imageGroup[SP].fillAmount = (float)currentSP / sp;
        imageGroup[HUNGRY].fillAmount = (float)currentHungry / hungry;
        imageGroup[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        imageGroup[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void DecreseStamina(int _count)
    {
        SPUsed = true;
        currentSPReChargeTime = 0;

        if (currentSP - _count > 0)
            currentSP -= _count;
        else
            currentSP = 0;
    }

    void SPRechargeTime()
    {
        if (SPUsed)
        {
            if (currentSPReChargeTime < spRechargeTime)
                currentSPReChargeTime++;
            else
                SPUsed = false;
        }
    }

    void SPRecover()
    {
        if (!SPUsed && currentSP < sp)
            currentSP += SPIncreseSpeed;
    }

    public int GetCurrentSP()
    {
        return currentSP;
    }

    public void IncreseHP(int _count)
    {
        if (currentHP + _count < hp)
            currentHP += _count;
        else
            currentHP = hp;
    }

    public void DecreseHP(int _count)
    {
        currentHP -= _count;

        if (currentHP <= 0)
            Debug.Log("ĳ������ HP �� 0 �� �Ǿ����ϴ�.");
    }

    public void IncreseSP(int _count)
    {
        if (currentSP + _count < sp)
            currentSP += _count;
        else
            currentSP = sp;
    }

    public void IncreseDP(int _count)
    {
        if (currentDP + _count < dp)
            currentDP += _count;
        else
            currentDP = dp;
    }

    public void DecreseDP(int _count)
    {
        if (currentDP > 0)
        {
            DecreseDP(_count);
            return;
        }

        currentDP -= _count;

        if (currentDP <= 0)
            Debug.Log("ĳ������ DP �� 0 �� �Ǿ����ϴ�.");
    }

    public void IncreseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    public void IncreseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }
}
