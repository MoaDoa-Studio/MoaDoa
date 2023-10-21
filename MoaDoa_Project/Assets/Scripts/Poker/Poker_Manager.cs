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


    //  ���� + žī�� ���� ���� ������ ���⸦ ����.
    public int calc_PokerRank(List<int> hand)
    {        
        int[] card_Cnt = new int[6] { 0, 0, 0, 0, 0, 0 };
        
        // ���� üũ�� ����.
        int pair = 0;
        bool three = false;
        bool four = false;
        bool five = false;
        
        int topCard = -1;
        int result = 0;
        // �� ī���� ���� +;
        foreach (int num in hand)
        {
            card_Cnt[num]++;
        }

        // A ~ F���� ��ȸ�ϸ鼭 �� ī���� ���� üũ.        
        // ž ī�� Ȯ��.
        for (int i = 0; i < 6; i++)
        {
            if (card_Cnt[i] == 1)
            {
                // ��� Ȥ�� Ʈ������ ���� ��쿡�� ž ī�� ����.
                if (pair == 0 && three != true)
                    topCard = i;
            }
            else if (card_Cnt[i] == 2)
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

    // ���� ���� �Լ�.
    public void judge_Poker()
    {
        int player_Score = calc_PokerRank(player_Hand);
        int dealer_Score = calc_PokerRank(dealer_Hand);

        Debug.Log("�÷��̾� ���ھ� " + player_Score);
        Debug.Log("���� ���ھ� " + dealer_Score);

        if (player_Score < dealer_Score)
            Debug.Log("���� ��");
        else if (player_Score > dealer_Score)
            Debug.Log("�÷��̾� ��");
        else // ���º��� ���.
            return;

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
            dealer_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Set[dealer_Hand[i]];
            player_Field.transform.GetChild(i).GetComponent<Image>().sprite = card_Set[player_Hand[i]];
        }
    }
}
