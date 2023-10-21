using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Poker_Manager : MonoBehaviour
{
    public GameObject poker_Canvas;

    private List<int> player_Hand;
    private List<int> dealer_Hand;

    // (임시)
    public Sprite[] card_Set;

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

    //임시 게임 시작 기능 함수.
    public void poker_Start()
    {
        init_Hand(); // 핸드 초기화.
        deal_Hand(); // 핸드 분배.
        display_Card(); // (임시) 카드 표시
    }

    // 중복 없이 카드 할당 함수.
    void deal_Hand()
    {        
        bool[] visited = new bool[30];
                
        int cnt = 0;        
        while(cnt < 5)
        {
            int temp = Random.Range(0, 30);            
            if (visited[temp] == true)
                continue;

            visited[temp] = true;
            dealer_Hand.Add(temp % 6);
            cnt++;
        }

        cnt = 0;
        while (cnt < 5)
        {
            int temp = Random.Range(0, 30);            
            if (visited[temp] == true)
                continue;

            visited[temp] = true;
            player_Hand.Add(temp % 6);
            cnt++;
        }
    }


    //  족보 + 탑카드 값을 통해 족보의 세기를 리턴.
    public int calc_PokerRank(List<int> hand)
    {        
        int[] card_Cnt = new int[6] { 0, 0, 0, 0, 0, 0 };
        
        // 족보 체크용 변수.
        int pair = 0;
        bool three = false;
        bool four = false;
        bool five = false;
        
        int topCard = -1;
        int result = 0;
        // 각 카드의 개수 +;
        foreach (int num in hand)
        {
            card_Cnt[num]++;
        }

        // A ~ F까지 순회하면서 각 카드의 개수 체크.        
        // 탑 카드 확정.
        for (int i = 0; i < 6; i++)
        {
            if (card_Cnt[i] == 1)
            {
                // 페어 혹은 트리플이 없을 경우에만 탑 카드 갱신.
                if (pair == 0 && three != true)
                    topCard = i;
            }
            else if (card_Cnt[i] == 2)
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
            else if (card_Cnt[i] == 4)
            {
                four = true;
                topCard = i;
                break;
            }
            else if (card_Cnt[i] == 5)
            {
                five = true;
                topCard = i;
                break;
            }            
        }

        if (five == true)        
            result = (int)Ranks.All + topCard;
        else if (four == true)
            result = (int)Ranks.FourCard + topCard;
        else if (three == true && pair == 1)
            result = (int)Ranks.FullHouse + topCard;
        else if (three == true)
            result = (int)Ranks.Triple + topCard;
        else if (pair == 2)
            result = (int)Ranks.TwoPair + topCard;
        else if (pair == 1)
            result = (int)Ranks.OnePair + topCard;
        else
            result = (int)Ranks.Top + topCard;
        
        return result;
    }

    // 승패 판정 함수.
    public void judge_Poker()
    {
        int player_Score = calc_PokerRank(player_Hand);
        int dealer_Score = calc_PokerRank(dealer_Hand);

        Debug.Log("플레이어 스코어 " + player_Score);
        Debug.Log("딜러 스코어 " + dealer_Score);

        if (player_Score < dealer_Score)
            Debug.Log("딜러 승");
        else if (player_Score > dealer_Score)
            Debug.Log("플레이어 승");
        else // 무승부의 경우.
            return;

    }

    // 덱 초기화 함수.
    void init_Hand()
    {
        player_Hand = new List<int>();
        dealer_Hand = new List<int>();
    }

    // (임시) 카드 표시 함수.
    void display_Card()
    {
        GameObject dealer_Field = poker_Canvas.transform.Find("Dealer_Field").gameObject;
        GameObject player_Field = poker_Canvas.transform.Find("Player_Field").gameObject;
        for (int i = 0; i < 5; i++)
        {            
            dealer_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Set[dealer_Hand[i]];
            player_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Set[player_Hand[i]];
        }
    }
}
