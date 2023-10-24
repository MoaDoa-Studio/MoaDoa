using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Interact_Card : MonoBehaviour, IPointerClickHandler
{
    public int card_Num; // 몇 번째 카드인지.
    public bool isPlayer; // 플레이어인지, False면 딜러.
    public bool canClick = false; // 클릭 가능한 지.
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick == false)
            return;

        Poker_Manager.instance.reroll_Card(isPlayer, card_Num);
    }
}
