using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildManager : MonoBehaviour
{   
    static public BuildManager instance = null;    
    public Tilemap tilemap;    
    public bool canBuild = false; // ��ġ ���� �������� üũ.

    // �ӽ�.    
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
    // �ӽ�
    public void click_BuildItem()
    {
        // ��ġ ���� ���·� ����.
        // build_33_GameObject �ڸ��� ������ �Ҵ��ϰ�.
        // build_Item���� �ν��Ͻ�ȭ �Ұ�.

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
            build_Clone = Instantiate(build_33_GameObject);        
        build_Clone.transform.position = mousePos;

        // ��Ŭ�� �ϴ� ���� ��ġ.
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 To �׸���.
            Vector3Int gridPos = tilemap.WorldToCell(mousePos); 
            tilemap.SetTile(gridPos, build_33);

            Destroy(build_Clone);
            canBuild = false;
        }
    }

    public bool can_Build()
    {
        // �Ǽ� ������ �������� Ȯ���ϴ� �Լ�.
        return true;
    }
}
