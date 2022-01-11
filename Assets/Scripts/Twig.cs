using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField]
    private int hp;                         // 체력
    [SerializeField]
    private float destroyTime;              // 이펙스 삭제 시간

    [SerializeField]
    private GameObject go_little_Twig;      // 부러진 나무 프리팹
    [SerializeField]
    private GameObject go_effect_prefab;    // 타격 이펙트

    // 회전값 변수
    private Vector3 originRot;
    private Vector3 wantedRot;
    private Vector3 currentRot;

    // 필요한 사운드 이름
    [SerializeField]
    private string hit_Sound;
    [SerializeField]
    private string broken_Sound;

    void Start()
    {
        originRot = transform.rotation.eulerAngles;
        currentRot = originRot;
    }

    public void Damage(Transform _playerTF)
    {
        hp--;

        Hit();
        StartCoroutine(HitSwayCoroutine(_playerTF));

        if(hp <= 0)
        {
            Destruction();
        }
    }

    void Hit()
    {
        SoundManager.instance.PlaySE(hit_Sound);

        GameObject clone = Instantiate(go_effect_prefab,
                                       gameObject.GetComponent<Collider>().bounds.center + (Vector3.up *.5f),
                                       Quaternion.identity);

        Destroy(clone, destroyTime);
    }

    IEnumerator HitSwayCoroutine(Transform _target)
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;

        CheckDirection(rotationDir);

        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, .25f);
            transform.rotation = Quaternion.Euler(currentRot);

            yield return null;
        }

        wantedRot = originRot;

        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, .15f);
            transform.rotation = Quaternion.Euler(currentRot);

            yield return null;
        }
    }

    private bool CheckThreshold()
    {
        if (Mathf.Abs(wantedRot.x - currentRot.x) <= .5f && Mathf.Abs(wantedRot.z - currentRot.z) <= .5f)
            return true;

        return false;
    }

    void CheckDirection(Vector3 _rotationDir)
    {
        Debug.Log(_rotationDir);

        if(_rotationDir.y > 180)
        {
            if (_rotationDir.y > 300)
                wantedRot = new Vector3(-50f, 0f, -50f);
            else if (_rotationDir.y > 240)
                wantedRot = new Vector3(0f, 0f, -50f);
            else
                wantedRot = new Vector3(50f, 0f, -50f);
        }
        else if(_rotationDir.y <= 180)
        {
            if (_rotationDir.y < 60)
                wantedRot = new Vector3(-50f, 0f, 50f);
            else if (_rotationDir.y > 120)
                wantedRot = new Vector3(0f, 0f, 50f);
            else
                wantedRot = new Vector3(50f, 0f, 50f);
        }
    }
    
    void Destruction()
    {
        SoundManager.instance.PlaySE(broken_Sound);

        GameObject clone1 = Instantiate(go_little_Twig,
                                       gameObject.GetComponent<Collider>().bounds.center + (Vector3.up * .5f),
                                       Quaternion.identity);
        GameObject clone2 = Instantiate(go_little_Twig,
                                       gameObject.GetComponent<Collider>().bounds.center - (Vector3.up * .5f),
                                       Quaternion.identity);

        Destroy(clone1, destroyTime);
        Destroy(clone2, destroyTime);

        Destroy(this.gameObject);
    }
}
