using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OngoingQuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitle;    //퀘스트 제목
    [SerializeField] private TextMeshProUGUI questDescription;  //퀘스트 내용
    [SerializeField] private TextMeshProUGUI questGoal; //퀘스트 통과 골
    [SerializeField] private Image backGroundImg;   //뒷 배경
    [SerializeField] private GameObject CloseButton;    //닫기 버튼

    // 컴포넌트 //
    private QuestManager questManager;

    // 플레이 씬에 보일 진행중인 퀘스트 데이터 //
    private OngoingQuest onQuest = new OngoingQuest();

    // 속성 //
    public OngoingQuest OnQuest { get { return onQuest; } }

    public void SetQuestData(MainQuestData data)    //퀘스트 데이터 세팅
    {
        onQuest.mainQuest = data;

        questTitle.text = data.questName;
        UpdateMiniQuest(data.miniQuests[0]);
    }

    public void UpdateMiniQuest(MiniQuestData data) //미니 퀘스트 데이터 업데이트
    {
        onQuest.miniQuest = data;

        questDescription.text = data.description;
        if (data.goalAmount < 1)
            questGoal.text = "";
        else
            questGoal.text = $"{data.nowGoalAmount} / {data.goalAmount}";
    }

    public void LoadOnQuestData(OngoingQuest qdata) //진행중인 퀘스트 데이터 로드
    {
        if (qdata == null)
            return;

        SetQuestData(qdata.mainQuest);
        UpdateMiniQuest(qdata.miniQuest);
    }

    public void HighLightedQuest()  //해당 퀘스트에 마우스 업 했을때
    {

    }

    public void CloseClick()    //퀘스트 닫기 클릭 버튼
    {

    }

    private void Start()
    {
        questManager = QuestManager.Instance;
        questManager.ev_UpdateOngoingQuest += UpdateMiniQuest;
    }
}

[System.Serializable]
public class OngoingQuest
{
    public MainQuestData mainQuest;
    public MiniQuestData miniQuest;
}
