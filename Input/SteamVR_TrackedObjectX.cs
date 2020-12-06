using UnityEngine;
using System.Collections;
using Valve.VR;

public class SteamVR_TrackedObjectX : SteamVR_TrackedObject {
	public SteamVR_Utils.RigidTransform pose;
	public bool track = false;

	new private void OnNewPoses(params object[] args)
	{
		if (index == EIndex.None)
			return;

		var i = (int)index;

		isValid = false;
		var poses = (Valve.VR.TrackedDevicePose_t[])args[0];
		if (poses.Length <= i)
			return;

		if (!poses[i].bDeviceIsConnected)
			return;

		if (!poses[i].bPoseIsValid)
			return;

		isValid = true;

		pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

		if (origin != null)
		{
			pose = new SteamVR_Utils.RigidTransform(origin) * pose;
			pose.pos.x *= origin.localScale.x;
			pose.pos.y *= origin.localScale.y;
			pose.pos.z *= origin.localScale.z;
			if (track) {
				transform.position = pose.pos;
				transform.rotation = pose.rot;
			}
		}
		else
		{
			if (track) {
				transform.localPosition = pose.pos;
				transform.localRotation = pose.rot;
			}

		}
	}

	new void OnEnable()
	{
		var render = SteamVR_Render.instance;
		if (render == null)
		{
			enabled = false;
			return;
		}

		SteamVR_Utils.Event.Listen("new_poses", OnNewPoses);
	}

	new void OnDisable()
	{
		SteamVR_Utils.Event.Remove("new_poses", OnNewPoses);
		isValid = false;
	}
}
