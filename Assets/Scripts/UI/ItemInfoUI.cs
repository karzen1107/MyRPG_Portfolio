using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;   //아이템 이미지
    [SerializeField] private TextMeshProUGUI itemName;  //아이템 이름
    [SerializeField] private TextMeshProUGUI itemGrade; //아이템 등급
    [SerializeField] private TextMeshProUGUI itemClass; //아이템 직업
    [SerializeField] private TextMeshProUGUI isUsingText;   //착용 여부
    [SerializeField] private TextMeshProUGUI itemPrice;   //아이템 가격

    [SerializeField] private TextMeshProUGUI canUseLevel;   //아이템 착용 가능 레벨
    [SerializeField] private TextMeshProUGUI itemAbilltyInfo;   //아이템 능력치 글자
    [SerializeField] private TextMeshProUGUI itemAbillity;  //아이템 능력치(공격력,방어력,회복량 등)
    [SerializeField] private TextMeshProUGUI itemHP;    //아이템 추가 생명력
    [SerializeField] private TextMeshProUGUI itemMP;    //아이템 추가 마나

    [SerializeField] private TextMeshProUGUI itemDescription; //아이템 부가 설명

    private string SetItemGradeToKorean(ItemGrade eng) //아이템 등급 한글화
    {
        switch (eng)
        {
            case ItemGrade.Normal: return "일반 등급";
            case ItemGrade.Advanced: return "고급 등급";
            case ItemGrade.Rare: return "희귀 등급";
            case ItemGrade.Hero: return "영웅 등급";
            case ItemGrade.Legend: return "전설 등급";
            default: return "";
        }
    }

    private string SetItemClassToKorean(PlayerClass eng)    //아이템 사용 클래스 한글화
    {
        switch (eng)
        {
            case PlayerClass.Warrior: return "전사 전용";
            case PlayerClass.Archor: return "궁수 전용";
            default: return "공용";
        }
    }

    private string SetItemEquipToKorean(EquipType eng)  //아이템 정보 한글화
    {
        switch (eng)
        {
            case EquipType.Weapon:
                return "공격력";
            case EquipType.Sheild:
            case EquipType.Head:
            case EquipType.Armor:
            case EquipType.Gloves:
            case EquipType.Pants:
            case EquipType.Belt:
            case EquipType.Boots:
                return "방어력";
            default: return "회복량";
        }
    }

    public void SetItemInfo(Item item)  //아이템 정보 세팅
    {
        itemImage.sprite = item.ItemImage;
        itemName.text = item.ItemName;
        itemGrade.text = SetItemGradeToKorean(item.Grade);
        itemClass.text = SetItemClassToKorean(item.ItemClass);
        isUsingText.text = "";
        itemPrice.text = item.CellPrice.ToString() + "G";

        canUseLevel.text = item.CanUseLevel.ToString();
        itemAbilltyInfo.text = SetItemEquipToKorean(item.EquipType);
        itemAbillity.text = item.ItemAbillity.ToString();
        itemHP.text = item.ItemHP.ToString();
        itemMP.text = item.ItemMP.ToString();

        itemDescription.text = item.ItemDescription;

        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());    //계산의 영향을 받는 레이아웃 요소 및 하위 레이아웃 요소를 즉시 재작성합니다.
        //위에꺼 쓴 이유 : 아이템 정보 뜰 때 아이템 부가설명의 크기에 맞춰 content size fitter의 Rect 사이즈가 실시간으로 조절이 되어야 하는데
        //실시간 업데이트가 안되어서 수동으로 사이즈 계산하도록 하기 위해

        //레이아웃 그룹 업데이트가 잘 안돼는데 인터넷 보니까 켰다 껏다 다시 켜주면 됀다고 해서 해보니까 됌.
        this.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }
}
