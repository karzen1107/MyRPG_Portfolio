using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SystemManager : Singletone<SystemManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerStatusUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject skillbookUI;

    private UIManager uiManager;

    private bool isMouseLocked;  //마우스가 잠겼는지 확인

    // 이벤트 //
    public UnityAction ev_InitDialog;   //다이얼로그 초기화 이벤트
    public UnityAction<PlayerData> ev_LoadData; //플레이어 데이터 로드 이벤트

    // 속성 //
    public bool IsMouseLocked { get { return isMouseLocked; } set { isMouseLocked = value; } }
    public GameObject Player { get { return player; } }
    public int BeforeSceneNum { get; set; } //이전 씬 번호
    public string BeforSceneName { get; set; }  //이전 씬 이름
    public List<OverrayUINumber> OpendUIList { get; set; }   //오픈된 UI 리스트
    public Vector3 CheckPoint { get; private set; }
    public Quaternion CheckRotate { get; private set; }


    private void SetPlayerPos(PlayerData data)  //플레이어 게임 시작 위치 세팅
    {
        CheckPoint = new Vector3(data.startTrans.startPos[0], data.startTrans.startPos[1], data.startTrans.startPos[2]);
        CheckRotate = new Quaternion(data.startTrans.startRot[0], data.startTrans.startRot[1], data.startTrans.startRot[2], data.startTrans.startRot[3]);
        player.transform.position = CheckPoint;
        player.transform.rotation = CheckRotate;
    }

    public void SetCheckPointPlayer()
    {
        // 플레이어 체크포인트로 이동
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = CheckPoint;
        player.transform.rotation = CheckRotate;
        player.GetComponent<CharacterController>().enabled = true;

        //플레이어 상태 세팅
        player.GetComponent<PlayerState>().ResetPlayer();
    }

    #region 단축키
    public void OnShortcutkey(InputAction.CallbackContext context)  //정보창(c), 인벤토리(i), 스킬북(k), 퀘스트(l), 옵션(o)
    {
        if (!uiManager.DefaultCanvas.activeSelf)
            return;

        if (context.started)
        {
            switch (context.control.name)
            {
                case "k":
                    skillbookUI.SetActive(!skillbookUI.activeSelf);
                    if (skillbookUI.activeSelf)
                        uiManager.SetUIOnLast(skillbookUI);
                    break;
                case "i":
                    inventoryUI.SetActive(!inventoryUI.activeSelf);
                    if (inventoryUI.activeSelf)
                        uiManager.SetUIOnLast(inventoryUI);
                    break;
                case "c":
                    playerStatusUI.SetActive(!playerStatusUI.activeSelf);
                    if (playerStatusUI.activeSelf)
                        uiManager.SetUIOnLast(playerStatusUI);
                    break;
            }
        }
    }

    public void CursorOnOff(InputAction.CallbackContext context)   //마우스 락/언락 메서드 + 게임 종료
    {
        if (context.started)
        {
            DefaultCanvas ui = uiManager.DefaultCanvas.GetComponent<DefaultCanvas>();
            ui.OpenActCheckUI("게임을 종료하시겠습니까?");
            //if (OpendUIList.Count <= 0)  //UI 열린게 하나도 없으면
            //{
            //    if (isMouseLocked)  //마우스 잠금 해제
            //    {
            //        Cursor.lockState = CursorLockMode.Confined;
            //        Cursor.visible = true;
            //        Debug.Log("MouseUnlock");
            //    }
            //    else   //마우스 잠금
            //    {
            //        Cursor.lockState = CursorLockMode.Locked;
            //        Cursor.visible = false;
            //        Debug.Log("MouseLock");
            //    }
            //}
            //else  //UI 열린게 있다면 UI 닫기
            //{
            //    uiManager.CloseUI(OpendUIList[OpendUIList.Count - 1]);
            //    OpendUIList.RemoveAt(OpendUIList.Count - 1);
            //}
        }
    }

    public void SaveKey()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveLoadData.SaveData();
            uiManager.ShowCaution(CautionType.SaveDone);
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        GameObject tempPlayer = Instantiate(player);
        player = tempPlayer;
        ev_LoadData += SetPlayerPos;
    }
    private void Start()
    {
        ev_LoadData?.Invoke(SaveLoadData.LoadData());
        uiManager = UIManager.Instance;
        playerStatusUI.SetActive(false);
        inventoryUI.SetActive(false);
        skillbookUI.SetActive(false);

        OpendUIList = new List<OverrayUINumber>();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void LateUpdate()
    {
        SaveKey();

        if (Input.GetKeyDown(KeyCode.G))
        {
            InventoryManager.Instance.GetGold(1000);
        }
    }
}
