using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusSlot : InventorySlot
{
    [SerializeField] private EquipType equipType;   //슬롯의 장비 타입
    [SerializeField] private GameObject shadowImage;    //착용 아이템 슬롯 미착용시 그림자 이미지

    public EquipType EquipType { get { return equipType; } }

    public override void SlotSetUI(Item item)
    {
        base.SlotSetUI(item);

        if (shadowImage != null)
            shadowImage.SetActive(false);
    }

    public override void SlotSetNull()
    {
        base.SlotSetNull();

        if (shadowImage != null)
            shadowImage.SetActive(true);
    }
}
