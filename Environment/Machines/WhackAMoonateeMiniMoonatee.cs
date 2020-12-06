using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackAMoonateeMiniMoonatee : MonoBehaviour 
{
    bool poppingUp;
    bool movingDown;
    Vector3 originalLocalPos;
    float speed;
    Rigidbody rb;
    int index;
    WhackAMoonatee wam;
    //float stayTimeCounter; // this is what we modify and countdown in code.
    float stayTime;
    bool startCountdown; // whether or not we are counting down on staytimecounter.
    bool hit = false;
    public AudioClip popupSound;
    public AudioClip hitSound;
    AudioSource audioSource;
    public enum Direction {X,Y,Z};
    public Direction targetDirection;
    public enum Polarity {Plus,Minus} // couldn't remember the better word :D
    public Polarity targetPolarity; 
    public float targetDestination; // localposition of Direction of where we are going to rise
    bool isTrex = false;
    GameObject TRexDoor = null;
    Animator TRexAnimator = null;
    Vector3 TRexDooOriginalPos;
    Vector3 TRexDoorUpPos;
    Quaternion originalRot;
    float tRexTiltAnimationLength;
    bool helperBoolean = false;

    void Awake()
    {        
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        wam = transform.parent.GetComponent<WhackAMoonatee>();
        rb = GetComponent<Rigidbody>();
        originalLocalPos = transform.localPosition;
        originalRot = transform.rotation;
        if (transform.name == "TRex")
        {
            isTrex = true;
        }
        if (targetDirection != Direction.X)
        {
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionZ; // this is because in the scene the platform has been rotated 90 degrees. Otherwise it should be freezepositionx
        }
        if (targetDirection != Direction.Y)
        {
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
        }
        if (targetDirection != Direction.Z)
        {
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionX; // this is because in the scene the platform has been rotated 90 degrees. Otherwise it should be freezepositionz
        }
        if (!rb.isKinematic)
        {
            rb.isKinematic = true;
        }
        if (isTrex)
        {
            TRexDoor = transform.parent.FindChild("TRexDoor").gameObject;
            TRexDooOriginalPos = TRexDoor.transform.localPosition;
            TRexDoorUpPos = new Vector3(TRexDooOriginalPos.x, TRexDooOriginalPos.y + 1.55f, TRexDooOriginalPos.z);
//            Debug.Log("TrexDoorOriginalPos: " + TRexDooOriginalPos + " trexdooruppos: " + TRexDoorUpPos);
            TRexAnimator = GetComponent<Animator>();
            TRexAnimator.enabled = false;
            tRexTiltAnimationLength = wam.GetTRexTiltAnimation().length;
        }
    }

    float checkDistance()
    {
        switch (targetDirection)
        {
            case Direction.X:
                return Vector3.Distance(transform.localPosition, new Vector3(targetDestination,transform.localPosition.y , transform.localPosition.z));
                break;
            case Direction.Y:
                return Vector3.Distance(transform.localPosition, new Vector3(transform.localPosition.x, targetDestination, transform.localPosition.z));
                break;
            case Direction.Z:
                return Vector3.Distance(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, targetDestination));
                break;
            default:
                return 666;
                Debug.Log("this should not happen");
                break;
        }         
    }

    bool IsBelowOriginal()
    {
        bool value;
        switch (targetDirection)
        {
            case Direction.X:
                value = transform.localPosition.x < originalLocalPos.x;
                break;
            case Direction.Y:
                value = transform.localPosition.y < originalLocalPos.y;
                break;
            case Direction.Z:
                value = transform.localPosition.z < originalLocalPos.z;
                break;
            default:
                value = false;
                Debug.Log("this should not happen");
                break;
        }
        if (targetPolarity == Polarity.Minus)
            return !value;
        else
            return value;
    }

    bool IsOverTarget()
    {
        bool value;
        switch (targetDirection)
        {
            case Direction.X:
                value = transform.localPosition.x > targetDestination;
                break;
            case Direction.Y:
                value = transform.localPosition.y > targetDestination;
                break;
            case Direction.Z:
                value = transform.localPosition.z > targetDestination;
                break;
            default:
                value = false;
                Debug.Log("this should not happen");
                break;
        }
        if (targetPolarity == Polarity.Minus)
            return !value;
        else
            return value;
    }

    void ResetToTarget()
    {
        switch (targetDirection)
        {
            case Direction.X:
                transform.localPosition = new Vector3(targetDestination, transform.localPosition.y, transform.localPosition.z);
                break;
            case Direction.Y:
                transform.localPosition = new Vector3(transform.localPosition.x,targetDestination, transform.localPosition.z);
                break;
            case Direction.Z:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, targetDestination);
                break;
        }
    }

    void Update()
    {
        if (poppingUp)
        {
            if (isTrex)
            {
                TRexDoor.transform.localPosition = Vector3.Lerp(TRexDoor.transform.localPosition, TRexDoorUpPos, speed);
            }
            Vector3 helper = transform.localPosition;;
            //Debug.Log("moving because of popping up");
            switch (targetDirection)
            {                
                case Direction.X:                    
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(targetDestination, transform.localPosition.y, transform.localPosition.z), speed);
                    /*if (helper.x < transform.localPosition.x)
                    {
                        Debug.Log("the polarity of " + transform.name + " should be plus");
                    }*/
                    break;
                case Direction.Y:
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, targetDestination, transform.localPosition.z), speed);
                    /*if (helper.y < transform.localPosition.y)
                    {
                        Debug.Log("the polarity of " + transform.name + " should be plus");
                    }*/
                    break;
                case Direction.Z:                    
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x,transform.localPosition.y, targetDestination), speed);
                    /*if (helper.z < transform.localPosition.z)
                    {
                        Debug.Log("the polarity of " + transform.name + " should be plus");
                    }*/
                    break;
            }
            if (checkDistance()< 0.1f)
            {
                if (isTrex)
                {                    
                    TRexAnimator.enabled = true;
                    TRexAnimator.Play("TRexTilt");
                    TRexAnimator.SetFloat("Speed", 1f);
                }
                startCountdown = true;
                poppingUp = false;
                //Debug.Log("Reached the top on " + this.name);
            }
        }

        if (transform.localPosition != originalLocalPos && IsBelowOriginal())
        {
            //Debug.Log("Resetting " + this.transform.name);
            // tässä trex animaattori reset
            if (isTrex)
            {
                TRexAnimator.enabled = false;
                transform.rotation = originalRot;
            }
            rb.velocity = Vector3.zero;
            transform.localPosition = originalLocalPos;
        }

        if (movingDown)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPos, speed);
            if (isTrex)
            {
                TRexDoor.transform.localPosition = Vector3.Lerp(TRexDoor.transform.localPosition, TRexDooOriginalPos, speed);
            }
            if (Vector3.Distance(transform.localPosition, originalLocalPos) < 0.1f)
            {
                if (isTrex && Vector3.Distance(TRexDoor.transform.localPosition,TRexDooOriginalPos) > 0.1f)
                {
                    return;
                }
                movingDown = false;
                wam.MissOn(index);//kerro wamille et pelaaja ei kerenny lyömään tätä
                rb.isKinematic = true;
            }
        }

        if (hit && isTrex)
        {
            if (Vector3.Distance(transform.localPosition, originalLocalPos) < 0.1f)
            {
                TRexDoor.transform.localPosition = Vector3.Lerp(TRexDoor.transform.localPosition, TRexDooOriginalPos, speed);
            }
        }

        if (startCountdown)
        {
            stayTime -= Time.deltaTime;
            if (stayTime <= 0)
            {
                stayTime = 0;
                if (isTrex)
                {                    
                    if (!helperBoolean)
                    {
//                        Debug.Log("This happens often");
                        AnimatorStateInfo state = TRexAnimator.GetCurrentAnimatorStateInfo(0);
                        TRexAnimator.Play("TRexTilt", 0, state.length);
                        TRexAnimator.SetFloat("Speed", -1f);
                        StartCoroutine(WaitAndDisable(state.length));
                        helperBoolean = true;
                    }
                    //TRexAnimator.Play("TRexTilt",0, tRexTiltAnimationLength*2);

                    //StartCoroutine(WaitAndDisable(tRexTiltAnimationLength));
                }
                else
                {
                    movingDown = true;
                    startCountdown = false;
                }                               
            }
        }

        if (hit && IsOverTarget()) // this seems like it's fundamentally wrong. Probably it should be for if the manatee gets hit and doesn't reach the end. Now it resets to where it was i think...
        {
            Debug.Log("resetting to target");
            rb.velocity = Vector3.zero;
            ResetToTarget();
        }
    }

    IEnumerator WaitAndDisable(float time)
    {
        yield return new WaitForSeconds(time);
        TRexAnimator.enabled = false;
        helperBoolean = false;
        movingDown = true;
        startCountdown = false;
    }

    public void PopUp(float speed,int index,float stayTime)
    {
        hit = false;
        this.stayTime = stayTime;
        poppingUp = true;
        this.speed = speed;
        rb.isKinematic = false;
        this.index = index;
        audioSource.clip = popupSound;
        audioSource.Play();
        if (isTrex)
        {
            TRexAnimator.SetFloat("Speed", 1f);
        }
    }

    public void MoveDown()
    {
        movingDown = true;
    }

    void OnCollisionEnter()
    {
        // mittaa ćollision voima/velocity ja sen mukaan rekisteröi oliko osuma vai ei. Ehkä suunta?
        if (!rb.isKinematic && !hit)
        {
            poppingUp = false;
            hit = true;
            audioSource.clip = hitSound;
            audioSource.Play();
            wam.HitOn(index);// kerro wamille et pelaaja löi tätä.
            if (isTrex)
            {
                TRexAnimator.enabled = false;
            }
        }
    }
}
