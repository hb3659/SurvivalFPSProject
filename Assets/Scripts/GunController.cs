using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private bool isReload;
    private bool isFineSightMode;

    [SerializeField]
    private Vector3 originPos;          // 원래의 포지션 값

    private new AudioSource audio;


    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload) 
            Fire();
    }

    void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancleFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;   // 연사 속도 재계산
        PlaySFX(currentGun.fireSound);
        currentGun.muzzleFlash.Play();

        // 총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetorActionCoroutine());
    }

    void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancleFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.animator.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
    }

    void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
            FineSight();
    }

    public void CancleFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.animator.SetBool("FineSightMode", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FindSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FindSightDeactivateCoroutine());
        }
    }

    IEnumerator FindSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.FineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, .2f);
            yield return null;
        }
    }

    IEnumerator FindSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, .2f);
            yield return null;
        }
    }

    IEnumerator RetorActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.FineSightOriginPos.y, currentGun.FineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - .02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, .4f);

                yield return null;
            }

            // 원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, .1f);

                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.FineSightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - .02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, .4f);

                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != currentGun.FineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, .1f);

                yield return null;
            }
        }
    }

    void PlaySFX(AudioClip _clip)
    {
        audio.clip = _clip;
        audio.Play();
    }
}
