using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildManager : MonoBehaviour
{   
    static public BuildManager instance = null;    
    public Tilemap tilemap;    
    public bool canBuild = false; // 배치 가능 상태인지 체크.

    // 임시.    
    [SerializeField]
    TileBase build_33;
    [SerializeField]
    GameObject build_33_GameObject;

    private GameObject build_Clone;    

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
    // 임시
    public void click_BuildItem()
    {
        // 배치 가능 상태로 변경.
        // build_33_GameObject 자리에 프리팹 할당하고.
        // build_Item에서 인스턴스화 할거.

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
            build_Clone = Instantiate(build_33_GameObject);        
        build_Clone.transform.position = mousePos;

        // 좌클릭 하는 순간 배치.
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 To 그리드.
            Vector3Int gridPos = tilemap.WorldToCell(mousePos); 
            tilemap.SetTile(gridPos, build_33);

            Destroy(build_Clone);
            canBuild = false;
        }
    }

    public bool can_Build()
    {
        // 건설 가능한 지역인지 확인하는 함수.
        return true;
    }
}
