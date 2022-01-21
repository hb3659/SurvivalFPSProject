using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController playerStatus;    // 플레이어 상태 정보

    [SerializeField]
    protected string animalName;          // 동물 이름
    [SerializeField]
    protected int animalHP;               // 동물 체력
    [SerializeField]
    protected float walkSpeed;            // 걷기 속도
    [SerializeField]
    protected float runSpeed;             // 뛰기 속도

    // 상태변수
    protected bool isAction;              // 행동 중인지 판별
    protected bool isWalk;                // 걷는지 판별
    protected bool isRun;                 // 뛰는지 판별
    protected bool isDead;                // 죽었는지 판별
    protected bool isChasing;             // 추격 중인지 판별
    protected bool isAttacking;           // 공격 중인지 판별

    [SerializeField]
    protected float walkTime;             // 걷기 시간
    [SerializeField]
    protected float waitTime;             // 대기 시간
    [SerializeField]
    protected float runTime;              // 뛰기 시간
    protected float currentChaseTime;

    // 필요한 컴포넌트
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected Collider collider;
    protected AudioSource audioSource;
    [SerializeField]
    protected AudioClip[] sound_Normal;
    [SerializeField]
    protected AudioClip sound_Hurt;
    [SerializeField]
    protected AudioClip sound_Dead;
    // NavMeshAgent는 RigidBody를 잠궈버린다.
    protected NavMeshAgent agent;
    protected FieldOfViewAngle fov;

    protected Vector3 destination;          // 방향

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = FindObjectOfType<StatusController>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfViewAngle>();

        // 대기시키기 위함
        currentChaseTime = waitTime;
        // 대기도 행동이기 때문
        isAction = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isDead)
        {
            Move();
            ElapsedTime();
        }
    }

    protected void Move()
    {
        if (isWalk || isRun)
            //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            agent.SetDestination(transform.position + destination * 5f);
    }

    protected void ElapsedTime()
    {
        if (isAction)
        {
            currentChaseTime -= Time.deltaTime;

            if (currentChaseTime <= 0 && !isChasing && !isAttacking)
                // 다음 랜덤 행동 개시
                ResetAction();
        }
    }

    protected virtual void ResetAction()
    {
        isWalk = false;
        isRun = false;
        isAction = true;
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Run", isRun);
        agent.speed = walkSpeed;
        agent.ResetPath();
        destination.Set(Random.Range(-.2f, -.2f), 0f, Random.Range
            (.5f, .1f));
    }

    protected void TryWalk()
    {
        isWalk = true;
        animator.SetBool("Walk", isWalk);
        currentChaseTime = walkTime;
        agent.speed = walkSpeed;
        Debug.Log("걷기");
    }

    public virtual void Damage(int _damage, Vector3 _targetPosition)
    {
        if (!isDead)
        {
            animalHP -= _damage;

            if (animalHP <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            animator.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        PlaySE(sound_Dead);
        isWalk = false;
        isRun = false;
        isDead = true;
        animator.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        // 일상 사운드 3개
        int _random = Random.Range(0, 3);
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
