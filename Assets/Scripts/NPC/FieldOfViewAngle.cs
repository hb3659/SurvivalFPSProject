using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;            // 시야각
    [SerializeField]
    private float viewDistance;         // 시야거리
    [SerializeField]
    private LayerMask targetMask;       // 타겟 마스크(플레이어)

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
                // 자신의 앞쪽 방향과 플레이어의 방향 사이의 각을 구한다.
                float _angle = Vector3.Angle(_direction, transform.forward);

                if(_angle < viewAngle * .5f)
                {
                    // 시야 내에 존재한다.
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 시야 내에 있습니다.");
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
                    Debug.Log("돼지가 주변에서 뛰고 있는 플레이어의 움직임을 파악했습니다.");
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

        // 자신과 플레이어의 위치도 기억하기 위함
        Vector3[] _wayPoint = new Vector3[_path.corners.Length + 2];

        // 첫번재와 마지막 배열에 자신과 플레이어의 위치를 저장한다.
        _wayPoint[0] = transform.position;
        _wayPoint[_path.corners.Length + 1] = _targetPosition;

        float _pathLength = 0;

        for (int i = 0; i < _path.corners.Length; i++)
        {
            // 웨이포인트에 경로를 넣는다.
            _wayPoint[i + 1] = _path.corners[i];
            // 바로 경로 길이를 계산을 한다.
            _pathLength += Vector3.Distance(_wayPoint[i], _wayPoint[i + 1]);
        }

        return _pathLength;
    }
}
