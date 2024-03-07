using Unity.VisualScripting;
using UnityEngine;

public class DropedItem : MonoBehaviour, IInteractive
{
    // 아이템 관련 //
    [SerializeField] private Item_SO itemInfo; //해당 아이템 정보
    [SerializeField] private Transform item;    //회전 시킬 객체
    [SerializeField] private float rotateSpeed = 40; //객체 회전 속도
    [SerializeField] private float moveSpeed;   //객체 드랍,획득 시 움직이는 속도
    [SerializeField] private Outline outline;   //아웃라인 스크립트

    // 글자 관련 //
    [SerializeField] private Transform tr_itemText;  //아이템 이름
    private TextMesh itemText;  //아이템 이름 텍스트
    private GameObject cam;  //메인 카메라
    private Vector3 startScale; //글씨 초기 스케일
    private float distance = 3f;    //유니티 좌표 단위1을 1미터라고 가정할때 3미터 거리에서 글자의 크기가 원본으로 보이게 하기 위함.
    private float showingDis = 15f; //아이템 이름이 보여지는 카메라와 아이템의 최대 거리

    // 속성 //
    public Item_SO ItemInfo { get { return itemInfo; } }   //아이템 정보 속성

    public void DoAction()  //아이템 획득
    {
        if (itemInfo.Type == ItemType.Gold)
        {
            InventoryManager.Instance.GetGold(ItemInfo.GoldAmount);
            Destroy(this.gameObject);
        }
        else
        {
            if (InventoryManager.Instance.GetItem(ItemInfo))  // 획득한 아이템의 정보를 플레이어 데이터에 전달한다.
                Destroy(this.gameObject);
        }
    }

    public void SetOutLine(bool isShow)   //플레이어와 가장 가까운 아이템 아웃라인 생성
    {
        outline.enabled = isShow;
    }

    private void Start()
    {
        itemText = tr_itemText.GetComponent<TextMesh>();
        cam = Camera.main.gameObject;
        startScale = tr_itemText.transform.localScale;
    }

    private void Update()
    {
        tr_itemText.rotation = cam.transform.rotation;  //카메라 방향으로 글자가 바라보게끔 제어

        item.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);  //아이템이 회전하도록 제어

        //카메라 거리가 달라도 일정하게 보이도록 제어 + 일정거리 멀어지면 글자 안보이게 제어
        float dis = Vector3.Distance(cam.transform.position, tr_itemText.position);

        if (dis > showingDis)
            itemText.text = "";
        else
            itemText.text = ItemInfo.ItemName;

        Vector3 newScale = startScale * dis / distance;
        tr_itemText.localScale = newScale;
    }
}
