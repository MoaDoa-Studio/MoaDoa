using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildManager : MonoBehaviour
{   
    static public BuildManager instance = null;    
    public Tilemap tilemap;    
    public bool canBuild = false; // ��ġ ���� �������� üũ.
    private List<string> buildings; // �Է� ���� �ǹ��� ����Ʈ�� ����.

    // ���� ���� ���� ����.
    private GameObject build_Prefab;    
    private TileBase build_TileBase;
    private int build_Size;

    private GameObject build_Clone;
    private bool[,] tiles = new bool[30, 30];
    private int half_Offset = 15; // �迭�� ���� ���� ���� Ŀ���ϱ� ���� �����.
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
        // �ش� �Լ��� �ϴ��� UI�� �ִ� �������� ������ ��, �ߵ��� �Լ�.
        // GameObject�� ������ ���� ���� �Ҵ�.        
        build_Prefab = prefab;
        build_TileBase = prefab.GetComponent<BuildItem>().tilebase;
        build_Size = prefab.GetComponent<BuildItem>().size;
        canBuild = true;
    }

    public void build_Item()
    {
        if (!canBuild)
            return;

        // ���콺 ������.
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        // ���콺�� ����ٴ� Ŭ�� ����.
        if (build_Clone == null)
            build_Clone = Instantiate(build_Prefab);
        build_Clone.transform.position = mousePos;

        // ��Ŭ�� �ϴ� ���� ��ġ.
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 To �׸���.
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);

            // ��ġ�� �� ���� ���̶��?
            if (!check_BuildPos(gridPos, build_Size))
            {
                Debug.Log("��ġ �Ұ�");
                return;
            }

            // ��ġ �����ϴٸ�
            set_BuildPos(gridPos, build_Size);
            tilemap.SetTile(gridPos, build_TileBase);
            Destroy(build_Clone);
            canBuild = false;
        }
    }
    
    public bool check_BuildPos(Vector3Int gridPos, int size)
    {
        // �Ǽ� ������ �������� Ȯ���ϴ� �Լ�.
        // �׸��� ��ǥ, ���� ������.
        int x = gridPos.x + half_Offset;
        int y = gridPos.y + half_Offset;

        // ���� + ������
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
            // ���콺 To �׸���.
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);
            TileBase tile = tilemap.GetTile(gridPos);

            Debug.Log(gridPos);
            if (tile == null)
                Debug.Log("Ÿ���� �����ϴ�.");
            else
                Debug.Log("Ÿ���� �ֽ��ϴ�.");
        }
    }
}
