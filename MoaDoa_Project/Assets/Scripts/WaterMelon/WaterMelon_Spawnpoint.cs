using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ڰ��� Spawn ��ġ �� ����
public class WaterMelon_Spawnpoint : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] gameObjects;

    // UI�����ϱ�
    
    public GameObject spawnObject; 
    public GameObject waitingObject;

    public Transform waiting_Point;

    // gamemanager���� ������ ������Ʈ
    public Queue<GameObject>[] firstTwoObjects = new Queue<GameObject>[2];

    // ������� ������ List ����
    List<Queue<GameObject>> resultList = new List<Queue<GameObject>>();
    
    private float probability1 = 0.45f;
    private float probability2 = 0.35f;
    private float probability3 = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // �ʹݿ� 30�� ����
        Spawning_Objects(30);
        Setting_Process();
    }

    // Update is called once per frame
    void Update()
    {
        Checklist_Watermelon(20);
        
        if(Input.GetMouseButtonUp(0))
        {
            Input_Proccess();
        }
        
    }
    void Spawning_Objects(int numIterations)
    {
        

        for (int i = 0; i < numIterations; i++)
        {
            float randomValue = Random.Range(0f, 1f);

            Queue<GameObject> resultQueue = new Queue<GameObject>();

            if (randomValue <= probability1)
            {
                resultQueue.Enqueue(gameObjects[0]);
            }
            if (randomValue <= probability1 + probability2)
            {
                resultQueue.Enqueue(gameObjects[1]);
            }
            if (randomValue <= probability1 + probability2 + probability3)
            {
                resultQueue.Enqueue(gameObjects[2]);
            }

            //��� ť�� List �߰�
            resultList.Add(resultQueue); 
        }

        firstTwoObjects[0] = resultList[0];
        firstTwoObjects[1] = resultList[1];
        
        // �ʱ�ȭ���ÿ��� �տ��� �ΰ��� ������
        resultList.RemoveRange(0, 2);


    }

    // firstTwoObjects[3,4] / UI[1.2] ���ÿϷ�
    void Setting_Process()
    {
        // ó�� UI ���� ��������
        spawnObject = firstTwoObjects[0].Dequeue();
        waitingObject = firstTwoObjects[1].Dequeue();

        Instantiate(spawnObject, this.transform.position, Quaternion.identity);

        Instantiate(waitingObject, waiting_Point.transform.position, Quaternion.identity);
           
        if (firstTwoObjects[0] == null)
        {
            firstTwoObjects[0] = resultList[0];
            firstTwoObjects[1] = resultList[1];

            resultList.RemoveRange(0, 2);

        }
        else if (firstTwoObjects[1] == null)
        {
            firstTwoObjects[1] = resultList[0];

            resultList.RemoveRange(0, 1);

        }
            
    }
       
   

   // ��ư ������ ���μ���
   void Input_Proccess()
    {
        // ��ư�� ���������� 
        spawnObject = waitingObject;
        Instantiate(spawnObject, this.transform.position, Quaternion.identity);

        waitingObject = firstTwoObjects[0].Dequeue();
        Instantiate(waitingObject, waiting_Point.transform.position, Quaternion.identity);

        // �� ��° ť�� ù���� ��Ҹ� ù ���� ť�� �ű�ϴ�
        firstTwoObjects[0].Enqueue(firstTwoObjects[1].Dequeue());

        GameObject objToMove = resultList[0].Dequeue();

        firstTwoObjects[1].Enqueue(objToMove);


    }


    void Checklist_Watermelon(int numIterations)
    {
        if(resultList.Count < 10)
        {
            for (int i = 0; i < numIterations; i++)
            {
                float randomValue = Random.Range(0f, 1f);

                Queue<GameObject> resultQueue = new Queue<GameObject>();

                if (randomValue <= probability1)
                {
                    resultQueue.Enqueue(gameObjects[0]);
                }
                if (randomValue <= probability1 + probability2)
                {
                    resultQueue.Enqueue(gameObjects[1]);
                }
                if (randomValue <= probability1 + probability2 + probability3)
                {
                    resultQueue.Enqueue(gameObjects[2]);
                }

                //��� ť�� List �߰�
                resultList.Add(resultQueue);
            }
        }
    }




}

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
*/