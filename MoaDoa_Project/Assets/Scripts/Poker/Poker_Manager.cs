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
    // 카드는 A ~ F = 1 ~ 6의 범위를 갖는다.
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
            dealer_Hand.Add(temp % 6 + 1);
            cnt++;
        }

        cnt = 0;
        while (cnt < 5)
        {
            int temp = Random.Range(0, 30);            
            if (visited[temp] == true)
                continue;

            visited[temp] = true;
            player_Hand.Add(temp % 6 + 1);
            cnt++;
        }
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
        List<int> player_Score = calc_PokerRank(player_Hand);
        List<int> dealer_Score = calc_PokerRank(dealer_Hand);
                       
        for(int i = 0; i < player_Score.Count; i++)
            Debug.Log("플레이어 스코어 " + i + " : "+ player_Score[i]);
        for (int i = 0; i < dealer_Score.Count; i++)
            Debug.Log("딜러 스코어 " + i + " : " + dealer_Score[i]);

        if (player_Score[0] < dealer_Score[0])
            Debug.Log("딜러 승");
        else if (player_Score[0] > dealer_Score[0])
            Debug.Log("플레이어 승");
        else // 무승부의 경우.
        {
            for(int i = 1; i < player_Score.Count; i++)
            {
                if (player_Score[i] < dealer_Score[i])
                {
                    Debug.Log("딜러 승");
                    return;
                }
                else if (player_Score[i] > dealer_Score[i])
                {
                    Debug.Log("플레이어 승");
                    return;
                }
            }
            Debug.Log("무승부");
        }            
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
            dealer_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Set[dealer_Hand[i] - 1];
            player_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Set[player_Hand[i] - 1];
        }
    }
}
