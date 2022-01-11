using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    // �θ� Ʒ �ı��Ǹ�, ĸ�� �ݶ��̴� ����
    [SerializeField]
    private Collider parentCollider;
    // �ڽ� Ʈ�� ������ �� �ʿ��� ������Ʈ �� �߷� Ȱ��ȭ
    [SerializeField]
    private Collider childCollider;
    [SerializeField]
    private Rigidbody childRigid;

    // ���� ���� ������
    [SerializeField]
    private GameObject[] treePieces;
    [SerializeField]
    private GameObject treeCenter;

    [SerializeField]
    private GameObject logPrefab;

    // �ڽ� Ʈ��
    [SerializeField]
    private GameObject childTree;

    // Ÿ�� ����Ʈ
    [SerializeField]
    private GameObject go_effect_prefab;

    // ���� ���� �ð�
    [SerializeField]
    private float debrisDestroyTime;
    // ���� ���� �ð�
    [SerializeField]
    private float destroyTime;

    // �ʿ��� ����
    [SerializeField]
    private string chop_Sound;
    [SerializeField]
    private string falldown_Sound;
    [SerializeField]
    private string logChange_Sound;

    // ������ �� �������� ������ ���� ����
    [SerializeField]
    private float force;

    public void Chop(Vector3 _pos, float _angleY)
    {
        Hit(_pos);

        AngleCalc(_angleY);

        if (CheckTreePieces())
            return;

        FallDownTree();
    }

    // ���� ����Ʈ
    void Hit(Vector3 _pos)
    {
        SoundManager.instance.PlaySE(chop_Sound);

        GameObject clone = Instantiate(go_effect_prefab, _pos, Quaternion.identity);
        Destroy(clone, debrisDestroyTime);
    }

    void AngleCalc(float _angleY)
    {
        Debug.Log(_angleY);
        if (0 <= _angleY && _angleY <= 70)
            DestroyPiece(2);
        else if (70 <= _angleY && _angleY <= 140)
            DestroyPiece(3);
        else if (140 <= _angleY && _angleY <= 210)
            DestroyPiece(4);
        else if (210 <= _angleY && _angleY <= 280)
            DestroyPiece(0);
        else if (280 <= _angleY && _angleY <= 360)
            DestroyPiece(1);
    }

    void DestroyPiece(int _num)
    {
        if (treePieces[_num].gameObject != null)
        {
            GameObject clone = Instantiate(go_effect_prefab, treePieces[_num].transform.position, Quaternion.identity);
            Destroy(clone, debrisDestroyTime);
            Destroy(treePieces[_num].gameObject);
        }
    }

    bool CheckTreePieces()
    {
        for (int i = 0; i < treePieces.Length; i++)
        {
            if (treePieces[i].gameObject != null)
                return true;
        }

        return false;
    }

    void FallDownTree()
    {
        SoundManager.instance.PlaySE(falldown_Sound);
        Destroy(treeCenter);

        parentCollider.enabled = false;
        childCollider.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
    }

    IEnumerator LogCoroutine()
    {
        yield return new WaitForSeconds(destroyTime);

        SoundManager.instance.PlaySE(logChange_Sound);

        Instantiate(logPrefab, childTree.transform.position + (childTree.transform.up * 3f), Quaternion.LookRotation(childTree.transform.up));
        Instantiate(logPrefab, childTree.transform.position + (childTree.transform.up * 6f), Quaternion.LookRotation(childTree.transform.up));
        Instantiate(logPrefab, childTree.transform.position + (childTree.transform.up * 9f), Quaternion.LookRotation(childTree.transform.up));

        Destroy(childTree.gameObject);
    }

    public Vector3 GetTreeCenterPosition()
    {
        return treeCenter.transform.position;
    }
}
