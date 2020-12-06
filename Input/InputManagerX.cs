using UnityEngine;
using System.Collections;
using TeamUtility.IO;
using Valve.VR;

//public class InputManagerX {
public class InputManagerX : InputManager {
	private static SteamVR_ControllerManager controllerManager;
	private static float touchSens = 0.7f;

	public static bool IsSteamVREnabled() {
        if (!Camera.main.GetComponent<SteamVR_TrackedObjectX>()) return false;
        return SteamVR.enabled;
	}

    public static SteamVR_ControllerManager GetControllerManager() {
        if (!controllerManager) controllerManager = Camera.main.GetComponentInParent<SteamVR_ControllerManager>();
        return controllerManager;
    }

    public static bool IsSteamVRControllerEnabled()
    {
        if (!InputManagerX.IsSteamVREnabled()) return false;
        if (!InputManagerX.GetControllerManager()) return false;
        if (!InputManagerX.GetControllerManager().left.GetComponent<SteamVR_TrackedObjectX>().isValid) return false;
        if (!InputManagerX.GetControllerManager().right.GetComponent<SteamVR_TrackedObjectX>().isValid) return false;
        return true;
    }

    /*
    public static bool IsSixenseInputEnabled() {
		if (InputManagerX.IsSteamVRControllerEnabled()) return false;
		if (!GameObject.FindObjectOfType<SixenseInput>().enabled) return false;
		if (!SixenseInput.IsBaseConnected(0)) return false;
		if (!SixenseInput.Controllers[0].Enabled) return false;
		if (!SixenseInput.Controllers[1].Enabled) return false;

		return true;
	}
    */   

	public static bool IsAxisAvailable(string AxisName) {
		if(InputManager.GetAxisConfiguration(PlayerID.One, AxisName) != null) return true;

		return false;
	}

	public static bool IsButtonAvailable(string BtnName) {
		if(InputManager.GetAxisConfiguration(PlayerID.One, BtnName) != null) return true;

		return false;
	}

	public static bool GetDPAD(string BtnName) {
		switch (BtnName) {
		case "DPAD Up":
			if (IsAxisAvailable("DPADVertical") && (InputManager.GetAxis("DPADVertical") > 0)) return true;
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetViveDPAD(BtnName);
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[0].GetButton(SixenseButtons.THREE);
			return false;
		case "DPAD Down":
			if (IsAxisAvailable("DPADVertical") && (InputManager.GetAxis("DPADVertical") < 0)) return true;
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetViveDPAD(BtnName);
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[0].GetButton(SixenseButtons.TWO);
			return false;
		case "DPAD Left":
			if (IsAxisAvailable("DPADHorizontal") && (InputManager.GetAxis("DPADHorizontal") < 0)) return true;
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetViveDPAD(BtnName);
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[0].GetButton(SixenseButtons.ONE);
			return false;
		case "DPAD Right":
			if (IsAxisAvailable("DPADHorizontal") && (InputManager.GetAxis("DPADHorizontal") > 0)) return true;
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetViveDPAD(BtnName);
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[0].GetButton(SixenseButtons.FOUR);
			return false;
		default:
			return false;
		}
	}

	public static bool GetViveDPAD(string BtnName) {
		if (!SteamVR_Controller.Input(1).GetPress(SteamVR_Controller.ButtonMask.Axis0)) return false;

		switch (BtnName) {
		case "DPAD Up":
			if (SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).y > touchSens) return true;
			return false;
		case "DPAD Down":
			if (SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).y < -touchSens) return true;
			return false;
		case "DPAD Left":
			if (SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).x < -touchSens) return true;
			return false;
		case "DPAD Right":
			if (SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).x > touchSens) return true;
			return false;
		default:
			return false;
		}
	}

	public static bool GetViveABXY(string BtnName) {
		if (!SteamVR_Controller.Input(2).GetPress(SteamVR_Controller.ButtonMask.Axis0)) return false;

		switch (BtnName) {
		case "A":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y < -touchSens) return true;
			return false;
		case "B":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x > touchSens) return true;
			return false;
		case "X":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x < -touchSens) return true;
			return false;
		case "Y":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y > touchSens) return true;
			return false;
		default:
			return false;
		}
	}

	public static bool GetViveABXYDown(string BtnName) {
		if (!SteamVR_Controller.Input(2).GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) return false;

		switch (BtnName) {
		case "A":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y < -touchSens) return true;
			return false;
		case "B":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x > touchSens) return true;
			return false;
		case "X":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x < -touchSens) return true;
			return false;
		case "Y":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y > touchSens) return true;
			return false;
		default:
			return false;
		}
	}

	public static bool GetViveABXYUp(string BtnName) {
		if (!SteamVR_Controller.Input(2).GetPressUp(SteamVR_Controller.ButtonMask.Axis0)) return false;

		switch (BtnName) {
		case "A":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y < -touchSens) return true;
			return false;
		case "B":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x > touchSens) return true;
			return false;
		case "X":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x < -touchSens) return true;
			return false;
		case "Y":
			if (SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y > touchSens) return true;
			return false;
		default:
			return false;
		}
	}

	public static Quaternion GetRotation(string ObjectName) {
		switch (ObjectName) {
		case "Hmd":
			if (InputManagerX.IsSteamVREnabled()) return Camera.main.GetComponent<SteamVR_TrackedObjectX>().pose.rot;
			return Quaternion.identity;
		case "Left":
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetControllerManager().left.GetComponent<SteamVR_TrackedObjectX>().pose.rot;
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[0].Rotation;
			return Quaternion.identity;
		case "Right":
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetControllerManager().right.GetComponent<SteamVR_TrackedObjectX>().pose.rot;
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[1].Rotation;
			return Quaternion.identity;
		default:
			return Quaternion.identity;
		}
	}

	public static Vector3 GetPosition(string ObjectName) {
		switch (ObjectName) {
		case "Hmd":
			if (InputManagerX.IsSteamVREnabled()) return Camera.main.GetComponent<SteamVR_TrackedObjectX>().pose.pos;
			return Vector3.zero;
		case "Left":
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetControllerManager().left.GetComponent<SteamVR_TrackedObjectX>().pose.pos;
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[0].Position;
			return Vector3.zero;
		case "Right":
			if (InputManagerX.IsSteamVRControllerEnabled()) return InputManagerX.GetControllerManager().right.GetComponent<SteamVR_TrackedObjectX>().pose.pos;
//			if (InputManagerX.IsSixenseInputEnabled()) return SixenseInput.Controllers[1].Position;
			return Vector3.zero;
		default:
			return Vector3.zero;
		}
	}

	new public static float GetAxis(string AxisName) {
		if (InputManager.GetAxis(AxisName) != 0) return InputManager.GetAxis (AxisName);

		if (InputManagerX.IsSteamVRControllerEnabled()) {
			switch (AxisName) {
			case "Horizontal":
				return SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).x;
			case "Vertical":
				return SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).y;
			case "LookHorizontal":
				return SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x;
			case "LookVertical":
				return SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y;
			case "LeftTrigger":
				return SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis1).x;
			case "RightTrigger":
				return SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis1).x;
			default:
				return 0;
			}
		}
        /*
		if (InputManagerX.IsSixenseInputEnabled()) {
			switch (AxisName) {
			case "Horizontal":
				return SixenseInput.Controllers[0].JoystickX;
			case "Vertical":
				return SixenseInput.Controllers[0].JoystickY;
			case "LookHorizontal":
				return SixenseInput.Controllers[1].JoystickX;
			case "LookVertical":
				return SixenseInput.Controllers[1].JoystickY;
			case "LeftTrigger":
				return SixenseInput.Controllers[0].Trigger;
			case "RightTrigger":
				return SixenseInput.Controllers[1].Trigger;
			default:
				return 0;
			}
		}
        */
		return 0;
	}

	new public static bool GetButton(string BtnName) {
		if (InputManager.GetButton(BtnName)) return InputManager.GetButton (BtnName);

		if (InputManagerX.IsSteamVRControllerEnabled()) {
			switch (BtnName) {
			case "A":
			case "B":
			case "X":
			case "Y":
				return InputManagerX.GetViveABXY(BtnName);
			case "LB":
			case "Move":
				return SteamVR_Controller.Input(1).GetPress(SteamVR_Controller.ButtonMask.Grip);
			case "RB":
			case "Look":
				return SteamVR_Controller.Input(2).GetPress(SteamVR_Controller.ButtonMask.Grip);
			case "LS":
				if (!SteamVR_Controller.Input(1).GetPress(SteamVR_Controller.ButtonMask.Axis0)) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).x) > touchSens) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).y) > touchSens) return false;
				return true;
			case "RS":
				if (!SteamVR_Controller.Input(2).GetPress(SteamVR_Controller.ButtonMask.Axis0)) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x) > touchSens) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y) > touchSens) return false;
				return true;
			case "Back":
			case "Cancel":
				return SteamVR_Controller.Input(1).GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
			case "Start":
			case "Submit":
				return SteamVR_Controller.Input(2).GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
			default:
				return false;
			}
		}
        /*
		if (InputManagerX.IsSixenseInputEnabled()) {
			switch (BtnName) {
			case "A":
				return SixenseInput.Controllers[1].GetButton(SixenseButtons.ONE);
			case "B":
				return SixenseInput.Controllers[1].GetButton(SixenseButtons.TWO);
			case "X":
				return SixenseInput.Controllers[1].GetButton(SixenseButtons.THREE);
			case "Y":
				return SixenseInput.Controllers[1].GetButton(SixenseButtons.FOUR);
			case "LB":
			case "Move":
				return SixenseInput.Controllers[0].GetButton(SixenseButtons.BUMPER);
			case "RB":
			case "Look":
				return SixenseInput.Controllers[1].GetButton(SixenseButtons.BUMPER);
			case "LS":
				return SixenseInput.Controllers[0].GetButton(SixenseButtons.JOYSTICK);
			case "RS":
				return SixenseInput.Controllers[1].GetButton(SixenseButtons.JOYSTICK);
			case "Back":
			case "Cancel":
				return SixenseInput.Controllers[0].GetButton(SixenseButtons.START);
			case "Start":
			case "Submit":
				return SixenseInput.Controllers[1].GetButton(SixenseButtons.START);
			default:
				return false;
			}
		}
        */
		return false;
	}

	new public static bool GetButtonDown(string BtnName) {
		if (InputManager.GetButtonDown(BtnName)) return InputManager.GetButtonDown (BtnName);

		if (InputManagerX.IsSteamVRControllerEnabled()) {
			switch (BtnName) {
			case "A":
			case "B":
			case "X":
			case "Y":
				return InputManagerX.GetViveABXYDown(BtnName);
			case "LB":
			case "Move":
				return SteamVR_Controller.Input(1).GetPressDown(SteamVR_Controller.ButtonMask.Grip);
			case "RB":
			case "Look":
				return SteamVR_Controller.Input(2).GetPressDown(SteamVR_Controller.ButtonMask.Grip);
			case "LS":
				if (!SteamVR_Controller.Input(1).GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).x) > touchSens) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).y) > touchSens) return false;
				return true;
			case "RS":
				if (!SteamVR_Controller.Input(2).GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x) > touchSens) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y) > touchSens) return false;
				return true;
			case "Back":
			case "Cancel":
				return SteamVR_Controller.Input(1).GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
			case "Start":
			case "Submit":
				return SteamVR_Controller.Input(2).GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
			default:
				return false;
			}
		}
        /*
		if (InputManagerX.IsSixenseInputEnabled()) {
			switch (BtnName) {
			case "A":
				return SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.ONE);
			case "B":
				return SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.TWO);
			case "X":
				return SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.THREE);
			case "Y":
				return SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.FOUR);
			case "LB":
			case "Move":
				return SixenseInput.Controllers[0].GetButtonDown(SixenseButtons.BUMPER);
			case "RB":
			case "Look":
				return SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.BUMPER);
			case "LS":
				return SixenseInput.Controllers[0].GetButtonDown(SixenseButtons.JOYSTICK);
			case "RS":
				return SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.JOYSTICK);
			case "Back":
			case "Cancel":
				return SixenseInput.Controllers[0].GetButtonDown(SixenseButtons.START);
			case "Start":
			case "Submit":
				return SixenseInput.Controllers[1].GetButtonDown(SixenseButtons.START);
			default:
				return false;
			}
		}
        */
		return false;
	}

	new public static bool GetButtonUp(string BtnName) {
		if (InputManager.GetButtonUp(BtnName)) return InputManager.GetButtonUp (BtnName);

		if (InputManagerX.IsSteamVRControllerEnabled()) {
			switch (BtnName) {
			case "A":
			case "B":
			case "X":
			case "Y":
				return InputManagerX.GetViveABXYUp(BtnName);
			case "LB":
			case "Move":
				return SteamVR_Controller.Input(1).GetPressUp(SteamVR_Controller.ButtonMask.Grip);
			case "RB":
			case "Look":
				return SteamVR_Controller.Input(2).GetPressUp(SteamVR_Controller.ButtonMask.Grip);
			case "LS":
				if (!SteamVR_Controller.Input(1).GetPressUp(SteamVR_Controller.ButtonMask.Axis0)) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).x) > touchSens) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(1).GetAxis(EVRButtonId.k_EButton_Axis0).y) > touchSens) return false;
				return true;
			case "RS":
				if (!SteamVR_Controller.Input(2).GetPressUp(SteamVR_Controller.ButtonMask.Axis0)) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).x) > touchSens) return false;
				if (Mathf.Abs(SteamVR_Controller.Input(2).GetAxis(EVRButtonId.k_EButton_Axis0).y) > touchSens) return false;
				return true;
			case "Back":
			case "Cancel":
				return SteamVR_Controller.Input(1).GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
			case "Start":
			case "Submit":
				return SteamVR_Controller.Input(2).GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
			default:
				return false;
			}
		}
        /*
		if (InputManagerX.IsSixenseInputEnabled()) {
			switch (BtnName) {
			case "A":
				return SixenseInput.Controllers[1].GetButtonUp(SixenseButtons.ONE);
			case "B":
				return SixenseInput.Controllers[1].GetButtonUp(SixenseButtons.TWO);
			case "X":
				return SixenseInput.Controllers[1].GetButtonUp(SixenseButtons.THREE);
			case "Y":
				return SixenseInput.Controllers[1].GetButtonUp(SixenseButtons.FOUR);
			case "LB":
			case "Move":
				return SixenseInput.Controllers[0].GetButtonUp(SixenseButtons.BUMPER);
			case "RB":
			case "Look":
				return SixenseInput.Controllers[1].GetButtonUp(SixenseButtons.BUMPER);
			case "LS":
				return SixenseInput.Controllers[0].GetButtonUp(SixenseButtons.JOYSTICK);
			case "RS":
				return SixenseInput.Controllers[1].GetButtonUp(SixenseButtons.JOYSTICK);
			case "Back":
			case "Cancel":
				return SixenseInput.Controllers[0].GetButtonUp(SixenseButtons.START);
			case "Start":
			case "Submit":
				return SixenseInput.Controllers[1].GetButtonUp(SixenseButtons.START);
			default:
				return false;
			}
		}
        */      

		return false;
	}
}
