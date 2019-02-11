using Client.Net;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	public GameObject ConnectionManager;
	private CharacterController controller;
	private Camera _camera;
	
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;

	private bool isRotating = false;
	
	void Awake()
	{
		controller = GetComponent<CharacterController>();
		_camera = Camera.main;
	}

	private Vector3 oldVal;
	bool CheckForUpdate(Vector3 newVal)
	{
		if (oldVal != newVal)
		{
			oldVal = newVal;
			return true;
		}

		return false;
	}
	
	void Update()
	{
		isRotating = Input.GetMouseButtonDown(1);
		
		if( Input.GetMouseButtonDown(0) )
		{
			Ray ray = _camera.ScreenPointToRay( Input.mousePosition );

			if( Physics.Raycast( ray, out var hit, 100 ) )
			{
				Debug.Log( hit.transform.gameObject.name );
			}
		}
		
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			if (controller.isGrounded)
			{

				var horizontalMove = 0f;
				var verticalMove = 0f;
				
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					horizontalMove = -1f;
					//transform.position += Vector3.left * speed * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.RightArrow))
				{
					horizontalMove = 1f;
					//transform.position += Vector3.right * speed * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.UpArrow))
				{
					verticalMove = 1f;
					//transform.position += Vector3.up * speed * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					verticalMove = -1f;
					//transform.position += Vector3.down * speed * Time.deltaTime;
				}
				
				
				moveDirection = new Vector3(horizontalMove, 0, verticalMove);
				moveDirection = transform.TransformDirection(moveDirection);
				moveDirection *= speed;
				if (Input.GetButton("Jump"))
					moveDirection.y = jumpSpeed;
             
			}
			
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
			
			
			if (Input.GetMouseButtonDown(1))
			{
				float mouseInput = Input.GetAxis("Mouse X");
				Vector3 lookhere = new Vector3(0, mouseInput, 0);
				transform.Rotate(lookhere);
			}

//			var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
//			var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
//
//			transform.Rotate(0, x, 0);
//			transform.Translate(0, 0, z);

			if (CheckForUpdate(transform.position))
			{
				//Debug.Log($"X:{transform.position.x} Y:{transform.position.y} Z:{transform.position.z}");
				//var position = new Position(controller.transform.position);
				//EventManager.Publish("SendUnreliable", new BasePacket());
				//_connectionManager.Send(position, MessageType.Movement);
			}
		}
	
	}
}
