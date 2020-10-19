using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Level : MonoBehaviour
{
    [SerializeField]
    public static float MoveSpeed = 20f;
    private static Level instance;

    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.7f;
    private const float PIPE_HEAD_HEIGHT = 2.76f;
    private const float PIPE_HEADH_WIDTH = 7.96f;
    private const float PIPE_DESTROY_POSITION = -90f;
    private const float PIPE_SPAWN_POSITION = 90f;
    private const float BIRD_X_POSITION = 0f;

    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax = 1.5f;
    private const float PIPE_MIN_LENGTH = 10f;
    private const float MIN_GAPSIZE = 20f;
    private const float MAX_GAPSIZE = 30f;


    private Difficulty difficulty = Difficulty.Easy;
    private float min_gapSize;
    private float max_gapSize;

    private int pipeSpawned;
    private int pipePassedCount;
    // private float lastSpawnedBottomHeight; 
    private List<Pipe> pipeList;
    private Bird bird;


    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }

    private State state;

    private enum Difficulty
    {
        Easy, Medium, Difficult, Boss
    }
    private const float difficulty_factor = 0.9f;



    public int GetPipeSpawned()
    {
        // Debug.Log(pipeSpawned); 
        return pipeSpawned;
    }

    public int GetPipePassedCount()
    {
        return pipePassedCount;
    }

    public static Level GetInstance()
    {
        return instance;
    }


    private void Awake()
    {
        instance = this;
        pipeList = new List<Pipe>();
        min_gapSize = MIN_GAPSIZE;
        max_gapSize = MAX_GAPSIZE;
        pipeSpawned = 0;
        pipePassedCount = 0;
        Transform bt = Instantiate(GameAssets.GetInstance().pfBird); // create a bird using the prefab


        // method 1 
        // don't instantiate anything inheriting from the MonoBehaviour class using the new keyword.
        // otherwise, unity will give warning 
        // this.bird = new Bird(bird);


        // However, you cannot remove the inheritence from Monobehaviour because you need the awake and start  function
        // bird.gameObject.AddComponent<Bird>();

        // this doesn't work as well because you cannot add the Bird script to a transform which is a component

        /**
         *  you can create a game object and put the transform in this game object
         *  then you add the script to this game object. 
         *  
         */


        // so method 2, fail again because birdGo is an empty game object without rigidbody
        // the object of the transform has a rigid body 
        //GameObject birdGO = new GameObject("bird");
        //bt.SetParent(birdGO.transform);
        //bird = Bird.CreateBird(birdGO, bt);

        bird = Bird.CreateBird(bt.gameObject, bt);


        Bird.OnDied += BirdDiedHandler; // subscribe to the event, the handler can be private
        Bird.OnStart += BirdStartHandler;
        state = State.WaitingToStart;
    }

    // private void birdDiedHandler(object sender, System.EventArgs e)
    private void BirdDiedHandler()
    {
        state = State.BirdDead;
        Debug.Log("level stop");
        // call the function after 1 second 
        //FunctionTimer.Create(() =>
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        //}, 1f);
    }

    private void BirdStartHandler()
    {
        state = State.Playing;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            SpawnPipeHandler();
        }

    }

    private void SetDifficulty(Difficulty d)
    {
        difficulty = d;
        max_gapSize = MAX_GAPSIZE * Mathf.Pow(difficulty_factor, (int)d);
        min_gapSize = MIN_GAPSIZE * Mathf.Pow(difficulty_factor, (int)d);
        Debug.Log("difficulty changed" + d);
    }

    private Difficulty GetDifficulty()
    {
        if (pipeSpawned >= 40) return Difficulty.Boss;
        if (pipeSpawned >= 30) return Difficulty.Difficult;
        if (pipeSpawned >= 20) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private float RandomBottomPipeLength(float gapSize)
    {
        float buffer = Random.Range(0f, CAMERA_ORTHO_SIZE * 2f - gapSize - PIPE_MIN_LENGTH * 2f);
        return PIPE_MIN_LENGTH + buffer;
    }

    private float RandomGapSize()
    {
        return Random.Range(min_gapSize, max_gapSize);
    }

    private void SpawnPipeHandler()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer <= 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;
            float gapSize = RandomGapSize();
            CreateGapPipes(RandomBottomPipeLength(gapSize), gapSize, PIPE_SPAWN_POSITION);
            Difficulty d = GetDifficulty();
            if (difficulty != d)
                SetDifficulty(d);
            pipeSpawned++;
        }
    }

    private void HandlePipeMovement()
    {

        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isOnTheRight = pipe.GetX() > BIRD_X_POSITION;
            pipe.Move();
            if (isOnTheRight && pipe.GetX() <= BIRD_X_POSITION && pipe.GetIsBottom())
            {
                //pipe passsed the bird
                pipePassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }


            if (pipe.GetX() <= PIPE_DESTROY_POSITION)
            {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }

        }
    }

    /**
     *  assert gapY is at least 10 
     */
    private void CreateGapPipes(float bottomPipeLength, float gapSize, float xPosition)
    {
        CreatePipe(bottomPipeLength, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - bottomPipeLength - gapSize, xPosition, false);
    }

    private void CreatePipe(float length, float xPosition, bool createBottom)
    {
        //create pipe head 
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom)
        {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + length - PIPE_HEAD_HEIGHT / 2f;
        }
        else
        {
            pipeHeadYPosition = +CAMERA_ORTHO_SIZE - length + PIPE_HEAD_HEIGHT / 2f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);




        // create pipe body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        //change the height of the pipe
        SpriteRenderer pipeBodySpriteRender = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRender.size = new Vector2(PIPE_WIDTH, length);


        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, length);
        pipeBodyBoxCollider.offset = new Vector2(0f, length / 2f);

        BoxCollider2D pipeHeadBoxCollider = pipeHead.GetComponent<BoxCollider2D>();
        pipeHeadBoxCollider.size = new Vector2(PIPE_HEADH_WIDTH, PIPE_HEAD_HEIGHT);
        pipeHeadBoxCollider.offset = new Vector2(0f, 0f);

        // add the new pipe to the list 
        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    private class Pipe
    {
        private Transform pipeHead;
        private Transform pipeBody;
        private bool isBottom;

        public Pipe(Transform ph, Transform pb, bool isBottom)
        {
            pipeHead = ph;
            pipeBody = pb;
            this.isBottom = isBottom;
        }

        public bool GetIsBottom()
        {
            return isBottom;
        }
        public void Move()
        {
            pipeHead.position += new Vector3(-1, 0, 0) * MoveSpeed * Time.deltaTime;
            pipeBody.position += new Vector3(-1, 0, 0) * MoveSpeed * Time.deltaTime;
        }

        public float GetX()
        {
            return pipeBody.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeHead.gameObject);
            Destroy(pipeBody.gameObject);
        }
    }
}
