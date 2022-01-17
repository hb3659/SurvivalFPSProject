using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    // 체력
    [SerializeField]
    private int hp;
    private int currentHP;

    // 스태미너
    [SerializeField]
    private int sp;
    private int currentSP;

    // 스태미너 증가량
    [SerializeField]
    private int SPIncreseSpeed;

    // 스태미너 재회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSPReChargeTime;

    // 스태미너 감소 여부
    private bool SPUsed;

    // 방어력
    [SerializeField]
    private int dp;
    private int currentDP;

    // 허기 수치
    [SerializeField]
    private int hungry;
    private int currentHungry;

    // 허기 하강 속도
    [SerializeField]
    private int hungryDecreseTime;
    private int currentHungryDecreseTime;

    // 갈증 하강 속도
    [SerializeField]
    private int thirstyDecreseTime;
    private int currentThirstyDecreseTime;

    // 갈증 수치
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // 필요한 이미지
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
            Debug.Log("허기 수치가 0 이 되었습니다.");
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
            Debug.Log("갈증 수치가 0 이 되었습니다.");
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
            Debug.Log("캐릭터의 HP 가 0 이 되었습니다.");
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
            Debug.Log("캐릭터의 DP 가 0 이 되었습니다.");
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
