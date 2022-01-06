using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Ȱ��ȭ ����
    public static bool isActivated = false;

    [SerializeField]
    private Gun currentGun;             // ���� ������ ��

    private float currentFireRate;      // ���� �ӵ� ���

    // ���� ����
    private bool isReload;
    public bool isFineSightMode;

    private Vector3 originPos;          // ������ ������ ��

    private new AudioSource audio;      // ȿ���� ���
    private RaycastHit hitInfo;         // ������ �浹 ���� �޾ƿ���

    // �ʿ� ������Ʈ
    [SerializeField]
    private Camera crossHairCam;        // ī�޶� ������ �� ����� ���߱� ���� ī�޶�
    [SerializeField]
    private Crosshair crosshair;

    // �ǰ� ����Ʈ
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

    // ���� �ӵ� ����
    void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    // �߻� �õ�
    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
            Fire();
    }

    // �߻� �� ���
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

    // �߻� �� ���
    void Shoot()
    {
        crosshair.FireAnimation();

        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;   // ���� �ӵ� ����
        PlaySFX(currentGun.fireSound);
        currentGun.muzzleFlash.Play();

        Hit();

        // �ѱ� �ݵ� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(RetorActionCoroutine());
    }

    void Hit()
    {
        // �Ѿ��� ������Ʈ Ǯ������ �����ϸ� ���ɻ� ������ �ڵ尡 ������,
        // �ش� ������Ʈ������ ��´�� ���ߴ� ������ �Ѵ�.

        // localPosition�� �ƴ� ������ �������� ��ġ�� �˾ƾ��ϱ� ����
        // (�ڽ����� ���� ������ ��𼭵� �ش� ������Ʈ�� ��ġ�� ����.)
        if(Physics.Raycast(crossHairCam.transform.position, crossHairCam.transform.forward + 
            new Vector3(Random.Range(-crosshair.GetAccuray() - currentGun.accuracy, crosshair.GetAccuray() + currentGun.accuracy),
                        Random.Range(-crosshair.GetAccuray() - currentGun.accuracy, crosshair.GetAccuray() + currentGun.accuracy),
                        0f),
            out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            // 2�� �ڿ� ������ ����
            Destroy(clone, 2f);
        }
    }

    // ���弱 �õ�
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

    // ������
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

    // ������ �õ�
    void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
            FineSight();
    }

    // ������ ���
    public void CancleFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    // ������ ���� ����
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

    // ������ Ȱ��ȭ
    IEnumerator FindSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.FineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, .2f);
            yield return null;
        }
    }

    // ������ ��Ȱ��ȭ
    IEnumerator FindSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, .2f);
            yield return null;
        }
    }

    // �ݵ� �ڷ�ƾ
    IEnumerator RetorActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.FineSightOriginPos.y, currentGun.FineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // �ݵ� ����
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - .02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, .4f);

                yield return null;
            }

            // ����ġ
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, .1f);

                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.FineSightOriginPos;

            // �ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - .02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, .4f);

                yield return null;
            }

            // ����ġ
            while (currentGun.transform.localPosition != currentGun.FineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, .1f);

                yield return null;
            }
        }
    }

    // ȿ���� ���
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
