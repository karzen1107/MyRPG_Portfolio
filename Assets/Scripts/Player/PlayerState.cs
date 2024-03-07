using System.Collections.Generic;
using UnityEngine;
///< summary >
/// 플레이어의 상태를 관리하는 클래스
/// </summary>
public class PlayerState : Singletone<PlayerState>
{
    // 장착 아이템 관련 //
    private Item[] usingItems = new Item[(int)EquipType.Max];

    // 상태 확인 //
    private bool isDeath; //플레이어가 죽었는지 확인
    private bool isFirstDamage; //비전투 모드에서 첫데미지를 받은것인지 확인

    //전투/비전투 모드 변환 관련 //
    private float waitTime; // 비전투 모드 변환까지 기다릴 시간
    private float setTime = 5f; // 비전투 모드 변환 세팅 시간

    // 타겟 //
    private GameObject target;

    // 컴포넌트 //
    private Animator ani; // 애니메이션 컴포넌트
    [Header("플레이어")]
    [SerializeField] private Transform playerCenter;    //플레이어 센터
    [SerializeField] private List<Renderer> renderers = new List<Renderer>();
    private CrossHairUI crossHairUI;
    private SystemManager systemManager;
    private UIManager UIManager;

    //속성//
    public bool IsDeath // isDeath 속성
    {
        get { return isDeath; }
        set
        {
            isDeath = value;
            ani.SetBool(PlayerAniVariable.isDeath, value);

            if (isDeath)
                UIManager.Instance.FadeCanvas.GetComponent<FadeInOut>().OutFade(isDeath);
        }
    }
    public bool AttackMode { get; set; }    //AttackMode 속성. 플레이어가 공격모드인지 확인
    public float Exp { get; private set; }  //경험치
    public string CharName { get; private set; }
    public int CharLevel { get; private set; }  //레벨
    public float StartLife { get; private set; } // 최대 라이프
    public float Life { get; private set; } //현재 라이프
    public float StartMana { get; private set; } // 최대 마나
    public float Mana { get; private set; }
    public float StartAtk { get; private set; }
    public float NowAtk { get; set; } // nowAttack 속성
    public float StartShield { get; private set; }
    public float NowShield { get; set; } // nowShield 속성
    public Item[] UsingItems { get { return usingItems; } }
    public GameObject Target { get { return target; } }
    public Vector3 TargetPos { get; private set; }

    public void TargettingAim(Vector3 point)  //타겟 조준 & 비조준 메서드
    {
        Vector3 dir = (point - playerCenter.position).normalized;

        if (Physics.Raycast(playerCenter.position, dir, out RaycastHit hit, 10f, ~LayerMask.NameToLayer("EnemyHitArea"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) //타겟팅 대상이 몬스터라면
            {
                target = hit.collider.gameObject;
                crossHairUI.AimmingTarget(hit.collider.gameObject);
            }
            else
            {
                target = null;
                TargetPos = point;
                crossHairUI.NoAimmingTarget();
            }
        }
        else
        {
            target = null;
            TargetPos = this.gameObject.transform.position;
            crossHairUI.NoAimmingTarget();
        }
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawRay(playerCenter.position, testDir);
    //}

    public void TakeDamage(float damage) // 데미지 입는 메서드
    {
        if (IsDeath)
            return;

        ResetTimer();
        Life -= damage;
        isFirstDamage = true;
        if (Life <= 0)
        {
            Life = 0;
            IsDeath = true;
        }
    }

    public void GiveDamage(EnemyState enemy, float skillDamage)    //최종 데미지 주는 메서드
    {
        if (enemy != null)
        {
            float finalDamage = NowAtk + skillDamage;
            enemy.TakeDamage(finalDamage);
        }
    }

    public void SetAttackMode(bool doDrawAni) //플레이어 전투 모드 전환
    {
        AttackMode = true;
        ani.SetFloat(PlayerAniVariable.attackMode, 1);
        ani.SetLayerWeight(PlayerAniVariable.DrawWeaponLayer, 1);
        ani.SetLayerWeight(PlayerAniVariable.SheathWeaponLayer, 0);
        if (doDrawAni)
            ani.SetTrigger(PlayerAniVariable.drawWeapon);
    }

    public void SetNormalMode() //플레이어 비전투 모드 전환
    {
        AttackMode = false;
        isFirstDamage = false;
        ani.SetFloat(PlayerAniVariable.attackMode, 0);
        ani.SetLayerWeight(PlayerAniVariable.DrawWeaponLayer, 0);
        ani.SetLayerWeight(PlayerAniVariable.SheathWeaponLayer, 1);
        ani.SetTrigger(PlayerAniVariable.sheathWeapon);
        ResetTimer();
    }

    public void ResetTimer() //타이머 초기화 메서드
    {
        waitTime = 0f;
    }

    public void UseMana(float amount) //마나 사용 메서드
    {
        if (Mana < amount)
            Debug.Log("마나가 부족합니다");
        else
            Mana -= amount;
    }

    public void RecoverHp(float amount) //체력 회복
    {
        Life += amount;
        if (Life > StartLife)
        {
            Life = StartLife;
        }
    }

    public void RecoverMp(float amount) //마나 회복
    {
        Mana += amount;
        if (Mana > StartMana)
        {
            Mana = StartMana;
        }
    }

    public void AddPlayerStatus(Item item)  // 아이템 장착 메서드
    {
        switch (item.EquipType)
        {
            case EquipType.Weapon:
                NowAtk += item.ItemAbillity;
                usingItems[(int)EquipType.Weapon] = item;
                break;
            case EquipType.Head:
                NowShield += item.ItemAbillity;
                usingItems[(int)EquipType.Head] = item;
                break;
            case EquipType.Sheild:
                NowShield += item.ItemAbillity;
                usingItems[(int)(EquipType.Sheild)] = item;
                break;
            case EquipType.Armor:
                NowShield += item.ItemAbillity;
                usingItems[(int)(EquipType.Armor)] = item;
                break;
            case EquipType.Gloves:
                NowShield += item.ItemAbillity;
                usingItems[(int)(EquipType.Gloves)] = item;
                break;
            case EquipType.Pants:
                NowShield += item.ItemAbillity;
                usingItems[(int)(EquipType.Pants)] = item;
                break;
            case EquipType.Belt:
                NowShield += item.ItemAbillity;
                usingItems[(int)(EquipType.Belt)] = item;
                break;
            case EquipType.Boots:
                NowShield += item.ItemAbillity;
                usingItems[(int)(EquipType.Boots)] = item;
                break;
        }
    }

    public void RemovePlayerStatus(Item item)   //아이템 장착 해제 메서드
    {
        switch (item.EquipType)
        {
            case EquipType.Weapon:
                NowAtk -= item.ItemAbillity;
                usingItems[(int)EquipType.Weapon] = null;
                break;
            case EquipType.Head:
                NowShield -= item.ItemAbillity;
                usingItems[(int)EquipType.Head] = null;
                break;
            case EquipType.Sheild:
                NowShield -= item.ItemAbillity;
                usingItems[(int)(EquipType.Sheild)] = null;
                break;
            case EquipType.Armor:
                NowShield -= item.ItemAbillity;
                usingItems[(int)(EquipType.Armor)] = null;
                break;
            case EquipType.Gloves:
                NowShield -= item.ItemAbillity;
                usingItems[(int)(EquipType.Gloves)] = null;
                break;
            case EquipType.Pants:
                NowShield -= item.ItemAbillity;
                usingItems[(int)(EquipType.Pants)] = null;
                break;
            case EquipType.Belt:
                NowShield -= item.ItemAbillity;
                usingItems[(int)(EquipType.Belt)] = null;
                break;
            case EquipType.Boots:
                NowShield -= item.ItemAbillity;
                usingItems[(int)(EquipType.Boots)] = null;
                break;
        }
    }

    private void SetAblePlayer(bool isAble) //플레이어 작동 활성화/비활성화
    {
        this.gameObject.GetComponent<PlayerMove>().enabled = isAble;
        this.gameObject.GetComponent<PlayerQuickSlotHandler>().enabled = isAble;
    }

    private void SetAblePlayerMesh(bool isAble) //플레이어 메쉬 활서화/비활성화
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].enabled = isAble;
        }
    }

    private void LoadPlayerData(PlayerData data) //플레이어 데이터 로드
    {
        CharName = data.charName;
        CharLevel = data.charLevel;
        StartLife = data.startLife;
        Life = StartLife;
        StartMana = data.startMana;
        Mana = StartMana;
        Exp = data.exp;
        StartAtk = data.startAtk;
        StartShield = data.startShield;
    }

    public void ResetPlayer()
    {
        LoadPlayerData(SaveLoadData.LoadData());
        IsDeath = false;
        AttackMode = false;
        isFirstDamage = false;
        ani.SetFloat(PlayerAniVariable.attackMode, 0);
        ResetTimer();
    }

    protected override void Awake()
    {
        base.Awake();
        systemManager = SystemManager.Instance;
        systemManager.ev_LoadData += LoadPlayerData;
    }

    private void Start()
    {
        ani = this.GetComponent<Animator>();
        crossHairUI = FindObjectOfType<CrossHairUI>();

        Life = StartLife;
        Mana = StartMana;
        NowAtk = StartAtk;
        NowShield = StartShield;
        target = null;
    }

    private void Update()
    {
        if (ani.GetBool(PlayerAniVariable.isDeath))
            return;

        if (CameraControls.Instance.IsNpcInteracting)    //플레이어 작동 활성화/비활성화
        {
            SetAblePlayer(false);
            SetAblePlayerMesh(false);
        }
        else
        {
            SetAblePlayer(true);
            SetAblePlayerMesh(true);
        }

        #region 전투/비전투 모드 전환
        if (AttackMode == true) // 비전투 모드 전환
        {
            waitTime += Time.deltaTime;
            if (waitTime >= setTime)
                SetNormalMode();
        }
        else if (AttackMode == false && isFirstDamage)  //전투 모드 전환
            SetAttackMode(true);
        #endregion
    }
}

[System.Serializable]
public class SavedPlayerTransform   //플레이어 위치 세이브&로드 용
{
    public float[] startPos = new float[3];
    public float[] startRot = new float[4];

    public void SetStartPos(float x, float y, float z)
    {
        startPos[0] = x;
        startPos[1] = y;
        startPos[2] = z;
    }

    public void SetStartRot(float x, float y, float z, float w)
    {
        startRot[0] = x;
        startRot[1] = y;
        startRot[2] = z;
        startRot[3] = w;
    }
}
