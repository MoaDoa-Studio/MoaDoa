using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Interact_Card : MonoBehaviour, IPointerClickHandler
{
    public int card_Num; // �� ��° ī������.
    public bool isPlayer; // �÷��̾�����, False�� ����.
    public bool canClick = false; // Ŭ�� ������ ��.
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick == false)
            return;

        Poker_Manager.instance.reroll_Card(isPlayer, card_Num);
    }
}
