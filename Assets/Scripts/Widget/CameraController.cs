using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	public float rotateSpeed = 0.05f;
	public float drag = 50;
	private Vector2 lastInputPos;
	private Vector2 inputVelocity = Vector2.zero;
	Vector2 consX;
	Vector2 consY;
	private void Start()
	{
		Transform cached = this.transform;
		consX = new Vector2(cached.localEulerAngles.x - 20, cached.localEulerAngles.x + 20);
		consY = new Vector2(cached.localEulerAngles.y - 10, cached.localEulerAngles.y + 10);
	}

	void Update()
	{
		Vector2 inputDelta = Vector2.zero;

		// Detect if running in Unity Editor or standalone build
#if UNITY_EDITOR || UNITY_STANDALONE
		// Use Mouse Input in Unity Editor or standalone build
		if (Mouse.current.leftButton.isPressed)
		{
			if (lastInputPos == Vector2.zero)
			{
				lastInputPos = Mouse.current.position.ReadValue();
			}
			inputDelta = Mouse.current.position.ReadValue() - lastInputPos;
			lastInputPos = Mouse.current.position.ReadValue();
		}
		else
		{
			lastInputPos = Vector2.zero;
		}
#else
        // Use Touch Input on mobile devices
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (lastInputPos == Vector2.zero)
            {
                lastInputPos = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            inputDelta = Touchscreen.current.primaryTouch.position.ReadValue() - lastInputPos;
            lastInputPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else
        {
            lastInputPos = Vector2.zero;
        }
#endif

		// If there was input this frame, set the input velocity
		if (inputDelta != Vector2.zero)
		{
			inputVelocity = inputDelta;
		}

		// Apply drag to the velocity
		inputVelocity = Vector2.MoveTowards(inputVelocity, Vector2.zero, drag * Time.deltaTime);

		// Rotate the camera based on the velocity
		transform.Rotate(new Vector3(inputVelocity.y, -inputVelocity.x, 0) * rotateSpeed, Space.Self);

		// Lock the Z rotation
		Vector3 eulerAngles = transform.rotation.eulerAngles;

		transform.rotation = Quaternion.Euler(eulerAngles);
		eulerAngles = transform.eulerAngles;
		if (eulerAngles.x > 180)
			eulerAngles.x -= 360;
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, consX.x, consX.y);
		if (eulerAngles.x < 0)
			eulerAngles.x += 360;


		if (eulerAngles.y > 180)
			eulerAngles.y -= 360;
		eulerAngles.y = Mathf.Clamp(eulerAngles.y, consY.x, consY.y);
		if (eulerAngles.y < 0)
			eulerAngles.y += 360;

		eulerAngles.z = 0;
		transform.eulerAngles = eulerAngles;

	}
}