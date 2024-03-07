using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어의 로컬 데이터 관리
/// </summary>

[System.Serializable]
public class PlayerData
{
    // 저장해야 할것 :캐릭터 모델, 스킬, 스킬레벨, 스킬 포인트, 퀵슬롯 등록 데이터, 인벤토리 아이템 데이터
    public string ID;   //아이디
    public string Passworld; //비밀번호
    public string charName; //이름
    public PlayerClass charClass; //직업
    public int charLevel;   //레벨
    public float startLife; //체력
    public float startMana; //마나
    public float exp;   //경험치
    public float startAtk;  //공격력
    public float startShield;   //방어력
    public SavedPlayerTransform startTrans; //시작 트랜스폼

    public List<MainQuestData> playerAcceptQuestData; //진행중인 퀘스트 데이터
    public List<MainQuestData> playerCompleteQuestData; //완료한 퀘스트 데이터
    public List<OngoingQuest> ongoingQuests;  //퀘스트 리스트 데이터

    public InventoryData inventoryData; //인벤토리 데이터

    public PlayerData() //생성자. 저장할 값 세팅
    {
        charName = PlayerState.Instance.CharName;
        charLevel = PlayerState.Instance.CharLevel;
        startLife = PlayerState.Instance.StartLife;
        startMana = PlayerState.Instance.StartMana;
        exp = PlayerState.Instance.Exp;
        startAtk = PlayerState.Instance.NowAtk;
        startShield = PlayerState.Instance.NowShield;

        Vector3 pos = PlayerState.Instance.transform.position;
        Quaternion rot = PlayerState.Instance.transform.rotation;
        startTrans = new SavedPlayerTransform();
        startTrans.SetStartPos(pos.x, pos.y, pos.z);
        startTrans.SetStartRot(rot.x, rot.y, rot.z, rot.w);

        #region 퀘스트 데이터 세팅
        if (QuestManager.Instance != null)
        {
            playerAcceptQuestData = new List<MainQuestData>();
            for (int i = 0; i < QuestManager.Instance.PlayerAcceptQuestData.Count; i++)
            {
                playerAcceptQuestData.Add(QuestManager.Instance.PlayerAcceptQuestData[i]);
            }

            playerCompleteQuestData = new List<MainQuestData>();
            for (int i = 0; i < QuestManager.Instance.PlayerCompleteQuestData.Count; i++)
            {
                playerCompleteQuestData.Add(QuestManager.Instance.PlayerCompleteQuestData[i]);
            }

            ongoingQuests = new List<OngoingQuest>();
            List<OngoingQuestUI> qusetList = QuestManager.Instance.func_ReturnOngoingQuests?.Invoke();
            for (int i = 0; i < qusetList.Count; i++)
            {
                ongoingQuests.Add(qusetList[i].GetComponent<OngoingQuestUI>().OnQuest);
            }
        }
        #endregion

        #region 인벤토리 데이터 세팅
        if (InventoryManager.Instance != null)
        {
            inventoryData = new InventoryData();
            inventoryData.useableSlotCount = InventoryManager.Instance.UseableSlotCount;
            inventoryData.slots = new InvenSlotData[inventoryData.useableSlotCount];
            for (int i = 0; i < inventoryData.slots.Length; i++)
            {
                InvenSlotData tempSlot = new InvenSlotData();
                tempSlot.SaveSlotData(i);
                inventoryData.slots[i] = tempSlot;
            }
            inventoryData.gold = InventoryManager.Instance.Gold;
        }
        #endregion
    }
}

/// <summary> 플레이어의 클래스 목록 </summary>
public enum PlayerClass
{
    Warrior, Archor, None
}
