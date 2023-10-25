using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Poker_Manager : MonoBehaviour
{
    public static Poker_Manager instance = null;
    public GameObject poker_Canvas;

    private List<int> player_Hand;
    private List<int> dealer_Hand;
    private int dealer_Num;
    private bool[] valid_Card = new bool[30];
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // (임시)
    public Sprite[] card_Sprite;
    public Sprite secret_Card;
    public int turn;
    public GameObject dealer_Field;
    public GameObject player_Field;
    public Sprite[] dealer_Sprite;
    public Image dealer_Image;
    private bool turn_Change = false;
    public TextMeshProUGUI result_TMP;
    // 족보의 정보를 Enum으로 저장.
    public enum Ranks
    {
        Top = 0,
        OnePair = 10,
        TwoPair = 20,
        Triple = 30,
        FullHouse = 40,
        FourCard = 50,
        All = 60
    }

    //임시 게임 시작 및 초기화 함수.
    public void poker_Start()
    {
        init_Poker();
        deal_Hand();
        set_Dealer();
        canClick_Card(true, false);
        canClick_Card(false, false);
        canClick_Secret_Card(false);

        // 임시
        result_TMP.text = "";
    }
    
    // 필드 및 턴 초기화.
    void init_Poker()
    {                
        for(int i = 0; i < 5; i++)
        {
            player_Field.transform.GetChild(i).GetComponent<Image>().sprite = null;
            dealer_Field.transform.GetChild(i).GetComponent<Image>().sprite = null;
        }
        turn = 0;
        turn_Change = false;
    }    
    // 중복 없이 카드 할당 함수.
    // 카드는 A ~ F = 1 ~ 6의 범위를 갖는다.
    void deal_Hand()
    {
        player_Hand = new List<int>();
        dealer_Hand = new List<int>();
        valid_Card = new bool[30];

        int cnt = 0;        
        while(cnt < 5)
        {
            int temp = Random.Range(0, 30);
            if (valid_Card[temp] == true)
                continue;

            valid_Card[temp] = true;
            dealer_Hand.Add(temp % 6 + 1);
            cnt++;
        }
        cnt = 0;
        while (cnt < 5)
        {
            int temp = Random.Range(0, 30);            
            if (valid_Card[temp] == true)
                continue;

            valid_Card[temp] = true;
            player_Hand.Add(temp % 6 + 1);
            cnt++;
        }
    }

    // 딜러 랜덤 할당 함수.
    void set_Dealer()
    {
        int temp = Random.Range(0, 4);
        dealer_Num = temp;
        dealer_Image.sprite = dealer_Sprite[temp];                
    }

    // List 제일 앞에는 족보, 뒤에는 족보를 이루고 있는 숫자들.
    public List<int> calc_PokerRank(List<int> hand)
    {
        int[] card_Cnt = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        
        // 풀하우스, 투 페어 체크용 변수.
        int pair = 0;
        bool three = false;        
        
        // 동일 족보, 동일 카드의 경우 하위 족보의 카드로 계산.
        int topCard = -1;        
        List<int> result = new List<int>();
        // 각 카드의 개수 +;
        foreach (int num in hand)
        {
            card_Cnt[num]++;
        }

        // A ~ F까지 순회하면서 각 카드의 개수 체크.        
        // 탑 카드 확정.
        for (int i = 1; i <= 6; i++)
        {            
            if (card_Cnt[i] == 2)
            {
                pair++;
                // 트리플이 없을 때만 탑 카드 갱신
                if (three != true)
                    topCard = i;
            }
            else if (card_Cnt[i] == 3)
            {
                three = true;
                topCard = i;                
            }
            // 포카드, 올은 여기서 확정.
            else if (card_Cnt[i] == 4)
            {
                result.Add((int)Ranks.FourCard);
                result.Add(i);
                break;
            }
            else if (card_Cnt[i] == 5)
            {
                result.Add((int)Ranks.All);
                result.Add(i);
                break;
            }            
        }

        if (three == true && pair == 1)
        {
            result.Add((int)Ranks.FullHouse);
            result.Add(topCard);
        }
        else if (three == true)
        {
            result.Add((int)Ranks.Triple);
            result.Add(topCard);
        }
        else if (pair == 2)
        {
            // TwoPair, 큰 쌍, 작은 쌍, 한 장
            result.Add((int)Ranks.TwoPair);

            int oneCard = -1;
            for (int i = 6; i >= 1; i--)
            {
                if (card_Cnt[i] == 2)
                    result.Add(i);
                else if (card_Cnt[i] == 1)
                    oneCard = i;
            }
            result.Add(oneCard);
        }
        else if (pair == 1)
        {
            // OnePair, 쌍, 한 장 카드 내림차순.
            result.Add((int)Ranks.OnePair);
            List<int> oneCard = new List<int>();
            for (int i = 6; i >= 1; i--)
            {
                if (card_Cnt[i] == 2)
                    result.Add(i);
                else if (card_Cnt[i] == 1)
                    oneCard.Add(i);
            }
            result.AddRange(oneCard);
        }
        else
        {
            // Top, 카드 내림차순.
            result.Add((int)Ranks.Top);
            for (int i = 6; i >= 1; i--)
            {                
                if (card_Cnt[i] == 1)
                    result.Add(i);
            }
        }

        return result;
    }
    // 승패 판정 함수.
    public void judge_Poker()
    {
        // secret 패 공개 후.
        for (int i = 0; i < 5; i++)
        {
            dealer_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Sprite[dealer_Hand[i] - 1];
            player_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Sprite[player_Hand[i] - 1];
        }

        List<int> player_Score = calc_PokerRank(player_Hand);
        List<int> dealer_Score = calc_PokerRank(dealer_Hand);

        if (player_Score[0] < dealer_Score[0])
            result_TMP.text = "Dealer Win";
        else if (player_Score[0] > dealer_Score[0])
            result_TMP.text = "Player Win";
        else // 무승부의 경우.
        {
            for (int i = 1; i < player_Score.Count; i++)
            {
                if (player_Score[i] < dealer_Score[i])
                {
                    result_TMP.text = "Dealer Win";
                    return;
                }
                else if (player_Score[i] > dealer_Score[i])
                {
                    result_TMP.text = "Player Win";
                    return;
                }
            }
            result_TMP.text = "Draw";
        }            
    }

    // (임시) 카드 표시 함수.
    void display_Card(int turn)
    {
        if (turn >= 8)        
            return;        
        else
        {
            if (turn_Change == false)
            {
                switch (turn)
                {
                    case 0:
                        dealer_Field.transform.GetChild(0).GetComponent<Image>().sprite = secret_Card;
                        dealer_Field.transform.GetChild(1).GetComponent<Image>().sprite = secret_Card;
                        break;
                    case 1:
                        player_Field.transform.GetChild(0).GetComponent<Image>().sprite = card_Sprite[player_Hand[0] - 1];
                        player_Field.transform.GetChild(1).GetComponent<Image>().sprite = card_Sprite[player_Hand[1] - 1];
                        break;
                    default:
                        if (turn % 2 == 0)
                            dealer_Field.transform.GetChild(turn / 2 + 1).GetComponent<Image>().sprite = card_Sprite[dealer_Hand[turn / 2 + 1] - 1];
                        else
                            player_Field.transform.GetChild(turn / 2 + 1).GetComponent<Image>().sprite = card_Sprite[player_Hand[turn / 2 + 1] - 1];
                        break;
                }
            }
            else
            {
                switch (turn)
                {
                    case 0:                        
                        player_Field.transform.GetChild(0).GetComponent<Image>().sprite = card_Sprite[player_Hand[0] - 1];
                        player_Field.transform.GetChild(1).GetComponent<Image>().sprite = card_Sprite[player_Hand[1] - 1];
                        break;
                    case 1:
                        dealer_Field.transform.GetChild(0).GetComponent<Image>().sprite = secret_Card;
                        dealer_Field.transform.GetChild(1).GetComponent<Image>().sprite = secret_Card;
                        break;
                    default:
                        if (turn % 2 == 0)
                            player_Field.transform.GetChild(turn / 2 + 1).GetComponent<Image>().sprite = card_Sprite[player_Hand[turn / 2 + 1] - 1];
                        else                            
                            dealer_Field.transform.GetChild(turn / 2 + 1).GetComponent<Image>().sprite = card_Sprite[dealer_Hand[turn / 2 + 1] - 1];
                        break;
                }
            }
        }
    }
    // 다음 턴으로 넘겨주는 함수.
    public void turn_Next()
    {
        display_Card(turn);
        turn += 1;        
    }       
    // 현재까지 얻은 카드의 클릭 상태를 변경. 
    // 시크릿 카드 제외.
    void canClick_Card(bool isPlayer, bool toggle)
    {        
        if (isPlayer == true)
        {            
            for (int i = 0; i < player_Hand.Count; i++)            
                player_Field.transform.GetChild(i).GetComponent<Interact_Card>().canClick = toggle;
        }
        else
        {            
            for (int i = 2; i < dealer_Hand.Count; i++)
                dealer_Field.transform.GetChild(i).GetComponent<Interact_Card>().canClick = toggle;
        }        
    }
    // 상대방의 시크릿 카드의 클릭 상태를 변경.
    void canClick_Secret_Card(bool toggle)
    {
        for (int i = 0; i < 2; i++)
        {
            if (dealer_Hand.Count < i)
                break;
            dealer_Field.transform.GetChild(i).GetComponent<Interact_Card>().canClick = toggle;
        }
    }
    // 플레이어, 딜러 여부와 선택한 카드를 받아서, 리롤하는 함수.
    public void reroll_Card(bool isPlayer, int card_Num)
    {
        while (true)
        {
            int temp = Random.Range(0, 30);
            if (valid_Card[temp] == true)
                continue;

            valid_Card[temp] = true;
            if (isPlayer == true)
            {
                player_Hand[card_Num] = temp % 6 + 1;
                player_Field.transform.GetChild(card_Num).GetComponent<Image>().sprite = card_Sprite[player_Hand[card_Num] - 1];
            }
            else
            {
                dealer_Hand[card_Num] = temp % 6 + 1;
                dealer_Field.transform.GetChild(card_Num).GetComponent<Image>().sprite = card_Sprite[dealer_Hand[card_Num] - 1];
            }
            break;
        }
        // 리롤 후 다시 클릭 못하게 수정.
        canClick_Card(true, false);
        canClick_Card(false, false);
        canClick_Secret_Card(false);
    }
    public void use_Skill()
    {
        switch(dealer_Num)
        {
            case 0:
                canClick_Card(true, true);
                break;
            case 1:
                canClick_Card(false, true);
                break;
            case 2:
                canClick_Secret_Card(true);
                break;
            case 3:
                turn_Change = true;
                break;
        }        
    }    
}
