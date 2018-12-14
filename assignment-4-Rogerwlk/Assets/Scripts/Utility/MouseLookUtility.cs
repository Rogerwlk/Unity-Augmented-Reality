using UnityEngine;
using System.Collections;

namespace CornellTech.Util
{
	public class MouseLookUtility : MonoBehaviour
	{

		public enum RotationAxes
		{
			MouseXAndY = 0,
			MouseX = 1,
			MouseY = 2
		}
//		public RotationAxes axes = RotationAxes.MouseXAndY;
		public float sensitivityX = 4f;
		public float sensitivityY = 4f;
		public float minimumX = -360F;
		public float maximumX = 360F;
		public float minimumY = -60F;
		public float maximumY = 60F;
		public float movementSpeed=1f;
		float rotationY = 0F;
		[Header("Enabled")]
		public bool positionEnabled = true;
		public bool rotationEnabled = true;
		public bool enabled=false;
		void Update ()
		{
			if(UnityEngine.Input.GetKeyDown(KeyCode.M)){
				enabled=!enabled;
			}
			if(!enabled)
				return;
			if (rotationEnabled)
			{
				float rotationX = transform.localEulerAngles.y + UnityEngine.Input.GetAxis ("Mouse X") * sensitivityX;

				rotationY += UnityEngine.Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

				transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
			}
			if (positionEnabled)
			{

				float multiplier = .03f*movementSpeed;
				if(UnityEngine.Input.GetKey(KeyCode.LeftShift)||UnityEngine.Input.GetKey(KeyCode.RightShift))
					multiplier*=4f;
				transform.position += transform.forward * UnityEngine.Input.GetAxis ("Vertical") * multiplier;
				transform.position += transform.up * UnityEngine.Input.GetAxis ("Lateral") * multiplier;
				transform.position += transform.right * UnityEngine.Input.GetAxis ("Horizontal") * multiplier;
			}
		}
	
		void Start ()
		{
			// Make the rigid body not change rotation
			if (GetComponent<Rigidbody> ())
				GetComponent<Rigidbody> ().freezeRotation = true;
		}
	}
}
