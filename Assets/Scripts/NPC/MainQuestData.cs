using System.Collections.Generic;

[System.Serializable]
public class MainQuestData
{
    public int number;  //퀘스트 번호
    public int characterIndex;    //NPC 인덱스
    public string questName; //퀘스트 이름
    public string description;  //퀘스트 설명
    public int dialogIndex; // 퀘스트 대화 인덱스
    public int level;   //퀘스트 레벨 제한

    public int goalAmount;  //퀘스트 달성 목표
    public int goldReward;  //퀘스트 골드 보상
    public int expReward;   //퀘스트 경험치 보상
    public int itemReward;  //퀘스트 아이템 보상

    public QuestState questState;

    public List<MiniQuestData> miniQuests = new List<MiniQuestData>();

    public void UpdateBattleQuest()   //미니 퀘스트 데이터 업데이트
    {
        if (miniQuests[0].questType != QuestType.Battle)
            return;

        miniQuests[0].nowGoalAmount++;
        if (miniQuests[0].nowGoalAmount >= miniQuests[0].goalAmount)
            miniQuests.RemoveAt(0);

        if (miniQuests.Count <= 1)
        {
            questState = QuestState.Complete;
            QuestManager.Instance.ev_CheckNpcQuest?.Invoke(this);
        }
    }

    public void UpdateSocialQuest(NpcAction nowNpc)   //미니 퀘스트 데이터 업데이트
    {
        if (miniQuests[0].questType != QuestType.Social)
            return;

        if (miniQuests[0].goalNpc == nowNpc.NPC.npcIndex)
            miniQuests.RemoveAt(0);

        if (miniQuests.Count <= 1)
        {
            questState = QuestState.Complete;
            QuestManager.Instance.ev_CheckNpcQuest?.Invoke(this);
        }
    }
}

[System.Serializable]
public class MiniQuestData
{
    public int number;  //퀘스트 번호
    public int mainNumber;  //메인 퀘스트 번호
    public int characterIndex;  //NPC 인덱스
    public string questName;    //퀘스트 이름
    public string description;  //미니 퀘스트 설명
    public int dialogIndex; //퀘스트 대화 인덱스
    public int level;   //퀘스트 레벨 제한
    public QuestType questType; //퀘스트 타입

    public int goalAmount;  //퀘스트 타입이 Battle일 경우 퀘스트 달성 목표
    public int goalNpc; //  퀘스트 타입이 Social일 경우 대화할 목표 Npc
    public int goldReward;  // 퀘스트 골드 보상
    public int expReward;   // 퀘스트 경험치 보상
    public int itemReward;  // 퀘스트 아이템 보상


    public int nowGoalAmount = 0;   //현재 퀘스트 진행량
}

public enum QuestState
{
    None, Ready, Accept, Complete
}

public enum QuestType
{
    Battle, Social
}
