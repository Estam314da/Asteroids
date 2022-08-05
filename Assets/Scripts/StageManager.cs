using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageManager : MonoBehaviour

{
    public GameObject asteroidPrefab;
    public Ship player;

    private Ship ship;
    List<Asteroid>asteroids;
    List<Bullet>bullets;

    // Start is called before the first frame update
    void Start()
    {
        ship= FindObjectOfType<Ship>();
        asteroids= new List<Asteroid>();
        bullets= new List<Bullet>();
        ResetGame();
       
    }

    void ResetGame()
    {
        foreach (Asteroid a in asteroids)
        {
            Destroy(a.gameObject);
        }
        foreach(Bullet j in player.bullets)
        {
            if (j!= null)
            {
                Destroy(j.gameObject);
            }
        }
        asteroids.Clear();
        bullets.Clear();
        player.transform.position=Vector3.zero;
        player.transform.rotation=Quaternion.identity;
        ship.StopVelocity();

        for (int i = 0; i < 5; i++) //i++ == i+1
        {
            Vector3 spawnPoint= (Vector3)(Random.insideUnitCircle.normalized*50);            //Vector3 spawnPoint=new Vector3(x:Random.Range(10,25),y:Random.Range(-15,15), z:0);
            CreateAsteroid(spawnPoint, 3);

        }
    }



    // Update is called once per frame
    void Update()
    {
        //Colisiones jugador-asteroires
        bool playerIsDead= false;
        foreach (Asteroid a in asteroids)
        {
            if (a== null) continue;
            Vector3 towardsPlayer=player.transform.position-a.transform.position;
            float distance=towardsPlayer.magnitude;
            if (distance < a.GetSize())
            {
                Debug.Log(message:"CRAAAAASH");
                playerIsDead= true;
            }
        }
        if (playerIsDead)
        {
            ResetGame();
        }
        //Colisiones balas-asteroides
        List<Vector3> asteroidSpawnPositions=new List<Vector3>();
        foreach (Asteroid a in asteroids)
        {
            if (a==null) continue;
            foreach(Bullet b in player.bullets)
            {
                if (b== null || a==null) continue; // ||=ó ; &&=y

                Vector3 towardsBullet=b.transform.position -a.transform.position;
                float distance =towardsBullet.magnitude;
                if (distance<a.GetSize())
                {
                    float size=a.GetSize();
                    Destroy(a.gameObject);
                    Destroy(b.gameObject);
                    Vector3 spawnPoint=a.transform.position;
                    if (size>1f)
                    {
                        spawnPoint.z=size/2; // cosa fea, como el eje z no lo estamos utilizando, vamos a guardar ahi el tamaño del asteroide para luego poder acceder a el mas facilmente
                        asteroidSpawnPositions.Add(spawnPoint);
                    }
                }
            }
        }
        for (int i=0; i<asteroidSpawnPositions.Count; i++)
        {
            Vector3 spawnPosition=asteroidSpawnPositions[i];
            float size= asteroidSpawnPositions[i].z;
            spawnPosition.z=0;
            CreateAsteroid(spawnPosition,size);
            CreateAsteroid(spawnPosition,size);

        }
        asteroids.RemoveAll(f=>f== null); //significa: para los a tal que a es nulo, entonces borralo (remove all)
        
        if(asteroids.Count==0) ResetGame();


    }

    void CreateAsteroid(Vector3 p, float size) //crea un asteroide en el punto p, de tamaño size
    {
        Vector3 spawnPoint= p;            //Vector3 spawnPoint=new Vector3(x:Random.Range(10,25),y:Random.Range(-15,15), z:0);
        GameObject go=Instantiate(asteroidPrefab,spawnPoint,Quaternion.identity, transform); //creamos un asteroide
        Asteroid asteroid = go.GetComponent<Asteroid>(); //cogemos el componente de un asteroide
        asteroid.SetSize(size); //llamamos a una funcion
        asteroids.Add(asteroid); //lo añadimos a la lista
    }

}
