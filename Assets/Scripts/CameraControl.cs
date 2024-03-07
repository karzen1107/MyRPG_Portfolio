using UnityEngine;
using UnityEngine.Playables;

public class CameraControls : Singletone<CameraControls>
{
    private float rotateX;  //카메라 X축 회전
    private float rotateY;  //카메라 Y축 회전
    [SerializeField] private float rotateSpeed;  //카메라 회전 속도
    private float maxRoX = 80;   //X축 최대 회전 각도
    private float minRoX = -70;   //X축 최소 회전 각도

    private Vector3 dir;    //플레이어와 카메라의 벡터 방향
    private float finalDistance; //장애물 까지 계산한 후의 최종 카메라 거리
    [SerializeField] private LayerMask obstacle;    //장애물이라고 인식할 레이어 설정

    [SerializeField] private float mouseWheelSpeed; //마우스 휠 속도;
    [SerializeField] private float dis = 3;  //휠로 설정할 플레이어와 카메라의 거리
    [SerializeField] private float minDis = 1;   //휠로 설정할 수 있는 거리 최솟값
    [SerializeField] private float maxDis = 6; //휠로 설정할 수 있는 거리 최댓값
    private float lastDis;  //최근 프레임의 카메라의 거리
    [SerializeField] private Transform cameraView; //카메라가 바라볼 대상

    // 컴포넌트 //
    private PlayerState playerState;

    // 속성 //
    public bool IsNpcInteracting { get; set; }
    public Vector3 Dir { get { return dir; } }

    private void MousePos() //마우스로 플레이어 중심으로 카메라 회전, 위치 제어
    {
        rotateX -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        rotateY += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        rotateX = Mathf.Clamp(rotateX, minRoX, maxRoX);
        this.transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);    //카메라 회전값
        dir = (this.transform.position - cameraView.position).normalized;    //플레이어와 카메라의 방향 벡터

        ObstructCamera();

        Vector3 centerPosition = new Vector3(0, 0, finalDistance);    //카메라로부터 z축으로 dis만큼 떨어진 회전 기준 위치
        #region centerPosition이 필요한 이유 설명
        /*
        해당 코드에서 `reverseDistance`는 카메라가 바라보는 방향의 앞쪽으로 이동하기 위한 벡터입니다.
        카메라를 촬영하면서 이동할 때, 카메라가 바라보는 방향의 앞쪽으로 이동해야 합니다.
        이때, 카메라가 바라보는 방향은 보통 Z축 방향이며, 이동량을 계산하기 위해서는 이 방향에 대한 벡터가 필요합니다.
        `reverseDistance`는 카메라가 바라보는 방향의 앞쪽으로 이동하기 위한 벡터를 구하기 위해, `distance`값을 Z축 방향으로 가지는 새로운 `Vector3`를 생성합니다. 
        즉, 카메라가 바라보는 방향으로 `distance`만큼 이동하기 위해서는, `reverseDistance` 벡터를 사용하면 됩니다.
        이후, `transform.position`을 계산할 때, `player.transform.position`에서 `transform.rotation`을 적용한 `reverseDistance` 벡터를 뺀 값으로 설정합니다. 
        이렇게 되면, 플레이어의 위치에서 카메라가 바라보는 방향으로 `distance`만큼 뒤쪽으로 이동한 위치가 `transform.position`이 됩니다.
        */
        #endregion
        this.transform.position = cameraView.position - this.transform.rotation * centerPosition;
        #region this.transform.rotation * centerPosition 이부분 설명
        /*
        `transform.rotation`은 회전을 나타내는 `Quaternion` 자료형이고, `transform.position`은 위치를 나타내는 `Vector3` 자료형입니다. 
        이 두 자료형은 서로 다른 자료형이지만, 벡터와 행렬의 곱셈 연산과 유사한 방식으로 곱할 수 있습니다.
        `transform.rotation`은 카메라의 회전 값을 나타내며, `transform.position`은 카메라의 위치 값을 나타냅니다. 
        이 두 값을 곱하면, 카메라의 위치 값에 회전 값을 적용한 새로운 위치 값을 얻을 수 있습니다. 
        이렇게 계산된 위치 값은 새로운 카메라 위치가 되며, 이를 `transform.position`에 할당함으로써 카메라의 위치를 업데이트할 수 있습니다.
        따라서 `transform.rotation`과 `transform.position`을 곱할 수 있는 이유는, 이 두 값을 연산하여 카메라의 위치와 회전 값을 동시에 업데이트할 수 있기 때문입니다
         */
        #endregion

    }

    private void ZoomInOut()    //마우스 휠로 카메라 줌인,아웃
    {
        dis -= Input.GetAxis("Mouse ScrollWheel") * mouseWheelSpeed * Time.deltaTime;
        dis = Mathf.Clamp(dis, minDis, maxDis);

        if (lastDis != dis)
        {
            float y = (0.425f * dis) + 0.875f;    //일차함수의 그래프 식(y = ax + b / (1, 1.3) , (5, 3)
            cameraView.transform.localPosition = new Vector3(cameraView.localPosition.x, y, cameraView.localPosition.z);
        }
    }

    private void ObstructCamera()   //카메라와 플레이어 사이에 장애물이 있을경우
    {
        if (Physics.Raycast(cameraView.position, dir, out RaycastHit hit, dis, obstacle))
            finalDistance = hit.distance * 0.9f;
        else
            finalDistance = dis;
    }

    public void FindTarget()    //타겟 활성화 & 비활성화
    {
        if (Physics.Raycast(this.transform.localPosition, -dir, out RaycastHit hit, 50f, ~LayerMask.NameToLayer("EnemyHitArea"), QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(this.transform.position, hit.point);
            playerState.TargettingAim(hit.point);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(this.transform.localPosition, -dir * 25f);
    //}

    private void Start()
    {
        playerState = PlayerState.Instance;
        cameraView = playerState.transform.Find("CameraCenter");
        dis = 3;
        float y = (0.425f * dis) / 25f + (0.875f / 125f);    //일차함수의 그래프 식(y = ax + b / (1, 1.3) , (5, 3)
        cameraView.transform.localPosition = new Vector3(cameraView.localPosition.x, y, cameraView.localPosition.z);
        IsNpcInteracting = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (IsNpcInteracting)
            return;

        MousePos();
        ZoomInOut();
        FindTarget();
        lastDis = dis;
    }
}