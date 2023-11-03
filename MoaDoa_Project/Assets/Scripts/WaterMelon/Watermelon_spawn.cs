using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ڰ��� Spawn ��ġ �� ����
public class WaterMelon_Spawn : MonoBehaviour
{
    public AudioClip clip;
    [SerializeField]
    GameObject waiting;

    [SerializeField]
    GameObject spawnObj;

    public GameObject moving_Point;
    public GameObject[] gameObjects;
    public Transform waitPos; 
    public Queue<GameObject>[] firstTwoObjects = new Queue<GameObject>[2];

    List<Queue<GameObject>> resultList = new List<Queue<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        Spawning_waitingObject();
        Making_waitObj();
        Spawning_waitingObject();
        Making_waitObj();
    }

    // Update is called once per frame
    void Update()
    {
        // Making waitingObj


        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.y = spawnObj.transform.position.y;
            AudioManager.instance.DropPlay("Drop", clip);
            spawnObj.transform.position = mousePos;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            giving_Gravitiy();
            spawnObj = null;
            Proccessing();
            // 기다리는놈을 뽑음
            Spawning_waitingObject();
            Making_waitObj();
         }
        
       
    }


    void Spawning_waitingObject()
    {
         float probability1 = 0.6f;
         float probability2 = 0.35f;
         float probability3 = 0.2f;

         float randomValue = Random.Range(0f, 1f);

        Queue<GameObject> resultQueue = new Queue<GameObject>();

        if (randomValue <= probability1)
        {

            waiting = gameObjects[0];
            return;
        }
        if (randomValue <= probability1 + probability2)
        {
            waiting = gameObjects[0];
            return;
        }
        if (randomValue <= probability1 + probability2 + probability3)
        {
            waiting = gameObjects[0];
            return;
        }

    }

    void Making_waitObj()
    {
       
        GameObject startObj = Instantiate(waiting);
        startObj.transform.position = waitPos.position;
    
        if(spawnObj == null)
        {
            startObj.transform.position = this.transform.position;
            spawnObj = startObj;
        }
       
    }

    void Proccessing()
    {
        if (spawnObj == null)
            spawnObj = waiting;
            spawnObj.transform.position = this.transform.position;
    }


    void giving_Gravitiy()
    {
        spawnObj.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
    void Setting_spawnObj()
    {

    }
}

        // 그 다음껄 눌렀을떄 떨어뜨려야함













/*
    // ������ ������ �޼���
    void Spawning_WatermelonSoul(int numIteration)
    {
        //������� ������ �迭����
        GameObject[] results = new GameObject[numIteration];

        for (int i = 0; i < numIteration; i++)
        {
            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= probability1)
            {
                results[i] = gameObjects[0];

            }
            else if (randomValue <= probability1 + probability2)
            {
                results[i] = gameObjects[1];

            }
            else
            {
                results[i] = gameObjects[2];

            }


            //������� ���ڿ��� ���
            string resultsString = "";
            for (int j = 0; j < results.Length; j++)
            {
                resultsString += results[i].name;

                if (j < results.Length - 1)
                {
                    resultsString += ", ";

                }
            }

            Debug.Log("Results: " + resultsString);
        }

        //���� ���۽ÿ� �ϳ��� ������Ʈ�� ��ġ�ؾ���
        //i ��� ���߾Ӱ� i+1�̶�� ���� UI�� ��ġ�ؾ���
    }




        Void AnotherFunction()
{

        GameObject instantiatedObject = InstantiatePrefabAndGetReference();
        if(instatiatedObject != null)
        {
            
        }


*/