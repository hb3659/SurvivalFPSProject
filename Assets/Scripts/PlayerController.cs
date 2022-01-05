using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �̵� �ӵ� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    // ���� ����
    private bool isRun;
    private bool isGround = true;
    private bool isCrouch;

    // �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    // ī�޶� �Ѱ�ġ
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    // ������Ʈ
    [SerializeField]
    private Camera FirstPersonCamera;
    private Rigidbody rigid;
    private new CapsuleCollider collider;
    [SerializeField]
    private GunController gunController;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;

        originPosY = FirstPersonCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    // �ɱ� �õ�
    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
    }

    // �ɱ� ����
    void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    // �ε巯�� �ɱ� ����
    IEnumerator CrouchCoroutine()
    {
        float posY = FirstPersonCamera.transform.localPosition.y;
        int count = 0;

        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.1f);
            FirstPersonCamera.transform.localPosition = new Vector3(0, posY, 0);

            if (count > 30)
                break;

            yield return null;
        }

        FirstPersonCamera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);
    }

    // ���� üũ
    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, collider.bounds.extents.y + 0.1f);
    }

    // ���� �õ�
    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
            Jump();
    }

    // ����
    void Jump()
    {
        // ���� ���¿��� �����ϸ� ���� ���� ����
        if (isCrouch)
            Crouch();

        rigid.velocity = transform.up * jumpForce;
    }

    // �޸��� �õ�
    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Running();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            RunningCancle();
    }

    // �޸��� ����
    void Running()
    {
        // ���� ���¿��� �޸��� ���� ���� ����
        if (isCrouch)
            Crouch();

        gunController.CancleFineSight();

        isRun = true;
        applySpeed = runSpeed;
    }

    // �޸��� ���
    void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    // ������ ����
    void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        rigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    // ���� ī�޶� ȸ��
    void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        // ī�޶� ���Ϲ��� (+=, -=)
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        FirstPersonCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // �¿� ĳ���� ȸ��
    void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(characterRotationY));
    }
}
