using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour {

    //public float messageTimeOut = 3f;
    public GameObject messagePrefab;

    //private float timer;

    public Sprite LImage;
    public Sprite KImage;
    public Sprite OImage;
    public Sprite IImage;

    public Sprite GoodImage;
    public Sprite BadImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*
		if (messageInstance != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                TimeOut();
            }
        }
        */
	}

    void TimeOut()
    {
        /*
        if (messageInstance!= null)
            Destroy(messageInstance);
        messageInstance = null;
        timer = messageTimeOut;
        */
    }

    public void TestDisplay()
    {
        DisplayMessage(PlayManager.Symbol.I);
    }

    public void DisplayMessage(PlayManager.Symbol symbol)
    {/*
        if (messageInstance != null)
        {
            Destroy(messageInstance);
        }
        */
        GameObject messageInstance = Instantiate(messagePrefab, transform);
        Sprite image;
        switch(symbol)
        {
            case PlayManager.Symbol.I:
                image = IImage;
                break;
            case PlayManager.Symbol.O:
                image = OImage;
                break;
            case PlayManager.Symbol.K:
                image = KImage;
                break;
            case PlayManager.Symbol.L:
                image = LImage;
                break;
            case PlayManager.Symbol.Good:
                image = GoodImage;
                break;
            case PlayManager.Symbol.Bad:
                image = BadImage;
                break;
            default:
                return;
        }

        messageInstance.GetComponentInChildren<Image>().sprite = image;
        //timer = messageTimeOut;
    }
}
