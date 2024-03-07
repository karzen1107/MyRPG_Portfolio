using UnityEngine;

public class EnemyDropPrize : MonoBehaviour
{
    [SerializeField] private Item_SO[] items;    //드랍할 아이템들
    [SerializeField] private int expPoint;  //지급할 경험치
    [SerializeField] private bool dropGold; //골드를 지급할지 말지 결정
    [SerializeField] private int minGold;  //지급할 최소 골드
    [SerializeField] private int maxGold;  //지급할 최대 골드
    private Item_SO goldItem;   //골드 아이템

    // 컴포넌트 //
    private EnemyState enemyState;

    private void DropItems()
    {
        int randomGold = Random.Range(minGold, maxGold);
        goldItem.GoldAmount = randomGold;

        Instantiate(goldItem.ItemPrefab, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));

        for (int i = 0; i < items.Length; i++)
        {
            int randomDrop = Random.Range(0, 2);    //0이면 드롭X, 1이면 드롭O
            if (randomDrop > 0)
                Instantiate(items[i].ItemPrefab, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }

        Debug.Log($"경험치 {expPoint}를 획득했습니다");
    }

    private void Start()
    {
        enemyState = this.GetComponent<EnemyState>();
        enemyState.ev_OnDropItem += DropItems;
        goldItem = Resources.Load("Items/Gold") as Item_SO;
    }
}
