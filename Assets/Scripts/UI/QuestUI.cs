using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questNameText; //퀘스트 제목
    [SerializeField] private TextMeshProUGUI questDescriptionText;  //퀘스트 내용
    [SerializeField] private TextMeshProUGUI questGoalText; //퀘스트 목표
    [SerializeField] private TextMeshProUGUI questRewardsText;  // 퀘스트 보상

    [SerializeField] private GameObject acceptButton;   //수락 버튼
    [SerializeField] private GameObject rewardButton;   //보상받기 버튼

    private QuestManager questManager;
    private UIManager uiManager;

    public void CloseClick()    //창 닫기 버튼
    {
        if (uiManager == null)
            uiManager = UIManager.Instance;

        uiManager.SetDefaultState();
        this.gameObject.SetActive(false);
    }

    public void AcceptClick()   //퀘스트 수락 버튼
    {
        questManager.AddPlayerQuest(questManager.currentQuest);
        CloseClick();
    }

    public void GetRewardClick() //보상 받기 버튼
    {
        if (questManager == null)
            questManager = QuestManager.Instance;

        if (questManager.currentQuest.goldReward > 0)   //골드 보상
            InventoryManager.Instance.GetGold(questManager.currentQuest.goldReward);

        if (questManager.currentQuest.expReward > 0)    //경험치 보상
            Debug.Log($"{questManager.currentQuest.expReward} 경험치 획득");

        if (questManager.currentQuest.itemReward > 0)   //아이템 보상
            Debug.Log("아이템 획득");

        questManager.RemovePlayerQuest();
        questManager.AddCompleteQuest();
        CloseClick();
    }

    public void SetQuestData(MainQuestData data)    //퀘스트 UI에 현재 퀘스트 정보 세팅
    {
        questNameText.text = data.questName;
        questDescriptionText.text = data.description;
        questGoalText.text = data.miniQuests[0].description;

        if (data.goldReward > -1)
            questRewardsText.text = "골드 : " + data.goldReward.ToString() + "G";
        if (data.expReward > -1)
            questRewardsText.text += System.Environment.NewLine + "경험치 : " + data.expReward.ToString() + "Exp"; //System.Environment.NewLine = 유니티에서는 \n(줄바꿈)이 안되서 이걸로 줄바꿈 명령
        if (data.itemReward > -1)
            questRewardsText.text += System.Environment.NewLine + "아이템 : " + data.itemReward.ToString();

        acceptButton.SetActive(true);
        rewardButton.SetActive(false);
    }

    public void SetRewardData(MainQuestData data)  //퀘스트 완료시 보상 UI세팅
    {
        questNameText.text = data.questName;
        questDescriptionText.text = data.description;
        questGoalText.text = "";

        if (data.goldReward > -1)
            questRewardsText.text = "골드 : " + data.goldReward.ToString() + "G";
        if (data.expReward > -1)
            questRewardsText.text += System.Environment.NewLine + "경험치 : " + data.expReward.ToString() + "Exp"; //System.Environment.NewLine = 유니티에서는 \n(줄바꿈)이 안되서 이걸로 줄바꿈 명령
        if (data.itemReward > -1)
            questRewardsText.text += System.Environment.NewLine + "아이템 : " + data.itemReward.ToString();

        acceptButton.SetActive(false);
        rewardButton.SetActive(true);
    }

    private void Start()
    {
        questManager = QuestManager.Instance;
        uiManager = UIManager.Instance;
    }
}
