using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildManager : MonoBehaviour
{   
    static public BuildManager instance = null;    
    public Tilemap tilemap;    
    public bool canBuild = false; // 배치 가능 상태인지 체크.
    private List<string> buildings; // 입력 받은 건물을 리스트로 보관.

    // 빌딩 정보 저장 변수.
    private GameObject build_Prefab;    
    private TileBase build_TileBase;
    private int build_Size;

    private GameObject build_Clone;
    private bool[,] tiles = new bool[30, 30];
    private int half_Offset = 15; // 배열의 반은 음수 값을 커버하기 위해 사용함.
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (canBuild)
        {
            build_Item();
        }        
    }
    
    public void click_Building(GameObject prefab)
    {
        // 해당 함수는 하단의 UI에 있는 아이템을 눌렀을 때, 발동할 함수.
        // GameObject에 선택한 빌딩 정보 할당.        
        build_Prefab = prefab;
        build_TileBase = prefab.GetComponent<BuildItem>().tilebase;
        build_Size = prefab.GetComponent<BuildItem>().size;
        canBuild = true;
    }

    public void build_Item()
    {
        if (!canBuild)
            return;

        // 마우스 포지션.
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        // 마우스에 따라다닐 클론 생성.
        if (build_Clone == null)
            build_Clone = Instantiate(build_Prefab);
        build_Clone.transform.position = mousePos;

        // 좌클릭 하는 순간 배치.
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 To 그리드.
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            // 설치할 수 없는 곳이라면?
            if (!check_BuildPos(gridPos, build_Size))
            {
                Debug.Log("설치 불가");
                return;
            }

            // 설치 가능하다면
            set_BuildPos(gridPos, build_Size);
            tilemap.SetTile(gridPos, build_TileBase);
            Destroy(build_Clone);
            canBuild = false;
        }
    }
    
    public bool check_BuildPos(Vector3Int gridPos, int size)
    {
        // 건설 가능한 지역인지 확인하는 함수.
        // 그리드 좌표, 빌딩 사이즈.
        int x = gridPos.x + half_Offset;
        int y = gridPos.y + half_Offset;

        // 지역 + 사이즈
        for (int i = x; i < x + size; i++)
        {
            for(int j = y; j < y + size; j++)
            {
                if (tiles[j, i] == true)
                    return false;
            }
        }
        return true;
    }

    public void set_BuildPos(Vector3Int gridPos, int size)
    {
        int x = gridPos.x + half_Offset;
        int y = gridPos.y + half_Offset;

        for (int i = x; i < x + size; i++)
        {
            for (int j = y; j < y + size; j++)
            {                
                tiles[j, i] = true;
            }
        }
    }

    public void check_Tiles()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 To 그리드.
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);
            TileBase tile = tilemap.GetTile(gridPos);

            Debug.Log(gridPos);
            if (tile == null)
                Debug.Log("타일이 없습니다.");
            else
                Debug.Log("타일이 있습니다.");
        }
    }
}
