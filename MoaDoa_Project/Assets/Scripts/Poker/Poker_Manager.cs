using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Poker_Manager : MonoBehaviour
{
    public GameObject poker_Canvas;

    private List<int> player_Hand;
    private List<int> dealer_Hand;

    // (�ӽ�)
    public Sprite[] card_Set;

    // ������ ������ Enum���� ����.
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

    //�ӽ� ���� ���� ��� �Լ�.
    public void poker_Start()
    {
        init_Hand(); // �ڵ� �ʱ�ȭ.
        deal_Hand(); // �ڵ� �й�.
        display_Card(); // (�ӽ�) ī�� ǥ��
    }

    // �ߺ� ���� ī�� �Ҵ� �Լ�.
    // ī��� A ~ F = 1 ~ 6�� ������ ���´�.
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


    // List ���� �տ��� ����, �ڿ��� ������ �̷�� �ִ� ���ڵ�.
    public List<int> calc_PokerRank(List<int> hand)
    {
        int[] card_Cnt = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        
        // Ǯ�Ͽ콺, �� ��� üũ�� ����.
        int pair = 0;
        bool three = false;        
        
        // ���� ����, ���� ī���� ��� ���� ������ ī��� ���.
        int topCard = -1;        
        List<int> result = new List<int>();
        // �� ī���� ���� +;
        foreach (int num in hand)
        {
            card_Cnt[num]++;
        }

        // A ~ F���� ��ȸ�ϸ鼭 �� ī���� ���� üũ.        
        // ž ī�� Ȯ��.
        for (int i = 1; i <= 6; i++)
        {            
            if (card_Cnt[i] == 2)
            {
                pair++;
                // Ʈ������ ���� ���� ž ī�� ����
                if (three != true)
                    topCard = i;
            }
            else if (card_Cnt[i] == 3)
            {
                three = true;
                topCard = i;                
            }
            // ��ī��, ���� ���⼭ Ȯ��.
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
            // TwoPair, ū ��, ���� ��, �� ��
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
            // OnePair, ��, �� �� ī�� ��������.
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
            // Top, ī�� ��������.
            result.Add((int)Ranks.Top);
            for (int i = 6; i >= 1; i--)
            {                
                if (card_Cnt[i] == 1)
                    result.Add(i);
            }
        }

        return result;
    }

    // ���� ���� �Լ�.
    public void judge_Poker()
    {
        List<int> player_Score = calc_PokerRank(player_Hand);
        List<int> dealer_Score = calc_PokerRank(dealer_Hand);
                       
        for(int i = 0; i < player_Score.Count; i++)
            Debug.Log("�÷��̾� ���ھ� " + i + " : "+ player_Score[i]);
        for (int i = 0; i < dealer_Score.Count; i++)
            Debug.Log("���� ���ھ� " + i + " : " + dealer_Score[i]);

        if (player_Score[0] < dealer_Score[0])
            Debug.Log("���� ��");
        else if (player_Score[0] > dealer_Score[0])
            Debug.Log("�÷��̾� ��");
        else // ���º��� ���.
        {
            for(int i = 1; i < player_Score.Count; i++)
            {
                if (player_Score[i] < dealer_Score[i])
                {
                    Debug.Log("���� ��");
                    return;
                }
                else if (player_Score[i] > dealer_Score[i])
                {
                    Debug.Log("�÷��̾� ��");
                    return;
                }
            }
            Debug.Log("���º�");
        }            
    }

    // �� �ʱ�ȭ �Լ�.
    void init_Hand()
    {
        player_Hand = new List<int>();
        dealer_Hand = new List<int>();
    }

    // (�ӽ�) ī�� ǥ�� �Լ�.
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
