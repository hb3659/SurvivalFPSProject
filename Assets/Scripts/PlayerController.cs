using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동 속도 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 카메라 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 한계치
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    // 컴포넌트
    [SerializeField]
    private Camera FirstPersonCamera;
    private Rigidbody rigid;
    private new CapsuleCollider collider;
    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private Crosshair crosshair;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();

        // 초기화
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
        MoveCheck();
        CameraRotation();
        CharacterRotation();
    }

    // 앉기 시도
    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
    }

    // 앉기 동작
    void Crouch()
    {
        isCrouch = !isCrouch;
        crosshair.CrouchingAnimation(isCrouch);

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

    // 부드러운 앉기 동작
    IEnumerator CrouchCoroutine()
    {
        float posY = FirstPersonCamera.transform.localPosition.y;
        int count = 0;

        while (posY != applyCrouchPosY)
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

    // 지면 체크
    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, collider.bounds.extents.y + 0.1f);
        crosshair.JumpAnimation(!isGround);
    }

    // 점프 시도
    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
            Jump();
    }

    // 점프
    void Jump()
    {
        // 앉은 상태에서 점프하면 앉은 상태 해제
        if (isCrouch)
            Crouch();

        rigid.velocity = transform.up * jumpForce;
    }

    // 달리기 시도
    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Running();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            RunningCancle();
    }

    // 달리기 실행
    void Running()
    {
        // 앉은 상태에서 달리면 앉은 상태 해제
        if (isCrouch)
            Crouch();

        gunController.CancleFineSight();

        isRun = true;
        crosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    // 달리기 취소
    void RunningCancle()
    {
        isRun = false;
        crosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    // 움직임 실행
    void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        //rigid.MovePosition(transform.position + velocity * Time.deltaTime);
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= .01f)
                isWalk = true;
            else
                isWalk = false;

            crosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    // 상하 카메라 회전
    void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        // 카메라 상하반전 (+=, -=)
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        FirstPersonCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // 좌우 캐릭터 회전
    void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(characterRotationY));
    }
}
