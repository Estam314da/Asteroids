using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 orientation;

    public float speed=300;
    public float elapsedTime;
    public float timeToLive; //se suele llamar ttl
    public void SetOrientation(Vector3 orientation)
    {
        this.orientation=orientation;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation:orientation*speed*Time.deltaTime);
        elapsedTime+= Time.deltaTime;
        if (elapsedTime>timeToLive)
        {
            Destroy(gameObject);
        }

    }
}
