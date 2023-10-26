using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���ڰ��� Spawn ��ġ �� ����
public class WaterMelon_Spawnpoint : MonoBehaviour
{

    private Touch tempTouch;

    public GameObject[] gameObjects;

    [HideInInspector]
    public GameObject spawnObject;
    [HideInInspector]
    public GameObject waitObject;
    [HideInInspector]
    public Transform waiting_Point;

    int modeStack = 2;

    public GameObject usingObj;

    private GameObject nowPrefab;

    
    private GameObject waitedPrefab;

    GameObject somethingnow;

    // Input access successful
    private bool isActivated;
    
    [SerializeField]
    GameObject currentObj;

    // gamemanager���� ������ ������Ʈ
    public Queue<GameObject>[] firstTwoObjects = new Queue<GameObject>[2];

    // ������� ������ List ����
    List<Queue<GameObject>> resultList = new List<Queue<GameObject>>();

    private float probability1 = 0.6f;
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
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Input_Proccess();
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.y = this.transform.position.y;
            usingObj.transform.position = mousePos;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Input_Proccess();
            
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
        spawnObject = firstTwoObjects[0].Dequeue();
        waitedPrefab = firstTwoObjects[1].Dequeue();

        GameObject settingObj = Instantiate(spawnObject);
        spawnObject.transform.position = this.transform.position;
        Rigidbody2D rigid2D = spawnObject.GetComponent<Rigidbody2D>();
        rigid2D.gravityScale = 0f;
        nowPrefab = settingObj;
        checking_usingobject(settingObj);
        Input_Proccess();
    }
   
    

    // ��ư ������ ���μ���
    void Input_Proccess()
    {
        if(modeStack == 1)
            nowPrefab.GetComponent<Rigidbody2D>().gravityScale = 1f;

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

        
            // 그 다음껄 눌렀을때 떨어뜨려야함
            if(isActivated == true)
            {
                GameObject othertownowPrefab = Instantiate(currentObj);
                othertownowPrefab.transform.position = this.transform.position;
                othertownowPrefab.GetComponent<CircleCollider2D>().enabled = true;
                Rigidbody2D rigids= othertownowPrefab.GetComponent<Rigidbody2D>();
                //nowPrefab = othertownowPrefab;
                rigids.gravityScale = 1f;
                checking_usingobject(othertownowPrefab);
                //inputStack += 1;
            }
         
        
        
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

        if(modeStack == 1)
        {
            GameObject involve = waitedPrefab;
        }
        else
            waitedPrefab = firstTwoObjects[0].Dequeue();

        GameObject waitingObj = Instantiate(waitedPrefab);
        waitingObj.transform.position = waiting_Point.position;
        waitingObj.GetComponent<CircleCollider2D>().enabled = false;
        Rigidbody2D rigid = waitingObj.GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
        
       
        // 대기중인 prefab 삭제 메서드
        if(currentObj == null)
        {
            currentObj = waitingObj;
            modeStack -= 1;
        }
        else
        {
            // 여기에서 대기중인놈을 삭제하다가 아예 삭제가 되어버림
            isActivated = true;
            
            Destroy(currentObj);

            currentObj = waitingObj;
            modeStack -= 1;
        }
    
    
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

    
    IEnumerator making_Watermelon(GameObject _something)
    {
        //falling = true;
        yield return new WaitForSeconds(2f);
        Instantiate(_something);
        _something.transform.position = this.transform.position;
        
        //falling = false;
    }

    public GameObject checking_waitobject(GameObject _lastObject)
    {
        waitedPrefab = _lastObject;
        return waitedPrefab;
    }


    public GameObject checking_usingobject(GameObject _nowObject)
    {
       usingObj = _nowObject;
        return usingObj;
    }


    // Update���� CheckList 20�� �̻��� üũ




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