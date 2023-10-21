using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ڰ��� Spawn ��ġ �� ����
public class WaterMelon_Spawnpoint : MonoBehaviour
{
    public GameObject[] gameObjects;

    private float probability1 = 0.45f;
    private float probability2 = 0.35f;
    private float probability3 = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Spawn_WatermelonSoul(30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ������ ������ �޼���
    void Spawn_WatermelonSoul(int numIteration)
    {
        //������� ������ �迭����
        GameObject[] results = new GameObject[numIteration];

        for(int i = 0; i < numIteration; i++)
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
            for(int j = 0; j < results.Length; j++)
            {
                resultsString += results[i].name;

                if( j < results.Length -1)
                {
                    resultsString += ", ";

                }
            }

            Debug.Log("Results: " + resultsString);
        }

    }
}
