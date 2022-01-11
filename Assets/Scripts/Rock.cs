using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;                         // ������ ü��
    [SerializeField]
    private float destroyTime;              // ���� ���� �ð�
    [SerializeField]
    private float effectDestroyTime;        // ����Ʈ ���� �ð�

    [SerializeField]
    private Collider collider;              // ��ü �ݶ��̴�

    // �ʿ��� ���� ������Ʈ
    [SerializeField]
    private GameObject go_rock;             // �Ϲ� ����
    [SerializeField]
    private GameObject go_debris;           // �μ��� ����
    [SerializeField]
    private GameObject go_effect_prefab;    // ä�� ����Ʈ
    [SerializeField]
    private GameObject go_rock_item;        // ������ ������

    [SerializeField]
    private int count;

    // �ʿ��� ���� �̸�
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);

        GameObject clone = Instantiate(go_effect_prefab, collider.bounds.center, Quaternion.identity);
        Destroy(clone, effectDestroyTime);

        hp--;

        if (hp <= 0)
            Destruction();
    }

    void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);

        collider.enabled = false;

        for (int i = 0; i < count; i++)
        {
            Instantiate(go_rock_item, go_rock.transform.position, Quaternion.identity);
        }
        
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
