using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private PlayerStatusSlot head;
    [SerializeField] private PlayerStatusSlot weapon;
    [SerializeField] private PlayerStatusSlot body;
    [SerializeField] private PlayerStatusSlot pants;
    [SerializeField] private PlayerStatusSlot shield;
    [SerializeField] private PlayerStatusSlot gloves;
    [SerializeField] private PlayerStatusSlot belt;
    [SerializeField] private PlayerStatusSlot boots;
    private List<PlayerStatusSlot> slots;   //정보창 슬롯 리스트

    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI MPText;
    [SerializeField] private TextMeshProUGUI AttackText;
    [SerializeField] private TextMeshProUGUI ShieldText;

    private PlayerStatusSlot endSlot;
    private Item endSlotItem;

    // 컴포넌트 //
    private PlayerState playerState;
    private InventoryManager inventoryManager;
    private UIManager uiManager;

    // 이벤트 //
    public UnityAction ev_OnResetIndex; //인벤토리UI 슬롯 인덱스 리셋 이벤트
    public List<PlayerStatusSlot> Slots { get { return slots; } }

    public void CloseClick()    //닫기 버튼 클릭 메서드
    {
        this.gameObject.SetActive(false);
    }

    public void SetUserItem(Item item)  //알맞은 자리에 아이템 장착하는 메서드
    {
        switch (item.EquipType)
        {
            case EquipType.Head:
                head.SlotSetUI(item);
                break;
            case EquipType.Weapon:
                weapon.SlotSetUI(item);
                break;
            case EquipType.Sheild:
                shield.SlotSetUI(item);
                break;
            case EquipType.Armor:
                body.SlotSetUI(item);
                break;
            case EquipType.Gloves:
                gloves.SlotSetUI(item);
                break;
            case EquipType.Pants:
                pants.SlotSetUI(item);
                break;
            case EquipType.Belt:
                belt.SlotSetUI(item);
                break;
            case EquipType.Boots:
                boots.SlotSetUI(item);
                break;
        }
    }

    private void OnPointerEnter()   //마우스가 슬롯 위로 올라갔을때 정보 보이기
    {
        foreach (var result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
        {
            if (result.gameObject.GetComponent<InventorySlot>() == null)
                continue;

            GameObject selected = result.gameObject;
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i] == selected.GetComponent<InventorySlot>() && playerState.UsingItems[i] != null)
                {
                    uiManager.IconOver(playerState.UsingItems[i]);
                    break;
                }
            }
        }
    }

    private void OnPointerExit()   //마우스가 슬롯 밖으로 나갔을때 정보 지우기
    {
        bool isOut = true;
        foreach (RaycastResult result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
        {
            if (result.gameObject.GetComponent<InventorySlot>() != null)
            {
                isOut = false;
                break;
            }
            isOut = true;
        }

        if (isOut)
            uiManager.IconExit();
    }

    private void OnPointerDown()    //마우스 클릭다운시 메서드
    {
        if (Input.GetMouseButtonDown(0))
        {
            uiManager.IconExit();

            foreach (var result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
            {
                PlayerStatusSlot temp = result.gameObject.GetComponent<PlayerStatusSlot>();
                if (temp != null && temp.Item != null)
                {
                    for (int i = 0; i < slots.Count; i++)
                    {
                        if (temp == slots[i])
                        {
                            uiManager.IconSelect(temp.ItemImage, i);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void OnPointerUp(Item equipment)  //마우스 클릭업시 메서드. 퀵슬롯 삭제, 퀵슬롯 스위치의 역할
    {
        if (Input.GetMouseButtonUp(0))
        {
            foreach (var result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
            {
                PlayerStatusSlot temp = result.gameObject.GetComponent<PlayerStatusSlot>();
                if (temp != null)
                {
                    endSlot = temp;
                    endSlotItem = temp.Item;
                    SwitchItem(equipment, endSlotItem); //아이템 위치 교체
                }
            }
        }
    }

    private void SwitchItem(Item start, Item end)   //아이템 위치 교환 메서드(인벤토리 <-> 장비창)
    {
        if (start == null)
            return;
        if (start.EquipType != endSlot.EquipType)
            return;
        //경우의 수 : end == 빈칸 / end != 빈칸
        if (end == null)
        {
            playerState.AddPlayerStatus(start);
            SetUserItem(start);
            inventoryManager.RemoveOldItem(uiManager.Index);
        }
        else
        {
            playerState.RemovePlayerStatus(end);
            inventoryManager.RemoveOldItem(uiManager.Index);
            inventoryManager.SettingNewItem(end, uiManager.Index);
            playerState.AddPlayerStatus(start);
            SetUserItem(start);
        }
        ev_OnResetIndex?.Invoke();
    }

    private void InteractItem() //마우스 오른쪽 클릭시 장착 해제 (Interact : 상호작용하다)
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (var result in uiManager.Results) //어떤 슬롯을 레이캐스트 했는지 검색
            {
                if (result.gameObject.GetComponent<PlayerStatusSlot>() == null)
                    continue;

                PlayerStatusSlot selected = result.gameObject.GetComponent<PlayerStatusSlot>();
                if (selected.Item != null && inventoryManager.GetItem(selected.Item)) //인벤토리에 아이템 등록을 성공하면
                {
                    playerState.RemovePlayerStatus(selected.Item);
                    selected.SlotSetNull();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.Instance;
        inventoryManager = InventoryManager.Instance;
        uiManager = UIManager.Instance;

        slots = new List<PlayerStatusSlot>()
        {
            head, weapon, body, pants, shield, gloves, belt, boots
        };

        uiManager.ev_OnPlayerStatusUIDrop += OnPointerUp;
    }

    private void Update()
    {
        HPText.text = playerState.StartLife.ToString();
        MPText.text = playerState.StartMana.ToString();
        AttackText.text = playerState.NowAtk.ToString();
        ShieldText.text = playerState.NowShield.ToString();

        if (uiManager.ValidNum == uiNumbers.PlayerStatusUI)
        {
            OnPointerEnter();
            OnPointerDown();
            InteractItem();
        }

        OnPointerExit();
    }
}
