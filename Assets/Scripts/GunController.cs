using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivated = false;

    [SerializeField]
    private Gun currentGun;             // 현재 장착된 총

    private float currentFireRate;      // 연사 속도 계산

    // 상태 변수
    private bool isReload;
    public bool isFineSightMode;

    private Vector3 originPos;          // 원래의 포지션 값

    private new AudioSource audio;      // 효과음 재생
    private RaycastHit hitInfo;         // 레이저 충돌 정보 받아오기

    // 필요 컴포넌트
    [SerializeField]
    private Camera crossHairCam;        // 카메라 시점의 정 가운데를 맞추기 위한 카메라
    [SerializeField]
    private Crosshair crosshair;

    // 피격 이펙트
    [SerializeField]
    private GameObject hitEffectPrefab;

    void Start()
    {
        originPos = Vector3.zero;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isActivated == true)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    // 연사 속도 재계산
    void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    // 발사 시도
    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
            Fire();
    }

    // 발사 전 계산
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

    // 발사 후 계산
    void Shoot()
    {
        crosshair.FireAnimation();

        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;   // 연사 속도 재계산
        PlaySFX(currentGun.fireSound);
        currentGun.muzzleFlash.Play();

        Hit();

        // 총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetorActionCoroutine());
    }

    void Hit()
    {
        // 총알을 오브젝트 풀링으로 생성하면 성능상 유리한 코드가 되지만,
        // 해당 프로젝트에서는 쏘는대로 맞추는 것으로 한다.

        // localPosition이 아닌 이유는 절대적인 위치를 알아야하기 때문
        // (자식으로 들어가기 때문에 어디서든 해당 오브젝트의 위치는 같다.)
        if(Physics.Raycast(crossHairCam.transform.position, crossHairCam.transform.forward + 
            new Vector3(Random.Range(-crosshair.GetAccuray() - currentGun.accuracy, crosshair.GetAccuray() + currentGun.accuracy),
                        Random.Range(-crosshair.GetAccuray() - currentGun.accuracy, crosshair.GetAccuray() + currentGun.accuracy),
                        0f),
            out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            // 2초 뒤에 프리팹 삭제
            Destroy(clone, 2f);
        }
    }

    // 재장선 시도
    void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancleFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancleReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // 재장전
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

    // 정조준 시도
    void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
            FineSight();
    }

    // 정조준 취소
    public void CancleFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    // 정조준 로직 가동
    void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.animator.SetBool("FineSightMode", isFineSightMode);
        crosshair.FineSightAnimation(isFineSightMode);

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

    // 정조준 활성화
    IEnumerator FindSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.FineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, .2f);
            yield return null;
        }
    }

    // 정조준 비활성화
    IEnumerator FindSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, .2f);
            yield return null;
        }
    }

    // 반동 코루틴
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

    // 효과음 재생
    void PlaySFX(AudioClip _clip)
    {
        audio.clip = _clip;
        audio.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;

        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>()
            ;
        WeaponManager.currentWeaponAnimator = currentGun.animator;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);

        isActivated = true;
    }
}
