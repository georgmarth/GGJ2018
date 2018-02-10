using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackMessage : MonoBehaviour {

    public float FeedbackInterval = 5f;
    public float goodStrength = 70f;
    public float badStrength = 30f;

    public MessageDisplay messageDisplay;

    float timer;

	// Use this for initialization
	void Start () {
        timer = FeedbackInterval;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (PlayManager.Instance.signalStrength > goodStrength)
            {
                messageDisplay.DisplayMessage(PlayManager.Symbol.Good);
                timer = FeedbackInterval;
            }
            else if (PlayManager.Instance.signalStrength < badStrength)
            {
                messageDisplay.DisplayMessage(PlayManager.Symbol.Bad);
                timer = FeedbackInterval;
            }

        }
    }
}
