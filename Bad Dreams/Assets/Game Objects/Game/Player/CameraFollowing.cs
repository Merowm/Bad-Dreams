using UnityEngine;
using System.Collections;

public class CameraFollowing : MonoBehaviour
{
	GameObject cameraObj;
	Transform camMinT;
	Transform camMaxT;

	void Start()
	{
		cameraObj = GameObject.Find("Main Camera");

		camMinT = GameObject.Find("Camera Limit Min").transform;
		camMaxT = GameObject.Find("Camera Limit Max").transform;
	}

	public void UpdateCameraPosition()
	{
		if (cameraObj != null)
		{
			//follow
			cameraObj.transform.position = transform.position;

			//limit
			Vector3 min = camMinT.position;
			Vector3 max = camMaxT.position;

			Vector3 pos = cameraObj.transform.position;
			Vector3 newPos = new Vector3(Mathf.Clamp(pos.x, min.x, max.x), Mathf.Clamp(pos.y, min.y, max.y), -10.0f);

			cameraObj.transform.position = newPos;
		}
	}
}
