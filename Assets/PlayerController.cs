using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<Transform> bodyparts = new List<Transform>();
    public float mindistance = 0.25f;
    public float speed = 1;
    public float rotationspeed = 50;
    public int beginsize;
    public GameObject prefab;

   
    private float dis;
    private Transform curbodypart, pervbodypart;


    
     
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i=0;i<beginsize;i++)
        {
            addbodypart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        move();
        if (Input.GetKey(KeyCode.Q))
        {
            addbodypart();
        }
    }
    void move() 
    {
        float curspeed = speed;

        if (Input.GetKey(KeyCode.W))
            curspeed *= 2;

        bodyparts[0].Translate(bodyparts[0].forward * curspeed * Time.smoothDeltaTime, Space.World);

        if (Input.GetAxis("Horizontal") != 0)
        {
            bodyparts[0].Rotate(Vector3.up * rotationspeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }
        for(int i=1; i< bodyparts.Count; i++)
        {
            curbodypart = bodyparts[i];
            pervbodypart = bodyparts[i - 1];

            dis = Vector3.Distance(pervbodypart.position, curbodypart.position);
            Vector3 newpos = pervbodypart.position;
            newpos.y = bodyparts[0].position.y;

            float T = Time.deltaTime * dis / mindistance * curspeed;

            if (T > 0.5)
                T = 0.5f;

            curbodypart.position = Vector3.Lerp(curbodypart.position,newpos,T);
            curbodypart.rotation = Quaternion.Lerp(curbodypart.rotation,pervbodypart.rotation, T);
        }
    }

    public void addbodypart()
    {
        Transform newpart = (Instantiate(prefab, bodyparts[bodyparts.Count - 1].position, bodyparts[bodyparts.Count - 1].rotation) as GameObject).transform;
        newpart.SetParent(transform);
        bodyparts.Add(newpart);
    }
}
