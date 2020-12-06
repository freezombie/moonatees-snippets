using UnityEngine;
using System.Collections;

public class CanteenGameCannon : MonoBehaviour
{
    public GameObject[] projectiles;

    enum State {Idle = 0, ReadyToFire = 1, FiringOnCooldown = 2, GameOver = 3};
    State state;

    private GameObject projectileSpawnPoint;
    private BoxCollider myGameStartingCollider;

    private GameObject player;
    private Vector3 playerOffsetPosition;
    private GameObject rightBat;
    private GameObject leftBat;

    private float aimingOffset = 0.8f;
    private float randomRotation = 3f;

    private bool playerHasBats = false;

    public int ballsLeft = 10;

    public BallGameScoreCounter bgcc;

    void Start ()
    {
        state = State.ReadyToFire;

        projectileSpawnPoint = this.transform.FindChild("ProjectileSpawnPoint").gameObject;

        if (projectileSpawnPoint == null)
        {
            Debug.Log("Projectile spawn point could not be found for ball game!");
        }

        myGameStartingCollider = this.transform.FindChild("GameStartingCollider").GetComponent<BoxCollider>();

        player = GameObject.FindGameObjectWithTag("Player");

        bgcc = GameObject.FindGameObjectWithTag("BallGameScores").GetComponent<BallGameScoreCounter>();
    }

    void Update ()
    {

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "BoxColliderBottom" || col.name == "BoxColliderMid" || col.name == "BoxColliderTop")
        {
            if (rightBat == null)
                rightBat = player.transform.FindChild("Controller (right) [Physical]/VRFlipperPivotRight(Clone)/FlipperBat").gameObject;
            if (leftBat == null)
                leftBat = player.transform.FindChild("Controller (left) [Physical]/VRFlipperPivotLeft(Clone)/FlipperBat").gameObject;

            rightBat.SetActive(true);
            leftBat.SetActive(true);

            playerHasBats = true;
        }
    }
   
    public void OnTriggerExit(Collider col)
    {
        if (col.name == "BoxColliderBottom" || col.name == "BoxColliderMid" || col.name == "BoxColliderTop")
        {
            if (playerHasBats)
            {
                rightBat.SetActive(false);
                leftBat.SetActive(false);

                ballsLeft = 10;
                bgcc.setBallsLeft(10);
                bgcc.resetScore();

            }
        }
    }

    public void OnTriggerStay(Collider col)

    {
        if (col.name == "BoxColliderBottom" || col.name == "BoxColliderMid" || col.name == "BoxColliderTop")
        {
            if (state == State.ReadyToFire && ballsLeft > 0)
            {
                state = State.FiringOnCooldown;

                // Get new player position
                playerOffsetPosition = player.transform.position;
                playerOffsetPosition.y += 2f;

                playerOffsetPosition.x += Random.Range(-aimingOffset, aimingOffset);
                playerOffsetPosition.y += Random.Range(-aimingOffset * 0.5f, aimingOffset * 0.5f);

                Vector3 targetDirection = projectileSpawnPoint.transform.position - playerOffsetPosition;

                Quaternion rotationTowardsPlayer = Quaternion.LookRotation(targetDirection);

                GameObject projectile =  Instantiate(Resources.Load("Projectiles/BallgameBall"), projectileSpawnPoint.transform.position, rotationTowardsPlayer) as GameObject;

                projectile.GetComponentInChildren<Rigidbody>().AddForce(-projectile.transform.forward * 1f, ForceMode.Impulse);

                ballsLeft -= 1;

                // Send the amount of balls left to the score counter
                bgcc.setBallsLeft(ballsLeft);

                StartCoroutine(FiringCooldown(2f));
            }

            if (state == State.ReadyToFire && ballsLeft == 0)
            {
                StartCoroutine(FiringCooldown(2f));
                state = State.GameOver;
                Debug.Log("Game over");
            }
        }
    }


    void resetAll()
    {
        state = State.Idle;
    }

	IEnumerator FiringCooldown(float time)
	{
		yield return new WaitForSeconds(time);
        state = State.ReadyToFire;            
	}
}