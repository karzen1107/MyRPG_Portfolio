using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : Singletone<QuestManager>
{
    // Xml 다이얼로그 //
    [SerializeField] private string mainXmlFile = "MainQuest";  //Resources. 파일 경로(메인)
    [SerializeField] private string miniXmlFile = "MiniQuest";  //Resources. 파일 경로(미니)
    [SerializeField] private string mainNodeString = "root/mainQuest";  //파일 내 노드 경로(메인)
    [SerializeField] private string miniNodeString = "root/miniQuest";  //파일 내 노드 경로(미니)

    private XmlNodeList mainAllNodes;   //메인 퀘스트 모든 노드 리스트
    private XmlNodeList miniAllNodes;   //미니 퀘스트 모든 노드 리스트

    // 퀘스트 데이터 //
    [SerializeField] private List<MainQuestData> playerAcceptQuestData = new List<MainQuestData>();   //플레이어가 갖고 있는 퀘스트 데이터
    [SerializeField] private List<MainQuestData> playerCompleteQuestData = new List<MainQuestData>();   //플레이어가 수행 완료한 퀘스트 데이터
    public MainQuestData currentQuest;  //현재 퀘스트

    // 이벤트 대리자 //
    public UnityAction<MainQuestData> ev_SetOngoingQuest;
    public UnityAction<MiniQuestData> ev_UpdateOngoingQuest;
    public UnityAction<MainQuestData> ev_RemoveOngoingQuest;
    public UnityAction<MainQuestData> ev_CheckNpcQuest;
    public Func<List<OngoingQuestUI>> func_ReturnOngoingQuests;

    // 컴포넌트 //
    private SystemManager systemManager;

    public List<MainQuestData> PlayerAcceptQuestData { get { return playerAcceptQuestData; } }
    public List<MainQuestData> PlayerCompleteQuestData { get { return playerCompleteQuestData; } }
    public List<GameObject> OngoingQuests { get; set; }

    private void LoadQuestXml()     //Xml파일 불러오기
    {
        // 메인퀘스트 데이터 불러오기 //
        var mainXmlAsset = Resources.Load<TextAsset>("Dialog/" + mainXmlFile);

        XmlDocument mainXmlDoc = new XmlDocument();
        mainXmlDoc.LoadXml(mainXmlAsset.text);
        mainAllNodes = mainXmlDoc.SelectNodes(mainNodeString);

        // 미니퀘스트 데이터 불러오기 miniXmlFile
        var miniXmlAsset = Resources.Load<TextAsset>("Dialog/" + miniXmlFile);

        XmlDocument miniXmlDoc = new XmlDocument();
        miniXmlDoc.LoadXml(miniXmlAsset.text);
        miniAllNodes = miniXmlDoc.SelectNodes(miniNodeString);
    }

    public List<MainQuestData> GetNpcQuests(int npcIndex)   //각 NPC에게 맞는 퀘스트 분배
    {
        List<MainQuestData> quests = new List<MainQuestData>();

        foreach (XmlNode node in mainAllNodes)
        {
            if (npcIndex == int.Parse(node["character"].InnerText))
            {
                MainQuestData data = new MainQuestData();

                data.number = int.Parse(node["number"].InnerText);
                data.characterIndex = int.Parse(node["character"].InnerText);
                data.questName = node["questName"].InnerText;
                data.description = node["description"].InnerText;
                data.dialogIndex = int.Parse(node["dialogIndex"].InnerText);
                data.level = int.Parse(node["level"].InnerText);

                data.goalAmount = int.Parse(node["goalAmount"].InnerText);
                data.goldReward = int.Parse(node["gold"].InnerText);
                data.expReward = int.Parse(node["exp"].InnerText);
                data.itemReward = int.Parse(node["item"].InnerText);

                // 메인 퀘스트 데이터에 미니 퀘스트 데이터 삽입 //
                foreach (XmlNode miniNode in miniAllNodes)
                {
                    if (data.number == int.Parse(miniNode["mainNumber"].InnerText))
                    {
                        MiniQuestData mdata = new MiniQuestData();
                        mdata.number = int.Parse(miniNode["number"].InnerText);
                        mdata.mainNumber = int.Parse(miniNode["mainNumber"].InnerText);
                        mdata.characterIndex = int.Parse(miniNode["character"].InnerText);
                        mdata.questName = miniNode["questName"].InnerText;
                        mdata.description = miniNode["description"].InnerText;
                        mdata.dialogIndex = int.Parse(miniNode["dialogIndex"].InnerText);
                        mdata.level = int.Parse(miniNode["level"].InnerText);

                        switch (miniNode["questType"].InnerText)
                        {
                            case "Battle":
                                mdata.questType = QuestType.Battle;
                                break;
                            case "Social":
                                mdata.questType = QuestType.Social;
                                break;
                        }

                        mdata.goalAmount = int.Parse(miniNode["goalAmount"].InnerText);
                        mdata.goalNpc = int.Parse(miniNode["goalNpc"].InnerText);
                        mdata.goldReward = int.Parse(miniNode["gold"].InnerText);
                        mdata.expReward = int.Parse(miniNode["exp"].InnerText);
                        mdata.itemReward = int.Parse(miniNode["item"].InnerText);

                        data.miniQuests.Add(mdata);
                    }
                }
                quests.Add(data);
            }
        }
        return quests;
    }

    public MiniQuestData GetTargetQuests(int targetIndex)   //각 퀘스트 타겟들에게 맞는 퀘스트 분배
    {
        foreach (XmlNode node in miniAllNodes)
        {
            if (targetIndex == int.Parse(node["number"].InnerText))
            {
                MiniQuestData data = new MiniQuestData();
                data.number = int.Parse(node["number"].InnerText);
                data.mainNumber = int.Parse(node["mainNumber"].InnerText);
                data.characterIndex = int.Parse(node["character"].InnerText);
                data.questName = node["questName"].InnerText;
                data.description = node["description"].InnerText;
                data.dialogIndex = int.Parse(node["dialogIndex"].InnerText);
                data.level = int.Parse(node["level"].InnerText);

                switch (node["questType"].InnerText)
                {
                    case "Battle":
                        data.questType = QuestType.Battle;
                        break;
                    case "Social":
                        data.questType = QuestType.Social;
                        break;
                }

                data.goalAmount = int.Parse(node["goalAmount"].InnerText);
                data.goalNpc = int.Parse(node["goalNpc"].InnerText);
                data.goldReward = int.Parse(node["gold"].InnerText);
                data.expReward = int.Parse(node["exp"].InnerText);
                data.itemReward = int.Parse(node["item"].InnerText);

                return data;
            }
        }

        return null;
    }

    public QuestState GetQuestState(MainQuestData NpcQuestdata) //플레이어 퀘스트 리스트 중 지정한 퀘스트의 상태 반환
    {
        QuestState qState = QuestState.Ready;
        currentQuest = NpcQuestdata;

        foreach (var questData in playerAcceptQuestData)
        {
            if (questData.number == NpcQuestdata.number)
            {
                qState = questData.questState;

                if (NpcQuestdata.miniQuests.Count <= 1)
                    qState = QuestState.Complete;
            }
        }
        return qState;
    }

    public void AddPlayerQuest(MainQuestData questData) //플레이어 퀘스트 추가(퀘스트 수락시)
    {
        if (questData == null)
            return;

        questData.questState = QuestState.Accept;
        playerAcceptQuestData.Add(questData);
        ev_SetOngoingQuest?.Invoke(questData);
        ev_CheckNpcQuest?.Invoke(questData);
    }

    public void UpdateCurrentQuest(MiniQuestData quest)    //진행중인 퀘스트 업데이트
    {
        for (int i = 0; i < playerAcceptQuestData.Count; i++)
        {
            if (playerAcceptQuestData[i].miniQuests[0].number == quest.number)
            {
                playerAcceptQuestData[i].UpdateBattleQuest();
                ev_UpdateOngoingQuest?.Invoke(playerAcceptQuestData[i].miniQuests[0]);
            }
        }
    }

    public void RemovePlayerQuest() //플레이어 퀘스트 제거
    {
        if (currentQuest == null)
            return;

        for (int i = 0; i < playerAcceptQuestData.Count; i++)
        {
            if (playerAcceptQuestData[i].number == currentQuest.number)
            {
                playerAcceptQuestData.RemoveAt(i);
                ev_RemoveOngoingQuest?.Invoke(currentQuest);
            }
        }
    }

    public void AddCompleteQuest()  //완료한 퀘스트 데이터 추가
    {
        if (currentQuest == null)
            return;

        playerCompleteQuestData.Add(currentQuest);
    }

    public void LoadPlayerQuestData(PlayerData data)    //플레이어 퀘스트 데이터 로드
    {
        if (data == null)
            return;

        for (int i = 0; i < data.playerAcceptQuestData.Count; i++)
        {
            AddPlayerQuest(data.playerAcceptQuestData[i]);
        }

        for (int i = 0; i < data.playerCompleteQuestData.Count; i++)
        {
            playerCompleteQuestData.Add(data.playerCompleteQuestData[i]);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        LoadQuestXml();
        systemManager = SystemManager.Instance;
        systemManager.ev_LoadData += LoadPlayerQuestData;
    }
}
