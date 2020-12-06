using UnityEngine;
using System.Collections;
using Valve.VR;

public class WandController : SteamVR_TrackedController
{
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int) controllerIndex); } }
	public Vector3 velocity { get { return controller.velocity; } }
    public Vector3 angularVelocity { get { return controller.angularVelocity; } }
    public Rigidbody player;
    public GameObject cameraHelper;
    bool padTouchedHelper;

    public float movementForce = 1000;
    // Use this for initialization
	protected override void Start ()
    {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update()
    {
   
        base.Update();
        Debug.DrawRay(player.transform.position, cameraHelper.transform.forward * 100, Color.green);
        Debug.DrawRay(player.transform.position, cameraHelper.transform.right * 100, Color.red);
        if (padTouched)
        {
            padTouchedHelper = true;
        }
        else
        {
            padTouchedHelper = false;
        }
        // we need to get where the player is looking from the headset it seems.        
    
	}

    void FixedUpdate()
    {

        // Normal movement
        if (padPressed && (controllerIndex == 3 || controllerIndex == 2) && !triggerPressed)
        {
            player.AddForce((cameraHelper.transform.forward * movementForce * GetTouchpadAxis().y) * Time.deltaTime, ForceMode.Acceleration);
            player.AddForce((cameraHelper.transform.right * movementForce * GetTouchpadAxis().x) * Time.deltaTime, ForceMode.Acceleration);
            //player.transform.Translate(-cameraHelper.transform.forward * GetTouchpadAxis().y * Time.deltaTime);
            //player.transform.Translate(-cameraHelper.transform.right * GetTouchpadAxis().x * Time.deltaTime);
        }

        // Strafing
        else if (padPressed && (controllerIndex == 3 || controllerIndex == 2) && triggerPressed)
        {
            player.AddForce((cameraHelper.transform.up * movementForce * GetTouchpadAxis().y) * Time.deltaTime, ForceMode.Acceleration);
            player.AddForce((cameraHelper.transform.right * movementForce * GetTouchpadAxis().x) * Time.deltaTime, ForceMode.Acceleration);
        }

        // Rotating
        if (padPressed && (controllerIndex == 4 || controllerIndex == 1))
        {
            //player.transform.Rotate(cameraHelper.transform.right * GetTouchpadAxis().y * Time.deltaTime * 100);
            player.transform.Rotate(player.transform.up * GetTouchpadAxis().x * Time.deltaTime * 50);
        }   
        
    }
    // Start of temp code
    public override void OnPadClicked(ClickedEventArgs e)
    {
        base.OnPadClicked(e);        
    }

    public override void OnPadUnclicked(ClickedEventArgs e)
    {
        base.OnPadUnclicked(e);
    }

    public override void OnPadTouched(ClickedEventArgs e)
    {
        base.OnPadTouched(e);
        // I have a feeling we do all the things needed to move the player here
        // the other functions migth be totally unnecessary        
        // e.padX + "," + e.padY; this also exists    
    }

    public override void OnPadUntouched(ClickedEventArgs e)
    {
        base.OnPadUntouched(e);
        //Debug.Log("Pad Untouched " + GetTouchpadAxis());
    }

    public float GetTriggerAxis()
    {        
        if (controller == null)
        {
            return 0;
        }   
                         
        return controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis1).x;
    }
    // end of temp code

    public Vector2 GetTouchpadAxis()
    {
        if(controller == null)
        {
            return new Vector2();
        }

        return controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        // returns things like this http://russellsoftworks.com/blog/steamvr_01/img/diag1.jpg
    }
}
