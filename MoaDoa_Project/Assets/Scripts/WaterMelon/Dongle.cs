using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public int level;
    public bool isDrag;
    public bool isMerge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Dongle")
        {
            Dongle other = collision.gameObject.GetComponent<Dongle>();

            if(level == other.level && !isMerge && level < 7)
            {
                // Dongle merge logic
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherx = other.transform.position.x;
                float otherY = other.transform.position.y;

                // 1. if I'm under
                // 2. if I'm on same level, or right

                
            }
        }
    }


}
