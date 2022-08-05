using System.Collections; // sirve para acceder a listas o rutinas
using System.Collections.Generic; // sirve para acceder a listas o rutinas
using UnityEngine; //libreria al completo de unity (todos los objetos)
using UnityEngine.InputSystem;

public class Ship: MonoBehaviour //Ship es el nombre de la clase, tiene que coincidir con el del fichero // las clases que geremos tienen que derivar de Monobehabiour
{
    public float maxSpeed; //al ponerlo publico lo podemos cambiar desde el inspector de Unity
    public float maxTurnSpeed; // velocidad de giro
    //public float radius;
    public float drag; // la resistencia
    public float acceleration;
    public float turnAcceleration;
    public List<Bullet> bullets;

    public GameObject bulletPrefab;


    private MeshRenderer _renderer; //_ porque es una variable privada
    private float currentSpeed; //la rapidez con la que cambiamos en el tiempo
    private float currentTurnSpeed;
    private const float WORLDWIDTH=26.2f;
    private const float WORLDEIGHT=15.2f;
    public ParticleSystem forwardParticles;
    private ParticleSystem.EmissionModule forwardEmission;



    // Start se llama justo antes del primer frame update (frame= cada imagen de un juego)
    void Start() //void= que esa funcion no devuelve nada, esa funcion no devuelve nada, se crea solo por el monobehavior, las da para permitirnos engancharnos al ciclo de vida de un componente
    {
        //MeshRenderer renderer; //decimos que renderer es una variable del TIPO MeshRenderer
        // renderer= GetComponent<MeshRenderer>(); //asignamos a renderer el valor
        _renderer =GetComponent<MeshRenderer>(); //todo junto
        bullets= new List<Bullet>();
        forwardEmission=forwardParticles.emission;

    }
 
    
    // Update is called once per frame
    void Update()
    {
        float turnForce=0;
        float moveForce=0;
        bool triggerFire=false;

        if(Keyboard.current.wKey.isPressed) moveForce=1; //isPressed es mantener pulsado (se dispara una vez por cada frame que ha estado pulsado el boton
        if(Keyboard.current.sKey.isPressed) moveForce=-1;
        if(Keyboard.current.dKey.isPressed) turnForce=-1;
        if(Keyboard.current.aKey.isPressed) turnForce=1;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            forwardEmission.rateOverTime=10;
        }
        else forwardEmission.rateOverTime=0;
        if(Keyboard.current.spaceKey.wasPressedThisFrame) triggerFire=true; //waspressed this frame solo se dispara una vez

        bool isThrusting=moveForce!=0; //para guardar si se esta moviendo
        bool isTurning= turnForce !=0;

        //CONTROL DE LA VELOCIDAD
        //v=v0+a*t^2

        currentSpeed=currentSpeed+moveForce *acceleration*Time.deltaTime;
        //Otras 2 formas en el word
        currentSpeed=Mathf.Clamp(value:currentSpeed,min:-maxSpeed,maxSpeed);
        if (!isThrusting) //if (isThrusting ==false) ==if (!isThrusting)
            {
                currentSpeed=currentSpeed*drag;
            }

        //CONTROL DE GIRO
        currentTurnSpeed =currentSpeed+turnForce*acceleration*Time.deltaTime;
        currentTurnSpeed =Mathf.Clamp(value:currentTurnSpeed,min:-maxTurnSpeed,maxTurnSpeed);
        if (!isTurning) //if (isThrusting ==false) ==if (!isThrusting)
            {
                currentTurnSpeed=currentTurnSpeed*drag;
            }

        transform.Rotate(turnForce*Vector3.forward*maxTurnSpeed*Time.deltaTime);
        transform.Translate(Vector3.up*currentSpeed*Time.deltaTime);
        //transform.position += deltaY*transform.up*speed*Time.deltaTime; lo mismo que la de arriba
        WrapAroundWorldEdges();

        if (triggerFire)
        {
            //Debug.Log(message:"Pewpewpew");
            GameObject go =Instantiate(bulletPrefab,transform.position,Quaternion.identity);
            Bullet bullet= go.GetComponent<Bullet>();
            bullet.SetOrientation(transform.up);
            bullets.Add(bullet);
        }

        //Keep bullets list clean
        bullets.RemoveAll(b => b==null);
        

    }


    private void WrapAroundWorldEdges()
    {
        Vector3 shipPosition=transform.position;
        if(shipPosition.x< -WORLDWIDTH)
            shipPosition.x=WORLDWIDTH;
        if(shipPosition.x>WORLDWIDTH)
            shipPosition.x= -WORLDWIDTH;

        if(shipPosition.y< -WORLDEIGHT) 
            shipPosition.y=WORLDEIGHT;
        if(shipPosition.y>WORLDEIGHT)
            shipPosition.y= -WORLDEIGHT;

        transform.position=shipPosition;
    }


    public void StopVelocity()
    {
        currentSpeed=0;
    }






        //Vector3 velocity=new Vector3(deltaX,deltaY,z:0);
        //transform.Translate(translation: velocity.normalized *Time.deltaTime*speed);
        


        /*
        bool moveforward = Keyboard.current.upArrowKey.isPressed; //para cambiar la tecla que pulsamos ponemos aKey
        if(moveforward) //if(moveforward==true)
        {   
            transform.Translate(Vector3.up*Time.deltaTime);
        }

        bool movebackward = Keyboard.current.downArrowKey.isPressed; 
        if(movebackward) //if(moveforward==true)
        {   
            transform.Translate(Vector3.down*Time.deltaTime);
        }        
        bool moveright = Keyboard.current.rightArrowKey.isPressed; 
        if(moveright) //if(moveforward==true)
        {   
            transform.Translate(Vector3.right*Time.deltaTime);
        }    

        bool moveleft = Keyboard.current.leftArrowKey.isPressed; 
        if(moveleft) //if(moveforward==true)
        {   
            transform.Translate(Vector3.left*Time.deltaTime);
        }  
        */ 
        /* VARIAR LA POSICION
        Vector3 newPosition= transform.position;
        newPosition.x=newPosition.x +speed* Time.deltaTime; //* Time.deltaTime se pone siempre que acumulemos un valor en cada frame, porque si no se ve como que da saltitos. El resultado es que se mueve a velocidad speed por segundo
        //newPosition.x=newPosition.x +0.1f; //la posicion de la x ahora va a sumar infinitamente 0.1
        //newPosition= new Vector3(x:Mathf.Sin(Time.time*speed), y:Mathf.Cos(Time.time*speed), z:0)*radius;
        transform.position=newPosition;
        */

        /*CAMBIAR ESCALA
        Vector3 newScale =transform.localScale;
        newScale=Vector3.one + Vector3.one*Mathf.Sin(Time.time);
        transform.localScale=newScale;
        */

        //rotacion
        //transform.Rotate(Vector3.up, angle:Time.deltaTime*45);
        //transform.Rotate(axis: new Vector3(x:0.25f,y:0.25f,z:0.25f), angle:Time.deltaTime*speed);//en vez de *speed podemos poner los grados que queramos
        //transform.Translate(translation: Vector3.right*Time.deltaTime,relativeTo: Space.World); //rOTACION CON TRASLACION
        
        //CAMBIAR COLOR:

        //renderer.material.color= new Color(r:0.9f,g:0.1f,b:0.9f);
        //renderer.material.color= new Color(r:0.9f,g:0.1f,b:0.9f)*(float)Mathf.Sin(Time.time*10); //cambio de color de uno a otro
        
        //_renderer.material.color= new Color(r:1,g:1,b:1)*Random.value;



    
}
