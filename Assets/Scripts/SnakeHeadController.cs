using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadController : MonoBehaviour
{

    public GameObject TailSegment;
    enum Direction {
        up, 
        down,
        left,
        right, 
        none
    }
    //direction taken in the last move action. We will use it to avoid the player can go on its own tail direction.
    Direction lastDirection;
    Direction currentDirection;
    public List<Transform> Tail = new List<Transform>();
    //position of the last dot of the snake's body
    Vector3 lastPos;
   Vector3 currentPosition;
    public float frameRate = 0.07f;
    public float step = 0.9f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
        lastDirection = Direction.none;
        currentDirection = Direction.none;
        currentPosition = Vector3.zero;

    }   

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Move", frameRate, frameRate);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentDirection!=Direction.down)
            currentDirection = Direction.up;
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentDirection!=Direction.up)
            currentDirection = Direction.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow)  && currentDirection!=Direction.right)
            currentDirection = Direction.left;
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentDirection!=Direction.left)
            currentDirection = Direction.right;

            
    }

    void Move(){
        //last position of the head. It will be the next pos of the 1st segment of the body.
        lastPos = transform.position;

        Debug.Log("se está ejecutando la acción mover");
        switch(currentDirection){
            case Direction.up: 
                currentPosition = Vector3.up;
                break;
              case Direction.down: 
                currentPosition = Vector3.down;
                break;
              case Direction.left: 
                currentPosition = Vector3.left;
                break;
              case Direction.right: 
                currentPosition = Vector3.right;
                break;                
        }

        currentPosition*=step;
        if(CheckValidMove())
        {
            transform.position += currentPosition;
            MoveTail();
            lastDirection = currentDirection;
        }
        else {
        
            // currentPosition*=step;
            Debug.Log("current direction = "+currentDirection+" y last direction = "+currentPosition);
            transform.position += currentPosition;
            MoveTail();

        }
    }
    //Cuando se un "paso", cada segmento de la cola ocupará la posición previamente por su segmento inmediatamente posterior
    void MoveTail(){
        for (int i = 0; i<Tail.Count; i++){
            Vector3 tempPos = Tail[i].position;
            Tail[i].position = lastPos;
            lastPos = tempPos;

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
        
     {
            if(other.CompareTag("Wall") || (other.CompareTag("Dot") && Tail.Count>=3))
            {
            Debug.Log("Has perdido");
            step =0;       
            }    
            //we will instantiate a new segment, add it to the transform's lists and relocate the food
            else if (other.gameObject.tag=="Food"){
                GameObject newTailSegment =  Instantiate(TailSegment, Tail[Tail.Count-1].position, Quaternion.identity);
                Tail.Add(newTailSegment.transform); 
                float newFoodPosX = Random.Range(-14.9f, 14.9f);
                float newFoodPosY = Random.Range(-9.2f, 9.2f);
                other.gameObject.transform.position = new Vector3 (newFoodPosX, newFoodPosY,0);               
            }
        
    }

    private bool CheckValidMove(){
        
        Debug.Log(currentDirection+" + "+ lastDirection);
        if((currentDirection!=Direction.none && currentDirection!=lastDirection))
        {
            Debug.Log("Movimiento válido");
            return true;
        }
        else return false;
    }
}
