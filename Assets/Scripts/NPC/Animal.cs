using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController playerStatus;    // �÷��̾� ���� ����

    [SerializeField]
    protected string animalName;          // ���� �̸�
    [SerializeField]
    protected int animalHP;               // ���� ü��
    [SerializeField]
    protected float walkSpeed;            // �ȱ� �ӵ�
    [SerializeField]
    protected float runSpeed;             // �ٱ� �ӵ�

    // ���º���
    protected bool isAction;              // �ൿ ������ �Ǻ�
    protected bool isWalk;                // �ȴ��� �Ǻ�
    protected bool isRun;                 // �ٴ��� �Ǻ�
    protected bool isDead;                // �׾����� �Ǻ�
    protected bool isChasing;             // �߰� ������ �Ǻ�
    protected bool isAttacking;           // ���� ������ �Ǻ�

    [SerializeField]
    protected float walkTime;             // �ȱ� �ð�
    [SerializeField]
    protected float waitTime;             // ��� �ð�
    [SerializeField]
    protected float runTime;              // �ٱ� �ð�
    protected float currentChaseTime;

    // �ʿ��� ������Ʈ
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
    // NavMeshAgent�� RigidBody�� ��Ź�����.
    protected NavMeshAgent agent;
    protected FieldOfViewAngle fov;

    protected Vector3 destination;          // ����

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = FindObjectOfType<StatusController>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfViewAngle>();

        // ����Ű�� ����
        currentChaseTime = waitTime;
        // ��⵵ �ൿ�̱� ����
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
                // ���� ���� �ൿ ����
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
        Debug.Log("�ȱ�");
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
        // �ϻ� ���� 3��
        int _random = Random.Range(0, 3);
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
