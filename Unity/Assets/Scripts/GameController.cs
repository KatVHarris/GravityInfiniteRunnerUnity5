using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class GameController : MonoBehaviour {
    
    //UI
    public static int score; 
	Text scoreText;
    Text finalScoreText;
    Text kinectConnectedText;
    Text oculusConnectedText;
    LookInputModule lookInput;

    //Player
    PlayerHealthController playerHealth;
    GameObject player;
    InfinitePlayerController infinitePlayerController; 

    //Kinect
    public GameObject handController;
    public GameObject bodyManager; 
    HandGestureTrigger handManagerComponent;
    BodySourceManager bodyManagerComponent;

    //Oculus
    OVRManager oculusOVRManager; 

    //Platrforms
    GameObject platformController;
    GameObject startPlaneObject; 
    TestSpawner testSpawner;
    GameObject problemSolverObject; 

    GameObject startPanel; 
    GameObject easyButtonObject;
    GameObject hardButtonObject;

    //Start Sequence
    enum Difficulty { easy, medium, hard };

    bool easydif = false;
    bool harddif = false;

    public int difficulty;
    bool gameended = false;
    bool gamestarted = false;
    public bool HasGameStarted()
    {
        return gamestarted; 
    }
    bool loading = false;
    public bool oculusActive = false;
    public bool kinectActive = false;
    int oculusactivity = 0; 

    float restartTimer;
	float restartDelay = 1f;
    float beginningTimer;
    float beginningDelay = .5f;

    Animator startAnim;
    Animator endAnim;



	// Use this for initialization
	void Start () {
        startPanel = GameObject.Find("StartPanel");

    }


    void Awake ()
	{
		// Setup the UI
        GameObject stui = GameObject.Find("ScoreText");
        scoreText = stui.GetComponent<Text>();
        GameObject edui = GameObject.Find("FinalScoreText");
        finalScoreText = edui.GetComponent<Text>();
        GameObject kText = GameObject.Find("KinectText");
        kinectConnectedText = kText.GetComponent<Text>();
        GameObject oText = GameObject.Find("OculusText");
        oculusConnectedText = oText.GetComponent<Text>();
        GameObject ovrcanvas = GameObject.Find("UICanvas");
        startAnim = ovrcanvas.GetComponent<Animator>();
        score = 0;
        GameObject es = GameObject.Find("EventSystem");
        lookInput = es.GetComponent<LookInputModule>();


        //Disable Game over Button
        GameObject gameOverButton = GameObject.Find("RestartButton");
        Button b = gameOverButton.GetComponent<Button>();
        gameOverButton.SetActive(false);

        //Setup Player
        platformController = GameObject.Find("PlatformController");
        testSpawner = platformController.GetComponent<TestSpawner>();
        player = GameObject.Find("FPSPreFab");
        infinitePlayerController = player.GetComponent<InfinitePlayerController>();
        playerHealth = player.GetComponent<PlayerHealthController>();

        //Setup Kinect
        bodyManagerComponent = bodyManager.GetComponent<BodySourceManager>();
        handManagerComponent = handController.GetComponent<HandGestureTrigger>();

        //Setup Oculus
        GameObject ovrCameraRig = GameObject.Find("OVRCameraRig");
        oculusOVRManager = ovrCameraRig.GetComponent<OVRManager>();

        problemSolverObject = GameObject.Find("ProblemSolver");

    }
	
	
	void Update ()
	{
        if (!gamestarted)
        {
            GameObject currentSelected = lookInput.CurrentSelectedObject();

            if (Input.GetKeyUp(KeyCode.Return))
            {

                //check if look is over play
                //GameObject currentSelected = lookInput.CurrentSelectedObject();
                if (currentSelected != null && currentSelected.transform.parent.name == "StartButton")
                {
                    StartGame();
                }

            }

            //check if look is over play
            if (currentSelected != null && currentSelected.transform.parent.name == "StartButton" && handManagerComponent.signalStart)
            {
                StartGame();
            }

            if (kinectActive)
                kinectConnectedText.color = new Color(0, 1, 0, 1);
            else
                checkKinectConnection();

            if (oculusActive)
                oculusConnectedText.color = new Color(0, 1, 0, 1); 
            else
                checkOculusConnection();
        }

        if (loading)
        {
            Debug.Log("Animation should start");
            // ... tell the animator the game is over.
            startAnim.SetTrigger("StartOVRGame");

            // .. increment a timer to count up to restarting.
            beginningTimer += Time.deltaTime;

            // .. if it reaches the restart delay...
            if (beginningTimer >= beginningDelay)
            {
                Debug.Log("TriggeringStart");
                loading = false;
                StartLevel();
            }
        }


		// Set the displayed text to be the word "Score" followed by the score value.
		scoreText.text = "Score: " + score;
        finalScoreText.text = "Score:" + score;


        if (playerHealth.currentHealth <= 0 && !gameended)
            InitializeEndGame();

        if (Input.GetKeyUp(KeyCode.Backspace) && !gameended)
            RestartGame();

        if (gameended)
        {
            restartTimer += Time.deltaTime;

            // .. if it reaches the restart delay...
            if (restartTimer >= restartDelay)
            {
                // .. then reload the currently loaded level.
                Application.LoadLevel(Application.loadedLevel);
            }
        }
	}

    void checkKinectConnection()
    {
        //Look at BodyConnection 
        //kinectActive = bodyManagerComponent.();
        //Debug.Log("kinectActive " + kinectActive);
    }

    void checkOculusConnection()
    {
        if (oculusOVRManager.headsetConnectionComplete)
            oculusActive = true;
    }


    void InitializeEndGame()
    {
        //startAnim.SetTrigger("EndGame");
        player.GetComponent<FirstPersonCharacter>().enabled = false;
        player.GetComponent<Rigidbody>().useGravity = false;

//        player.GetComponent<Shoot>().enabled = false; 

        //Destroy all platforms
        testSpawner.StopMovement();
        problemSolverObject.SetActive(false);

        EndGameInstantly();
        
    }

    public void RestartGame(){
        gameended = true;

    }

    void EndGameInstantly()
    {
        restartTimer += Time.deltaTime;

        // .. if it reaches the restart delay...
        if (restartTimer >= restartDelay)
        {
            // .. then reload the currently loaded level.
            Application.LoadLevel(Application.loadedLevel);
        }

        player.GetComponent<FirstPersonCharacter>().enabled = false;
        //player.rigidbody.useGravity = false;

        //Destroy all platforms
        testSpawner.StopMovement();
    }

    public void setDifficultyEasy()
    {
        if (!harddif && !easydif)
        {
            easydif = true; 
            //disable button hard
            difficulty = (int)Difficulty.easy;
            //hardButtonObject.SetActive(false);
       
        }
    }

    public void setDifficultyHard()
    {
        if(!harddif && !easydif)
        {
            harddif = true; 
            difficulty = (int)Difficulty.hard;
            //disable button easy
            //easyButtonObject.SetActive(false);
      
        }

    }

    public void StartGame()
    {
        if(kinectActive && oculusActive)
            loading = true; 
    }

    void StartLevel()
    {
        //loading false start true - EnablePlayerController before level is loaded
        Debug.Log("Spawning and Enabling Player");
        
        infinitePlayerController.enabled = true;
        testSpawner.StartGeneration();
        //Setup is done

    }
}
