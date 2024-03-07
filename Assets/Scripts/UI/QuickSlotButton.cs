using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotButton : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode;    //퀵슬롯 키 이름
    [SerializeField] private GameObject iconImage;   //아이콘 이미지
    [SerializeField] private TextMeshProUGUI numberText;    //수량 텍스트
    private int originalIndex = -1;  //원본 인덱스

    // 컴포넌트 //
    private PlayerQuickSlotHandler playerQuickSlotHandler;  //PlayerQuickSlotHandler 스크립트
    private PlayerSkillManager skillManager;
    private InventoryManager inventoryManager;
    public GameObject IconImage { get { return iconImage; } }

    public void SendAction()    //PlayerQuickSlotHandler에게 해당 키의 데이터 보내주는 메서드
    {
        if (Input.GetKeyDown(keyCode) && originalIndex > -1)
        {
            if (iconImage.GetComponent<UIDragAndDrop>().quickType == QuickSlotType.Max)
                return;

            if (iconImage.GetComponent<UIDragAndDrop>().quickType == QuickSlotType.Skill)    //사용한게 스킬이면
                playerQuickSlotHandler.ReceiveSkillAction(skillManager.BaseSkills[originalIndex]);
            else  //사용한게 아이템이면
            {
                Item temp = inventoryManager.InvenSlots[originalIndex].Item;
                if (playerQuickSlotHandler.ReceiveItemAction(temp)) //아이템 사용에 성공했다면
                {
                    if (temp.ItemCount > 1)
                    {
                        temp.ItemCount--;
                        inventoryManager.SettingNewItem(temp, originalIndex);
                        numberText.text = temp.ItemCount.ToString();
                    }
                    else
                    {
                        inventoryManager.RemoveOldItem(originalIndex);
                        NullQuickSlot();
                    }
                }
            }
        }
    }

    public void SetQuickSlot(QuickSlotType type, int index) //퀵슬롯에 아이콘 등록하는 메서드
    {
        if (type == QuickSlotType.Max)    //대상이 없으면 초기화
        {
            iconImage.GetComponent<Image>().sprite = null;
            iconImage.GetComponent<UIDragAndDrop>().quickType = QuickSlotType.Max;
            iconImage.SetActive(false);
            originalIndex = -1;
            numberText.text = "";
        }
        else //대상이 있다면
        {
            if (type == QuickSlotType.Skill) //대상이 스킬일경우
            {
                originalIndex = index;
                iconImage.GetComponent<Image>().sprite = skillManager.BaseSkills[index].Skill.SkillImage;
                iconImage.GetComponent<UIDragAndDrop>().quickType = QuickSlotType.Skill;
                iconImage.SetActive(true);
                numberText.text = "";
                skillManager.BaseSkills[index].InitializeComboCount();  //콤보 카운트 초기화
                SetEffPos(skillManager.BaseSkills[index]);
            }
            else  //대상이 아이템일 경우
            {
                originalIndex = index;
                iconImage.GetComponent<Image>().sprite = inventoryManager.InvenSlots[index].Item.ItemImage;
                iconImage.GetComponent<UIDragAndDrop>().quickType = QuickSlotType.Item;
                iconImage.SetActive(true);
                numberText.text = inventoryManager.InvenSlots[index].Item.ItemCount.ToString();
            }
        }

    }

    public void NullQuickSlot() //퀵슬롯 초기화
    {
        iconImage.GetComponent<Image>().sprite = null;
        iconImage.SetActive(false);
        originalIndex = -1;
        numberText.text = "";
    }

    #region 지금은 안쓰지만 나중에 쓸지도모름
    //private void SetNegativeSize(RectTransform itemUI)  //아이템 UI 기본위치 설정
    //{
    //    itemUI.SetAsFirstSibling();
    //    itemUI.localScale = Vector3.one;
    //    itemUI.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
    //    itemUI.anchorMin = Vector2.zero;
    //    itemUI.anchorMax = Vector2.one;
    //    itemUI.offsetMin = new Vector2(0, 0); // = new Vector2(left,bottom);
    //    itemUI.offsetMax = new Vector2(0, 0); // = new Vector2(-right, -top);
    //    #region 하이어라키 오브젝트 순서 변경 메서드들
    //    //Transform.SetAsLastSibling(); - 해당 오브젝트의 순위를 마지막으로 변경(가장 나중에 출력되므로 겹쳐졋을 경우 앞으로 나옵니다.)
    //    //Transform.SetAsFirstSibling(); - 해당 오브젝트의 순위를 처음으로 변경(가장 처음 출력되므로 겹쳐졋을 경우 가려집니다.)
    //    //Transform.SetSiblingIndex(int nIndex); - nIndex를 매개변수를 넣어서 순위를 지정합니다.(0이 처음입니다.)
    //    //Transform.GetSiblingIndex(); - 해당 오브젝트의 순위를 얻어옵니다.
    //    #endregion
    //}
    #endregion

    private void SetEffPos(BaseSkill baseSkill)  //스킬 이펙트 위치 설정
    {
        if (baseSkill != null)   //이펙트 시작 위치 설정하기
        {
            Vector3 pos = playerQuickSlotHandler.gameObject.transform.position;
            for (int i = 0; i < baseSkill.effects.Length; i++)
            {
                baseSkill.effects[i].StartPosRot = playerQuickSlotHandler.gameObject.transform;
                baseSkill.effects[i].StartPosRot.position = new Vector3(pos.x, (pos.y + 15f), pos.z);
            }
        }
    }

    public int SendOriginalIndex()  //인덱스 번호 전송 메서드
    {
        return originalIndex;
    }

    private void Start()
    {
        playerQuickSlotHandler = GameObject.FindObjectOfType<PlayerQuickSlotHandler>();
        inventoryManager = InventoryManager.Instance;
        skillManager = PlayerSkillManager.Instance;
        numberText.text = "";
    }

    private void Update()
    {
        SendAction();
    }
}
