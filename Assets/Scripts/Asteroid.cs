using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    //public float size; //escla del asteroide
    public float speed;
    public Transform rockModel;
    public float rotationSpeed;

    private Vector2 orientation;
    private Vector2 rockRotationVelocity;
    private float asteroidSize;  //    public float asteroidSize {get; private set;} la hace publica pero dice que el set es privado


    private const float WORLDWIDTH=25.2f;
    private const float WORLDEIGHT=14.2f;


    public void SetSize(float size)
    {
        asteroidSize=size; 
        rockModel.localScale= Vector3.one*size;
    }

    public float GetSize()
    {
        return asteroidSize;
    }


    void Start()
    {
        speed=Random.Range(-speed,speed);
        orientation=Random.insideUnitCircle.normalized;
        rockRotationVelocity=Random.insideUnitCircle*Random.Range(-rotationSpeed,rotationSpeed);
    }

    void Update()
    {
        transform.Translate(translation:(Vector3)(orientation*speed*Time.deltaTime));
        rockModel.Rotate(eulers:(Vector3)(rockRotationVelocity*Time.deltaTime));
        WrapAroundWorldEdges();
    }

    private void WrapAroundWorldEdges()
    {
        Vector3 asteroidPosition=transform.position;
        if(asteroidPosition.x< -WORLDWIDTH-(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2) // - la hipotenisa del cubo del asteroide/2
            asteroidPosition.x=WORLDWIDTH+(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2;
        if(asteroidPosition.x>WORLDWIDTH+(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2)
            asteroidPosition.x= -WORLDWIDTH-(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2;

        if(asteroidPosition.y< -WORLDEIGHT-(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2) 
            asteroidPosition.y=WORLDEIGHT+(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2;
        if(asteroidPosition.y>WORLDEIGHT+(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2)
            asteroidPosition.y= -WORLDEIGHT-(Mathf.Sqrt(2f*Mathf.Pow(asteroidSize,2f)))/2;

        transform.position=asteroidPosition;    
    }
}