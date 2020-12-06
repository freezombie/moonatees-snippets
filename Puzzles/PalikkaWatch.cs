using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalikkaWatch : MonoBehaviour 
{
    public List<GameObject> palikkas = new List<GameObject>();
    public bool distanceCheckGoing = false;

    public bool checkingEnabled = true;

    private int counter = 1;

    private GameObject squareSpawnPoint, triangleSpawnPoint, circleSpawnPoint, rectangleSpawnPoint;

    GameObject triangle, circle, square, rectangle;
    GameObject particeSpot1, particeSpot2, particeSpot3, particeSpot4;


	// Use this for initialization
	void Start () 
    {
        squareSpawnPoint = this.transform.FindChild("SquareSpawnPoint").gameObject;
        triangleSpawnPoint = this.transform.FindChild("TriangleSpawnPoint").gameObject;
        circleSpawnPoint = this.transform.FindChild("CircleSpawnPoint").gameObject;
        rectangleSpawnPoint = this.transform.FindChild("RectangleSpawnPoint").gameObject;

        // Spawning palikkas
        triangle =  Instantiate(Resources.Load("Triangle")) as GameObject;
        Vector3 originalScale1 = triangle.transform.localScale;
        triangle.transform.parent = this.transform; 
        triangle.transform.localScale = originalScale1;
        triangle.transform.position = triangleSpawnPoint.transform.position;

        circle =  Instantiate(Resources.Load("Circle")) as GameObject;
        Vector3 originalScale2 = circle.transform.localScale;
        circle.transform.parent = this.transform; 
        circle.transform.localScale = originalScale2;
        circle.transform.position = circleSpawnPoint.transform.position;

        square =  Instantiate(Resources.Load("Square")) as GameObject;
        Vector3 originalScale3 = square.transform.localScale;
        square.transform.parent = this.transform; 
        square.transform.localScale = originalScale3;
        square.transform.position = squareSpawnPoint.transform.position;

        rectangle =  Instantiate(Resources.Load("Rectangle")) as GameObject;
        Vector3 originalScale4 = rectangle.transform.localScale;
        rectangle.transform.parent = this.transform; 
        rectangle.transform.localScale = originalScale4;
        rectangle.transform.position = rectangleSpawnPoint.transform.position;
            
        foreach (MeshCollider go in this.GetComponentsInChildren<MeshCollider>())
            palikkas.Add(go.gameObject);


	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!distanceCheckGoing && checkingEnabled)
        {
            StartCoroutine(checkDistances(1f));
            distanceCheckGoing = true;
        }
	}

    public void setPalikatKinematic()
    {
        foreach (GameObject go in palikkas)
        {               
                go.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void destroyPalikat()
    {
        foreach (GameObject go in palikkas)
        {
            Destroy(go.gameObject);
        }
    }

    public void disableChecking()
    {
        setPalikatKinematic();
        checkingEnabled = false;
    }

    public bool checkIfEachPalikkaHasARigidbody()
    {
        bool pass = true;

        foreach (GameObject go in palikkas)
        {
            if (go.GetComponent<Rigidbody>() == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator checkDistances(float delay)
    {
        foreach (GameObject go in palikkas)
        {
            if (Vector3.Distance(this.transform.position, go.transform.position) > 4f && go.GetComponent<Rigidbody>() != null)
            {
                if (go.CompareTag("SquarePalikka"))
                {
                    particeSpot1 = Instantiate(Resources.Load("DisappearingSmoke")) as GameObject;
                    particeSpot1.transform.position = go.transform.position;
                    go.transform.position = squareSpawnPoint.transform.position;
                    go.transform.rotation = Quaternion.identity;

                }
                if (go.CompareTag("TrianglePalikka"))
                {
                    particeSpot2 = Instantiate(Resources.Load("DisappearingSmoke")) as GameObject;
                    particeSpot2.transform.position = go.transform.position;
                    go.transform.position = triangleSpawnPoint.transform.position;
                    go.transform.rotation = Quaternion.identity;
                   
                }
                if (go.CompareTag("RectanglePalikka"))
                {
                    particeSpot3 = Instantiate(Resources.Load("DisappearingSmoke")) as GameObject;
                    particeSpot3.transform.position = go.transform.position;
                    go.transform.position = rectangleSpawnPoint.transform.position;
                    go.transform.rotation = Quaternion.identity;

                }
                if (go.CompareTag("CirclePalikka"))
                {
                    particeSpot4 = Instantiate(Resources.Load("DisappearingSmoke")) as GameObject;
                    particeSpot4.transform.position = go.transform.position;
                    go.transform.position = circleSpawnPoint.transform.position;
                    go.transform.rotation = Quaternion.identity;

                }
                
                go.GetComponent<Rigidbody>().velocity = Vector3.zero;
                go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                break;
            }
        }

        yield return new WaitForSeconds(delay);
        distanceCheckGoing = false;
    }

}
