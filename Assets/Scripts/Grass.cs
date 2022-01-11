using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private int hp;
    [SerializeField]
    private int destroyTime;
    [SerializeField]
    private float force;                                  // Æø¹ß·Â

    [SerializeField]
    private GameObject go_effect_prefab;                // Å¸°Ý ÀÌÆåÆ®

    private Rigidbody[] rigids;
    private Collider[] colliders;

    [SerializeField]
    private string hit_Sound;

    void Start()
    {
        rigids = this.transform.GetComponentsInChildren<Rigidbody>();
        colliders = this.transform.GetComponentsInChildren<Collider>();
    }

    public void Damage()
    {
        hp--;

        Hit();

        if(hp <= 0)
        {
            Destruction();
        }
    }

    void Hit()
    {
        SoundManager.instance.PlaySE(hit_Sound);

        GameObject clone = Instantiate(go_effect_prefab, transform.position + Vector3.up, Quaternion.identity);

        Destroy(this.gameObject, destroyTime);
    }

    void Destruction()
    {
        for (int i = 0; i < rigids.Length; i++)
        {
            rigids[i].useGravity = true;
            rigids[i].AddExplosionForce(force, transform.position, 1f);
            colliders[i].enabled = true;
        }

        Destroy(this.gameObject, destroyTime);
    }
}
