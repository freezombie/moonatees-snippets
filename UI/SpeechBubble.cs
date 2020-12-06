using UnityEngine;
using System.Collections;
using UnityEngine.UI; // To be able to get Text elements

public class SpeechBubble : MonoBehaviour
{
    private GameObject targetToFollow;
    private float followingSpeed = 3f;
    public float offsetHeight = 2f;
    public float distanceTrigger = 8;
    public float tooCloseDistance = 1;
    public float tooFarDistance = 8;

    private RectTransform myRectTransform;

    private float time;

    private Text textElement;

    private int textLength;

    private Image imageElement;
    private Camera camera;

    private float distanceToPlayer;
    private float speechBoxAlphaValue = 0.8f;

    public bool maxReached = false;
    public bool rollingTextIn = false;
    public bool rollingTextOut = false;
    public bool minReached = false;

    private string text;
    private string bankText;
    private int bankTextLength;

    public bool changingText = false;

    private SpeechBank speechBank;

    private int previousRandom;

    private float differenceToMin;
    private float differenceToMax;

    private float closeAlphaValue;
    private float farAlphaValue;

    public bool followTarget = false;

    private int currentSpeechNumber = 0;
    public float requiredDisplayTime = 0;
    public bool currentSpeechShownEnough = false;
    public float speechTimer = 0;

    private Renderer parentRenderer;

    private InputOptionsMessage inputOptionsMessage;

    private Vector3 startingPosition;

    public bool fadingUp = false;
    public bool fadingDown = false;

    private float fadingSpeed = 20f;

//    private float minTextAlpha = 0;
    private float maxTextAlpha = 1;
    private float maxBoxAlpha = 0.8f;

    public int firstSpeechIndex;

    public bool forceFadingAway = false;

    public Sprite nonFocusedSprite;
    public Sprite focusedSprite;

    private Image mySprite;

    private string myParentName;
    private Canvas myCanvas;

    public bool beFullyTransparent = false;

    void Start()
    {
        camera = Camera.main;
        textElement = this.GetComponentInChildren<Text>();

        myRectTransform = this.GetComponent<RectTransform>();

        myRectTransform.sizeDelta = new Vector2(myRectTransform.rect.width, 0);  // Sets the size by default to 0, so it's closed
        imageElement = GetComponent<Image>();
        imageElement.canvasRenderer.SetAlpha(speechBoxAlphaValue); // Base alpha value to 0.8f

        speechBank = GameObject.FindGameObjectWithTag("SpeechBank").GetComponent<SpeechBank>();

        speechBank.addSpeechToList(this);

        myParentName = this.transform.parent.name;

        myCanvas = this.GetComponent<Canvas>();

        //inputOptionsMessage = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputOptionsMessage>();
       
        /*
        if (inputOptionsMessage == null)
        {
            Debug.Log("This ducking SpeechBubble.cs couldn't find GameController tag");
        }
        */

        imageElement.canvasRenderer.SetAlpha(0.7f);
        textElement.canvasRenderer.SetAlpha(1);

        targetToFollow = this.transform.parent.gameObject;

        mySprite = this.GetComponent<Image>();

        if (beFullyTransparent)
        {
            setSpeechAlphas(0, 0);
            imageElement.canvasRenderer.SetAlpha(0);
            textElement.canvasRenderer.SetAlpha(0);
        }
    }

    public void setSpeechAlphas(float textMax, float boxMax)
    {
        maxTextAlpha = textMax;
        maxBoxAlpha = boxMax;
    }

    public void setBeFullyTransparentOff()
    {
        beFullyTransparent = false;
        imageElement.canvasRenderer.SetAlpha(maxBoxAlpha);
        textElement.canvasRenderer.SetAlpha(maxTextAlpha);

        // Toggle version
        /*
        beFullyTransparent = !beFullyTransparent;

        if (beFullyTransparent)
        {
            imageElement.canvasRenderer.SetAlpha(0);
            textElement.canvasRenderer.SetAlpha(0);
        }
        else
        {
            imageElement.canvasRenderer.SetAlpha(maxBoxAlpha);
            textElement.canvasRenderer.SetAlpha(maxTextAlpha);
        }
        */
    }

    public string getParentName()
    {
        return myParentName;
    }

    public void setCanvasLayer(int layer)
    {
        myCanvas.sortingOrder = layer;
    }

    public void getSpeechFromBank(int index)
    {		
        bankText = speechBank.getSpeech(index);
        bankTextLength = bankText.Length;
        currentSpeechNumber = index;
        requiredDisplayTime = speechBank.getRequiredDisplayTime(index);
        currentSpeechShownEnough = false;
        changingText = true;

        setSpeechAlphas(1f, 0.8f);

    }
        
    public void callFadeAlphaDown()
    {
        if (!fadingDown)
        {
            StopAllCoroutines();
            fadingUp = false;
            StartCoroutine(fadeAlphasDown());
            fadingDown = true;
        } 
    }

    public void callFadeAlphaUp()
    {
        if (!fadingUp)
        {
            StopAllCoroutines();
            fadingDown = false;
            StartCoroutine(fadeAlphasUp());
            fadingUp = true;
        }
    }

    IEnumerator fadeAlphasDown()
    {

        float start = Time.time;
        while (imageElement.canvasRenderer.GetAlpha() >= 0)
        {

            imageElement.canvasRenderer.SetAlpha(imageElement.canvasRenderer.GetAlpha() - 0.1f * Time.deltaTime * fadingSpeed);
            textElement.canvasRenderer.SetAlpha(imageElement.canvasRenderer.GetAlpha() - 0.1f * Time.deltaTime * fadingSpeed);
            yield return 0;
        }
        fadingDown = false;
        yield return true;
    }

    IEnumerator fadeAlphasUp()
    {
        float start = Time.time;

        while (imageElement.canvasRenderer.GetAlpha() <= maxBoxAlpha)
        {
            imageElement.canvasRenderer.SetAlpha(imageElement.canvasRenderer.GetAlpha() + 0.1f * Time.deltaTime * fadingSpeed);
            yield return 0;
        }

        while (textElement.canvasRenderer.GetAlpha() <= maxTextAlpha)
        {
            textElement.canvasRenderer.SetAlpha(textElement.canvasRenderer.GetAlpha() + 0.1f * Time.deltaTime * fadingSpeed);
            yield return 0;
        }

        fadingUp = false;
        yield return true;
    }

    public void setSpeechShown(int index)
    {
        speechBank.setSpeechShown(1);
    }

    public void enableForceFadingAway()
    {
        forceFadingAway = true;
    }

    public bool isForceFadingAwayOn()
    {
        return forceFadingAway;
    }

    public void setFocusedSprite()
    {
        if (mySprite.sprite != focusedSprite)
        {
            mySprite.sprite = focusedSprite;
        }
    }

    public void setNonFocusedSprite()
    {		
        if (mySprite.sprite != nonFocusedSprite)
        {	
            mySprite.sprite = nonFocusedSprite;
        }
    }

    void Update()
    {
        if (!forceFadingAway)
        {            
            if (!beFullyTransparent)
            {
                if (currentSpeechNumber == 0)
                {
                    getSpeechFromBank(firstSpeechIndex);
                }

                // Get distance to the player
                distanceToPlayer = Vector3.Distance(this.transform.position, camera.transform.position);

                // Check when player is close enough to play speech
                if (distanceToPlayer < distanceTrigger)
                {
                    if (!changingText)
                    {
                        if (!maxReached && !currentSpeechShownEnough)
                        {
                            callFadeAlphaUp();
                            rollingTextIn = true;
                            rollTextIn(1);
                        }
                        else
                            rollingTextIn = false;
                    }
                    else
                    {
                        rollingTextOut = true;
                        rollTextOut(-2);
                    }
                }
                else
                {
                    if (!minReached)
                    {
                        callFadeAlphaDown();
                        rollingTextOut = true;
                        rollTextOut(-2);
                    }
                    else
                        rollingTextOut = false;
                }
                //				}

                // Fadings of the box and text when player gets too close or far
                differenceToMin = tooCloseDistance - distanceToPlayer + 1;
                differenceToMax = distanceToPlayer + 1 - tooFarDistance;

                if (differenceToMin > 0)
                {					
                    callFadeAlphaDown();
                } 

                if (differenceToMax > -5 && differenceToMax < 4)
                {
                    callFadeAlphaUp();
                }

            } // beFullyTransparent check

            // Turn the speech bubble to always face player
            this.transform.rotation = Quaternion.LookRotation(camera.transform.forward);

            // Move the bubble with the following target if public boolean enabled
            if (followTarget)
            {
                Vector3 targetPosition = new Vector3(targetToFollow.transform.position.x, targetToFollow.transform.position.y + offsetHeight, targetToFollow.transform.position.z);
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, followingSpeed * Time.deltaTime);
            }
        }
    }

    public void rollTextIn(int rollSpeed)
    {
        int currentTextLength = textElement.text.Length;

        if (currentTextLength < bankTextLength)
        {
            textElement.text = bankText.Substring(0, currentTextLength + 1);
            minReached = false;
        }
        else
        {
            maxReached = true;
        }
    }

    public void rollTextOut(int rollSpeed)
    {
        int currentTextLength = textElement.text.Length;

        if (currentTextLength > 0)
        {
            textElement.text = textElement.text.Substring(0, currentTextLength - 1);
            maxReached = false;

        }
        else
        {
            minReached = true;
            if (changingText)
            {
                rollingTextOut = false;
                changingText = false;
            }
        }		
    }
}
