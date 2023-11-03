using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// 생성할 아이템 프리팹에 부착할 스크립트.
public class BuildItem : MonoBehaviour
{
    // 아이템 이름.
    public string itemName;
    // 아이템 사이즈. (일단 정사각형이라고 생각).
    public int size;
    // 타일 정보
    public TileBase tilebase;
}
