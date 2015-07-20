using UnityEngine;
using System.Collections;

public class MouseTouchControls : MonoBehaviour {

	public enum MouseTouchState {
		MOUSE_TOUCH_WAITING,
		MOUSE_TOUCH_TAP,
		MOUSE_TOUCH_BEGAN,
		MOUSE_TOUCH_ENDED,
		MOUSE_TOUCH_LONGPRESS_BEGAN,
		MOUSE_TOUCH_LONGPRESS_ENDED,
		MOUSE_TOUCH_SWIPE_LEFT,
		MOUSE_TOUCH_SWIPE_RIGHT,
		MOUSE_TOUCH_SWIPE_UP,
		MOUSE_TOUCH_SWIPE_DOWN,
		
		DEFAULT
	}

	public MouseTouchState currentMouseTouchState;

	[SerializeField] float touchTimer = 0.0f;
//	[SerializeField] float resetTouchTimer = 0.0f;
	
	[SerializeField] float longPressTime = 0.3f;

	[SerializeField] bool stillTouching = false;
	[SerializeField] bool enableTimer = false;
//	[SerializeField] bool enableResetTimer = false;
	
	//VARIABLES
	//distance calculation
	public MouseTouchState mouseTouchState;			//string to receive touch/mouse output (Debug Only)
	[SerializeField] private float fInitialX;
	[SerializeField] private float fInitialY;
	[SerializeField] private float fFinalX;
	[SerializeField] private float fFinalY;
	private int iTouchStateFlag;						//flag to check 
	[SerializeField] private float touchSensitivity = 100.0f;
	[SerializeField] private float inputX;								//x-coordinate
	[SerializeField] private float inputY;								//y-coordinate
	private float slope;								//slope (m) of the the 
//	private float fDistance;							//magnitude of distance between two positions
	
	void Awake () {
		currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_WAITING;
	}

	private static MouseTouchControls _instance = default(MouseTouchControls);
	
	public static MouseTouchControls Instance {
		get {
			if (!_instance) {
				GameObject touchCtrl = GameObject.Find ("_Core");          
				if (touchCtrl == null) {
					touchCtrl = new GameObject ("_Core");
					DontDestroyOnLoad (touchCtrl);
				}
				
				_instance = touchCtrl.gameObject.GetComponent<MouseTouchControls> ();
				if (!_instance) {
					_instance = touchCtrl.gameObject.AddComponent (typeof(MouseTouchControls)) as MouseTouchControls;
				}
			}
			
			return _instance;
		}
	}

	// Use this for initialization
	void Start ()
	{
		fInitialX = 0.0f;
		fInitialY = 0.0f;
		fFinalX = 0.0f;
		fFinalY = 0.0f;
		
		inputX = 0.0f;
		inputY = 0.0f;
		
		iTouchStateFlag = 0;
		mouseTouchState = MouseTouchState.MOUSE_TOUCH_WAITING;
	}
	
	void Update()
	{
		if ( iTouchStateFlag == 0 )
		{
			mouseTouchState = MouseTouchState.MOUSE_TOUCH_WAITING;
		}

		if (iTouchStateFlag == 0 
		    && Input.GetMouseButtonDown(0))	//state 1 of mouse touch control
		{	
			fInitialX = Input.mousePosition.x;	//get the initial x mouse/ finger value
			fInitialY = Input.mousePosition.y;	//get the initial y mouse/ finger value


			mouseTouchState = MouseTouchState.MOUSE_TOUCH_BEGAN;

			iTouchStateFlag = 1;

			touchTimer = 0.0f;
			enableTimer = true;
		}		

		if (iTouchStateFlag == 1)	//state 2 of mouse touch control
		{
			if (stillTouching)
			{
				stillTouching = false;
				mouseTouchState = MouseTouchState.MOUSE_TOUCH_BEGAN;
				fInitialX = Input.mousePosition.x;
				fInitialY = Input.mousePosition.y;
			} 
			else
			{
				fFinalX = Input.mousePosition.x;
				fFinalY = Input.mousePosition.y;
				if (mouseTouchState == MouseTouchState.MOUSE_TOUCH_ENDED)
					iTouchStateFlag = 2;
			}
		}

		if (iTouchStateFlag == 2 
		    || Input.GetMouseButtonUp(0))	//state 3 of mouse touch control
		{
//			Debug.Log ("fFinalX " + fFinalX + "\nfFinalY " + fFinalY);
			mouseTouchState = CheckGestureState();	//get the gesture direction
//			Debug.Log ("[MouseTouchControls] " + mouseTouchState);
			// call a function to reset the mouseTouchState
			enableTimer = false;
//			resetTouchTimer = 0.0f;

//			enableResetTimer = true;
			iTouchStateFlag = 0;
			stillTouching = false;

			//woot

//			enableResetTimer = false;
//			resetTouchTimer = 0.0f;
			fInitialX = 0;
			fInitialY = 0;
			fFinalX = 0;
			fFinalY = 0;
			inputX = 0;
			inputY = 0;


		}


		if ( enableTimer )
		{
			touchTimer += Time.deltaTime;
//			stillTouching = true;
		}

//		/* for the timers */
//		if (enableTimer)
//		{
////			Debug.Log("inputX = " + inputX + "\ninputY = " + inputY);
//			touchTimer += Time.deltaTime;
////			touchTimer += 0.5f;
//			if ( touchTimer >= longPressTime
//			    && inputX == 0
//			    && inputY == 0)
//			{
//				mouseTouchState = MouseTouchState.MOUSE_TOUCH_LONGPRESS_BEGAN;
////				Debug.Log(mouseTouchState);
//			}
//		}
//
//		if (enableResetTimer) 
//		{
//			resetTouchTimer += Time.deltaTime;
////			resetTouchTimer += 0.5f;
//			
//			if (resetTouchTimer >= 0.0175f) {
////			if (resetTouchTimer >= 2.5f) {
//				mouseTouchState = MouseTouchState.MOUSE_TOUCH_WAITING;
//				enableResetTimer = false;
//				resetTouchTimer = 0.0f;
//				fInitialX = 0;
//				fInitialY = 0;
//				fFinalX = 0;
//				fFinalY = 0;
//				inputX = 0;
//				inputY = 0;
//			}
//		}
	}

	/*
	 *	FUNCTION: Calculate the swipe direction
	 */
	// issues with the long press not updating the touch state
	private MouseTouchState CheckGestureState()
	{
		//calculate the slope of the swipe
		inputX = fInitialX - fFinalX;
		inputY = fInitialY - fFinalY;
		slope = inputY / inputX;
//		Debug.Log ("[gestureState] inputX = " + inputX + "\ninputY = " + inputY + "\nslope + " + slope);
		//calculate the distance of tap start and end
//		fDistance = Mathf.Sqrt( Mathf.Pow((fFinalY-fInitialY), 2) + Mathf.Pow((fFinalX-fInitialX), 2) );
		if (inputX >= 0 
		    && inputY > 100
		    && slope > 1)//first octant MOUSE_TOUCH_SWIPE_DOWN
		{		
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_DOWN;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_DOWN;
		}
		else if (inputX <= 0 
		         && inputY > 100
		         && slope < -1)//eighth octant MOUSE_TOUCH_SWIPE_DOWN
		{
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_DOWN;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_DOWN;
		}
		else if (inputX > touchSensitivity
		         && inputY >= 0 
		         && slope < 1 
		         && slope >= 0)//second octant MOUSE_TOUCH_SWIPE_LEFT
		{
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_LEFT;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_LEFT;
		}
		else if (inputX > touchSensitivity
		         && inputY <= 0 
		         && slope > -1 
		         && slope <= 0)//third octant MOUSE_TOUCH_SWIPE_LEFT
		{
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_LEFT;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_LEFT;
		}
		else if (inputX < -touchSensitivity
		         && inputY >= 0 
		         && slope > -1 
		         && slope <= 0)//seventh octant MOUSE_TOUCH_SWIPE_RIGHT
		{
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_RIGHT;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_RIGHT;
		}
		else if (inputX < -touchSensitivity
		         && inputY <= 0 
		         && slope >= 0 
		         && slope < 1)//sixth octant  MOUSE_TOUCH_SWIPE_RIGHT
		{
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_RIGHT;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_RIGHT;
		}
		else if (inputX >= 0 
		         && inputY < -touchSensitivity
		         && slope < -1)//fourth octant MOUSE_TOUCH_SWIPE_UP
		{
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_UP;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_UP;
		}
		else if (inputX <= 0 
		         && inputY < -touchSensitivity
		         && slope > 1)//fifth octant MOUSE_TOUCH_SWIPE_UP
		{
			currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_SWIPE_UP;
			return MouseTouchState.MOUSE_TOUCH_SWIPE_UP;
		}
		else //if (inputX == 0
		         //&& inputY == 0)
		{
			if (touchTimer >= longPressTime) {
				currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_LONGPRESS_ENDED;
				return MouseTouchState.MOUSE_TOUCH_LONGPRESS_ENDED;
			} else {
				currentMouseTouchState = MouseTouchState.MOUSE_TOUCH_TAP;
				return MouseTouchState.MOUSE_TOUCH_TAP;
			}
		}
//		return MouseTouchState.MOUSE_TOUCH_WAITING;
	}//end of gestureState function
	
	/*
	*	FUNCTION: Return gesture state.
	*	RETURNS: Returns MOUSE_TOUCH_WAITING if no gestures are detected.
	*			  Returns mouseTouchState if a valid gesture is detected
	*/
	public MouseTouchState GetGestureState()
	{
//		Debug.Log("[getGestureState] called!");
		if (mouseTouchState != MouseTouchState.MOUSE_TOUCH_WAITING)
		{
			var eTempGestureState = mouseTouchState;
			mouseTouchState = MouseTouchState.MOUSE_TOUCH_WAITING;
			
			return eTempGestureState;
		}
		else
			return MouseTouchState.MOUSE_TOUCH_WAITING;
	}

	public MouseTouchState ReturnMouseTouchState() {
		return mouseTouchState;
	}

	public void ResetMouseTouchState () {
//		Debug.Log("ResetMouseTouchState");
		stillTouching = true;
		mouseTouchState = MouseTouchState.MOUSE_TOUCH_WAITING;
		touchTimer = 0.0f;
		iTouchStateFlag = 1;
	}
}