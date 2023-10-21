using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 수박게임 Spawn 위치 및 생성
public class WaterMelon_Spawnpoint : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] gameObjects;

    // UI세팅하기
    
    public GameObject spawnObject; 
    public GameObject waitingObject;

    public Transform waiting_Point;

    // gamemanager에서 가져올 오브젝트
    public Queue<GameObject>[] firstTwoObjects = new Queue<GameObject>[2];

    // 결과값을 저장할 List 생성
    List<Queue<GameObject>> resultList = new List<Queue<GameObject>>();
    
    private float probability1 = 0.45f;
    private float probability2 = 0.35f;
    private float probability3 = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        // 초반에 30개 세팅
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

            //결과 큐를 List 추가
            resultList.Add(resultQueue); 
        }

        firstTwoObjects[0] = resultList[0];
        firstTwoObjects[1] = resultList[1];
        
        // 초기화세팅에서 앞에서 두개를 지워줌
        resultList.RemoveRange(0, 2);


    }

    // firstTwoObjects[3,4] / UI[1.2] 세팅완료
    void Setting_Process()
    {
        // 처음 UI 세팅 가져오기
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
       
   

   // 버튼 눌릴시 프로세스
   void Input_Proccess()
    {
        // 버튼이 떨어졌을때 
        spawnObject = waitingObject;
        Instantiate(spawnObject, this.transform.position, Quaternion.identity);

        waitingObject = firstTwoObjects[0].Dequeue();
        Instantiate(waitingObject, waiting_Point.transform.position, Quaternion.identity);

        // 두 번째 큐의 첫번쨰 요소를 첫 번쨰 큐로 옮깁니다
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

                //결과 큐를 List 추가
                resultList.Add(resultQueue);
            }
        }
    }




}

/*
    // 정령을 스폰할 메서드
    void Spawning_WatermelonSoul(int numIteration)
    {
        //결과값을 저장할 배열생성
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


            //결과값을 문자열로 출력
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

        //최초 시작시에 하나의 오브젝트를 배치해야함
        //i 라면 정중앙과 i+1이라면 좌측 UI에 배치해야함
    }
*/