using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ڰ��� Spawn ��ġ �� ����
public class WaterMelon_Spawnpoint : MonoBehaviour
{

    public GameObject[] gameObjects;


    public GameObject spawnObject;

    public GameObject waitObject;
    [HideInInspector]
    public Transform waiting_Point;


    [SerializeField]
    private GameObject nowPrefab;

    [SerializeField]
    private GameObject waitedPrefab;

    private GameObject firstwaitPrefab;

    // Input access successful
    private bool isActivated;
    private bool falling = false;
    [SerializeField]
    private bool isWatedactivate;

    [SerializeField]
    GameObject currentObj;

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
        Setting_Prefab();
    }

    // Update is called once per frame
    void Update()
    {
        Checklist_Watermelon(20);

        if (Input.GetMouseButtonDown(0))
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
        // isActivated = true;

        //nowPrefab = resultList[1].Dequeue();

        // �ʱ�ȭ���ÿ��� �տ��� �ΰ��� ������
        resultList.RemoveRange(0, 2);


    }

    // firstTwoObjects[3,4] / UI[1.2] ���ÿϷ�
    void Setting_Process()
    {
        // ó�� UI ���� ��������
        spawnObject = firstTwoObjects[0].Dequeue();
        waitObject = firstTwoObjects[1].Dequeue();

        firstTwoObjects[0] = resultList[0];
        firstTwoObjects[1] = resultList[1];


        resultList.RemoveRange(0, 2);

    }

    void Setting_Prefab()
    {
        GameObject obj1 = Instantiate(spawnObject);
        Rigidbody2D rb1 = obj1.GetComponent<Rigidbody2D>();
        obj1.transform.position = this.transform.position;
        rb1.gravityScale = 0;

        checking_nowobject(obj1);

        GameObject obj2 = Instantiate(waitObject);
        Rigidbody2D rb2 = obj2.GetComponent<Rigidbody2D>();
        obj2.transform.position = waiting_Point.position;
        //firstwaitPrefab = obj2;
        rb2.gravityScale = 0;

        checking_waitobject(obj2);

        isWatedactivate = true;
    }



    // ��ư ������ ���μ���
    void Input_Proccess()
    {


        if (firstTwoObjects[0] == null || firstTwoObjects[1] == null)
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


        if(isActivated == true)
        {
            GameObject othertownowPrefab = Instantiate(currentObj);
            othertownowPrefab.transform.position = this.transform.position;
            Rigidbody2D rigids= othertownowPrefab.GetComponent<Rigidbody2D>();
            rigids.gravityScale = 1f;
        }
        // ��ư�� ���������� 
        if(isActivated == false)
        {
            nowPrefab = waitedPrefab;
            nowPrefab.transform.position = this.transform.position;
            Rigidbody2D rigidnow = nowPrefab.GetComponent<Rigidbody2D>();
            rigidnow.gravityScale = 1f;
        }
   
        waitedPrefab = firstTwoObjects[0].Dequeue();

        // 떨어지는 중에는 생성x 
      
        // 2번째 큐 1번쨰큐로 밀음
        if (firstTwoObjects[1].Count > 0)
        {
            firstTwoObjects[0].Enqueue(firstTwoObjects[1].Dequeue());
            firstTwoObjects[1].Enqueue(resultList[0].Dequeue());
            resultList.RemoveRange(0, 1);
        }
        else if(firstTwoObjects[0].Count <= 0)
        {
            firstTwoObjects[0].Enqueue(resultList[0].Dequeue());
            firstTwoObjects[1].Enqueue(resultList[1].Dequeue());
            resultList.RemoveRange(0, 2);
        }

        // 숫자 3의 요소 생성
        GameObject waitingObj = Instantiate(waitedPrefab);
        waitingObj.transform.position = waiting_Point.position;
        Rigidbody2D rigid = waitObject.GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        
        // 프리팹으로 생성된 오브젝트 체크
        checking_waitobject(waitingObj);

        // 대기중인 prefab 삭제 메서드
        if(currentObj == null)
        {
            currentObj = waitingObj;
            bool ready = true;
            isActivated = true;
        }
        else
        {
            // 여기에서 대기중인놈을 삭제하다가 아예 삭제가 되어버림
            nowPrefab = waitingObj;
            Destroy(currentObj);

            currentObj = waitingObj;

        }
    
    
    } 



    
    IEnumerator making_Watermelon(GameObject _something)
    {
        falling = true;
        yield return new WaitForSeconds(2f);
        Instantiate(_something);
        _something.transform.position = this.transform.position;
        
        falling = false;
    }

    public GameObject checking_waitobject(GameObject _lastObject)
    {
        waitedPrefab = _lastObject;
        return waitedPrefab;
    }


    public GameObject checking_nowobject(GameObject _nowObject)
    {
        nowPrefab = _nowObject;
        return nowPrefab;
    }


    // Update���� CheckList 20�� �̻��� üũ
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




        Void AnotherFunction()
{

        GameObject instantiatedObject = InstantiatePrefabAndGetReference();
        if(instatiatedObject != null)
        {
            
        }


*/