using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For Text and other UI elements

public class BallGameScoreCounter : MonoBehaviour 
{
    public int score = 0;
    public int ballsLeft = 0;

    public Text ballsLeftText;
    public Text ballsLeftAmountText;
    public Text scoreText;

	// Use this for initialization
	void Start () 
    {

        ballsLeftAmountText = this.transform.FindChild("BallsLeftAmount").gameObject.GetComponent<Text>();

        scoreText = this.transform.FindChild("ScoreText").GetComponent<Text>();
        scoreText.text = "0";

        ballsLeftAmountText.text = ballsLeft.ToString();
        scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void resetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
        ballsLeftAmountText.text = ballsLeft.ToString();
    }

    public void setBallsLeft(int amount)
    {
        ballsLeft = amount;
        ballsLeftAmountText.text = ballsLeft.ToString();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("BallGameProjectile"))
        {
            score += 10;


            GameObject smokeParticles = Instantiate(Resources.Load("DisappearingSmoke", typeof(GameObject))) as GameObject;
            smokeParticles.transform.position = col.transform.position;
            Destroy(col.gameObject);

            scoreText.text = score.ToString();
        }
    }
}
