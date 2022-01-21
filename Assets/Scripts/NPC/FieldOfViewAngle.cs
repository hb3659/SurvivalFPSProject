using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;            // �þ߰�
    [SerializeField]
    private float viewDistance;         // �þ߰Ÿ�
    [SerializeField]
    private LayerMask targetMask;       // Ÿ�� ����ũ(�÷��̾�)

    private PlayerController player;
    private NavMeshAgent agent;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetTargetPosition()
    {
        return player.transform.position;
    }

    public bool View()
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTF = _target[i].transform;
            if (_targetTF.name == "Player")
            {
                Vector3 _direction = (_targetTF.position - transform.position).normalized;
                // �ڽ��� ���� ����� �÷��̾��� ���� ������ ���� ���Ѵ�.
                float _angle = Vector3.Angle(_direction, transform.forward);

                if(_angle < viewAngle * .5f)
                {
                    // �þ� ���� �����Ѵ�.
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.Log("�÷��̾ �þ� ���� �ֽ��ϴ�.");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);

                            return true;
                        }
                    }
                }
            }

            if (player.GetRun())
            {
                if (CalcPathLength(player.transform.position) <= viewDistance)
                {
                    Debug.Log("������ �ֺ����� �ٰ� �ִ� �÷��̾��� �������� �ľ��߽��ϴ�.");
                    return true;
                }
            }
        }

        return false;
    }

    float CalcPathLength(Vector3 _targetPosition)
    {
        NavMeshPath _path = new NavMeshPath();
        agent.CalculatePath(_targetPosition, _path);

        // �ڽŰ� �÷��̾��� ��ġ�� ����ϱ� ����
        Vector3[] _wayPoint = new Vector3[_path.corners.Length + 2];

        // ù����� ������ �迭�� �ڽŰ� �÷��̾��� ��ġ�� �����Ѵ�.
        _wayPoint[0] = transform.position;
        _wayPoint[_path.corners.Length + 1] = _targetPosition;

        float _pathLength = 0;

        for (int i = 0; i < _path.corners.Length; i++)
        {
            // ��������Ʈ�� ��θ� �ִ´�.
            _wayPoint[i + 1] = _path.corners[i];
            // �ٷ� ��� ���̸� ����� �Ѵ�.
            _pathLength += Vector3.Distance(_wayPoint[i], _wayPoint[i + 1]);
        }

        return _pathLength;
    }
}
