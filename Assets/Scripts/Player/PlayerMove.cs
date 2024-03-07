using UnityEngine;
using UnityEngine.InputSystem;
/// <summary> 플레이어 이동,점프를 관리하는 클래스 </summary>
public class PlayerMove : MonoBehaviour
{
    // 컴포넌트 관련 //
    private CharacterController controller; //캐릭터 컨트롤러
    private Animator ani;   //플레이어 애니메이션

    [Header("이동 관련")]
    private Vector2 inputMove; //이동 방향 인풋 
    private float inputX;   //inputMove X값 저장
    private float inputY;   //inputMove Y값 저장
    private Transform playerCamera;   //카메라 회전 값 받아올 객체
    private Vector3 cameraDir; //카메라가 바라보는 벡터 방향
    private Vector3 dir;    //플레이어 이동 벡터 방향
    private Vector3 finalDir;   //최종 방향
    [SerializeField] private float moveSpeed;    //플레이어 이동 속도

    [Header("점프 관련")]
    [SerializeField] private float jumpPower;    //플레이어 점프 힘
    [SerializeField] private float gravity;    //플레이어에게 작용하는 중력
    private bool isGrounded;    //플레이어가 땅위에 있는지 확인
    RaycastHit hit; //바닥 확인할 레이캐스트

    // 속성 //
    public bool IsGrounded    //isGround 속성
    {
        get { return isGrounded; }
        set
        {
            isGrounded = value;
            ani.SetBool(PlayerAniVariable.isGround, value);
        }
    }

    public Transform PlayerCamera { get { return playerCamera; } }

    #region InputSystem 메서드
    public void OnMove(InputAction.CallbackContext context) //플레이어 이동 메서드
    {
        inputMove = context.ReadValue<Vector2>();
        inputX = inputMove.x;
        inputY = inputMove.y;
    }

    public void OnJump(InputAction.CallbackContext context) //플레이어 점프 메서드
    {
        if (ani.GetBool(PlayerAniVariable.isDeath) || this.GetComponent<PlayerState>().AttackMode) //죽었거나 공격모드일때는 점프 불가
            return;
        if (context.started && IsGround())
        {
            finalDir.y = jumpPower;
            ani.SetTrigger(PlayerAniVariable.jump);
        }
    }
    #endregion

    public void Move()  //플레이어 이동 메서드
    {
        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        cameraDir = playerCamera.forward; //카메라의 로컬 회전값에 z축만을 방향으로 설정
        cameraDir.y = 0;  //y축은 0으로 해서 항상 전방을 바라보게 함
        dir = (cameraDir * inputY + playerCamera.right * inputX).normalized;
        if (inputMove != Vector2.zero)
        {
            ani.SetBool(PlayerAniVariable.run, true);
            this.transform.rotation = Quaternion.LookRotation(dir);
        }
        else
            ani.SetBool(PlayerAniVariable.run, false);
    }

    private void Jump() //플레이어 점프 메서드
    {
        if (!IsGround())    //땅에서 떨어져있다면
            finalDir.y -= gravity * Time.deltaTime;
    }

    private bool IsGround() //땅위에 있는지 확인하는 메서드
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.05f))
            IsGrounded = true;
        else
            IsGrounded = false;

        return IsGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, Vector3.down * hit.distance);
    }

    private void Start()
    {
        ani = this.ani = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        dir = Vector3.zero;
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (ani.GetBool(PlayerAniVariable.isDeath) || ani.GetBool(PlayerAniVariable.useSkill))
            return;
        IsGround();
        Move();
        Jump();

        finalDir = new Vector3(dir.x, finalDir.y, dir.z);   //최종 이동 방향

        if (ani.GetBool(PlayerAniVariable.isDeath) || ani.GetBool(PlayerAniVariable.useSkill))  //죽었거나 스킬 사용중일때는 이동방향 없음
            finalDir = Vector3.zero;

        controller.Move(finalDir * moveSpeed * Time.deltaTime);
    }
}