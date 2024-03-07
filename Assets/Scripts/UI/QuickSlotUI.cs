using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 인벤토리, 스킬북의 오브젝트 및 퀵슬롯간의 오브젝트 이동을 관리하는 클래스
/// </summary>
public class QuickSlotUI : MonoBehaviour
{
    private QuickSlotButton[] quickButtonIndex; //퀵슬롯 버튼들의 인덱스 번호 배열
    private int beginSlotIndex;
    private int endSlotIndex;
    private QuickSlotType endSlotQuickType;

    // 컴포넌트 //
    UIManager uiManager;

    private void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (RaycastResult result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
            {
                QuickSlotButton temp = result.gameObject.GetComponent<QuickSlotButton>();
                if (temp != null)
                {
                    for (int i = 0; i < quickButtonIndex.Length; i++)
                    {
                        if (temp == quickButtonIndex[i])
                        {
                            beginSlotIndex = i;
                            uiManager.IconSelect(quickButtonIndex[i].IconImage, quickButtonIndex[i].SendOriginalIndex());
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnPointerUp(QuickSlotType iconType, int index)  //마우스 클릭업시 메서드. 퀵슬롯 삭제, 퀵슬롯 스위치의 역할
    {
        foreach (RaycastResult result in uiManager.Results)
        {
            for (int i = 0; i < quickButtonIndex.Length; i++)   //어떤 슬롯을 레이캐스트 했는지 검색
            {
                if (quickButtonIndex[i] == result.gameObject.GetComponent<QuickSlotButton>())
                {
                    endSlotIndex = i;
                    endSlotQuickType = quickButtonIndex[endSlotIndex].IconImage.GetComponent<UIDragAndDrop>().quickType;
                    break;
                }
            }

            if (endSlotIndex > -1)
                break;
        }
        if (endSlotIndex > -1)
        {
            SwitchItem(iconType, quickButtonIndex[endSlotIndex], index);
        }
    }

    private void SwitchItem(QuickSlotType startType, QuickSlotButton end, int startIndex)
    {
        if (startType == QuickSlotType.Max)
            return;
        //경우의 수 : end == 빈칸 / end != 빈칸
        if (end.SendOriginalIndex() <= -1)    //빈칸인 경우
        {
            end.SetQuickSlot(startType, startIndex);

            if (beginSlotIndex > -1)  //시작 아이콘이 같은 퀵슬롯이라면
            {
                quickButtonIndex[beginSlotIndex].NullQuickSlot();
            }

        }
        else  //빈칸이 아닌경우
        {
            int num = quickButtonIndex[endSlotIndex].SendOriginalIndex();
            end.SetQuickSlot(startType, startIndex);
            if (beginSlotIndex > -1) //시작 아이콘이 같은 퀵슬롯이 내부라면 해당 퀵슬롯의 세팅한 객체 정보 가져오기
            {
                quickButtonIndex[beginSlotIndex].SetQuickSlot(endSlotQuickType, num); ;
            }
        }
        ResetSlot();
    }

    public void DeleteBeginSlot()
    {
        if (beginSlotIndex > -1)
            quickButtonIndex[beginSlotIndex].NullQuickSlot();
        ResetSlot();
    }

    public void ResetSlot()
    {
        beginSlotIndex = -1;
        endSlotIndex = -1;
        endSlotQuickType = QuickSlotType.Max;
    }

    private void Awake()
    {
        uiManager = UIManager.instance;
        quickButtonIndex = this.GetComponentsInChildren<QuickSlotButton>();
    }

    private void Start()
    {
        if (uiManager == null)
            uiManager = UIManager.instance;

        uiManager.ev_OnQuikSlotsUIDrop += OnPointerUp;
        uiManager.ev_OnDeleteUI += DeleteBeginSlot;
        ResetSlot();
    }
    private void Update()
    {
        if (uiManager.ValidNum == uiNumbers.QuikSlots)
            OnPointerDown();
    }
}
