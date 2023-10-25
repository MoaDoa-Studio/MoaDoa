using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public int level;
    public bool isDrag;
    public bool isMerge;

    Rigidbody2D rigid;
    CircleCollider2D circle;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        string objectName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Dongle")
        {
            Dongle other = collision.gameObject.GetComponent<Dongle>();

            if(level == other.level && !isMerge && level < 7)
            {
                // Dongle merge logic
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                // 1. if I'm under
                // 2. if I'm on same level, or right
                if(meY < otherY || meY == otherY && meX > otherX)
                {
                    // hide something
                    other.Hide(transform.position);
                    LevelUp();
                }
                
            }
        }
    }

    public void Hide(Vector3 targetPos)
    {
        isMerge = true;

        rigid.simulated = false;
        circle.enabled = false;

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos)
    {
        int frameCount = 0;

        while(frameCount < 20) 
        {
            frameCount++;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
        }

        isMerge = false;
        gameObject.SetActive(false); 


        yield return null;
    }

    void LevelUp()
    {
        isMerge = true;

        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;
        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
       // yield return new WaitForSeconds(0.2f);

        // 애니메이션 실행
        level++;
        GameObject newObject = (GameObject)Instantiate(Resources.Load("Prefabs/WaterMelon/Soul_object" + level), transform.position, Quaternion.identity);
        newObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        yield return new WaitForSeconds(0.1f);

        isMerge = false;
        Destroy(this.gameObject);
    }
}
