using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 아이템을 등록할 수 있는 슬롯 설정 클래스
/// </summary>
public class UIDragAndDrop : MonoBehaviour
{
    [Header("사용가능한 슬롯")]
    [SerializeField] private List<uiNumbers> canUseSlot = new List<uiNumbers>();

    [Header("퀵슬롯 전용")]
    public QuickSlotType quickType;

    public List<uiNumbers> CanUseSlot { get { return canUseSlot; } set { canUseSlot = value; } }
}

public enum QuickSlotType
{
    Skill, Item, Max
}
