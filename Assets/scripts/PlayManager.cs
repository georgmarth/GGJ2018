using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour {

    public enum GameState { Standby, Ready, Running, Gameover}
    public enum Symbol { I, O, K, L, None}

    public float signalIncreaseRate = 2f;
    public float signalDecreaseRate = 2f;
    
    bool messageUpdated = false;

    private Symbol activeSymbol;
    private Symbol activeMessage;
    public GameState gameState;
    private GameState remoteState;

    private List<Beam> beams;

    public float signalStrength = 100f;
    public MessageDisplay messageDisplay;

    public GameObject startScreen;
    public GameObject gameOverScreen;
    public InputField inputField;

#region Singleton
    public static PlayManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.LoadScene("Lighting", LoadSceneMode.Additive);
            return;
        }
        Destroy(gameObject);
    }
#endregion

    // Use this for initialization
    void Start () {

        gameState = GameState.Standby;
        remoteState = GameState.Standby;
        activeSymbol = Symbol.I;
        activeMessage = Symbol.None;

        beams = new List<Beam>();
        foreach (GameObject gO in GameObject.FindGameObjectsWithTag("Beam"))
        {
            Beam beam = gO.GetComponent<Beam>();
            if (beam != null)
                beams.Add(beam);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (remoteState == GameState.Gameover)
        {
            gameState = GameState.Gameover;
        }

        if (gameState == GameState.Gameover)
        {
            GameOver();
            return;
        }

        if (remoteState == GameState.Ready && gameState == GameState.Ready)
        {
            gameState = GameState.Running;
            StartGame();
        }

        if (gameState != GameState.Running)
        {
            return;
        }

        CalcSignalStrength();

        if (messageUpdated)
        {
            Debug.Log("Display Message");
            messageDisplay.DisplayMessage(activeMessage);
            messageUpdated = false;
        }

    }

    public void SetReady()
    {

        GetComponent<UDPTransfer>().remoteIP = inputField.text;
        gameState = GameState.Ready;
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
    }

    private void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    private void CalcSignalStrength()
    {
        float signalIncrease;
        if(BeamHitsDish())
        {
            signalIncrease = signalIncreaseRate * Time.deltaTime;
        }
        else
        {
            signalIncrease = -signalDecreaseRate * Time.deltaTime;
        }

        signalStrength = Mathf.Clamp(signalStrength + signalIncrease, 0f, 100f);
    }

    private bool BeamHitsDish()
    {
        foreach(Beam beam in beams)
        {
            if (beam.BeamSymbol == activeSymbol && beam.Intersecting)
                return true;
        }
        return false;
    }

    public void UpdateGameState(string newGameStateString)
    {
        newGameStateString = newGameStateString.Substring(0, 1);
        switch (newGameStateString)
        {
            case "o":
                remoteState = GameState.Gameover;
                break;
            case "r":
                remoteState = GameState.Ready;
                break;

            default:
                return;
        }
    }

    public void UpdateMessage(string newMessageString)
    {
        newMessageString = newMessageString.Substring(0, 1);
        //Debug.Log("Update Message: " + newMessageString);
        Symbol newMessage;
            switch (newMessageString)
            {
                case "i":
                    newMessage = Symbol.I;
                    break;
                case "o":
                    newMessage = Symbol.O;
                    break;
                case "k":
                    newMessage = Symbol.K;
                    break;
                case "l":
                    newMessage = Symbol.L;
                    break;
                default:
                //Debug.Log(newMessageString.Length);
                    return;
            }
        //Debug.Log("new Message: " + newMessage.ToString());
            if (newMessage == activeMessage)
                return;

            activeMessage = newMessage;
            messageUpdated = true;
    }

    public void UpdateSymbol(string newSymbolString)
    {
        newSymbolString = newSymbolString.Substring(0, 1);
        //Debug.Log("Update Symbol: '" + newSymbolString + "'");
            switch (newSymbolString)
            {
                case "i":
                    activeSymbol = Symbol.I;
                    break;
                case "o":
                    activeSymbol = Symbol.O;
                    break;
                case "k":
                    activeSymbol = Symbol.K;
                    break;
                case "l":
                    activeSymbol = Symbol.L;
                    break;
                default:
                    return;
            }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
