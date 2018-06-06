using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//************************************************************************************
// This class will serve as the base class for controlling tool tip(s, currently it only shows one at a time
// but in some future case it might show more than one)
// The basic idea is to have a generic tool tip class that can be customized and to which 
// we can add functionality later on
// The basic classes needed to make this a generic customizeable class are
// a. The class that acts as an interface between the other modules and the Tool tip system
// b. The class that defines the sturcture of the tool tip data
// c. The class that has some generic animations for showing and hiding tool tips
// 
// It DOES NOT contain the data that will be shown in the tool tip
// It will be called to show some data (might be text(s) or image(s)) when needed
// 
// To start off we want to have support to show a basic tool tip and if the user stays on the element
// It can show a detailed tool tip
// The general states of this class will be as follows:
//
// Initializing              // Error checking and initialization, make sure we have everything we need
// WaitingToBeCalled         // This will be the most common state, in which the Tool Tip is waiting to be invoked
// SetupToolTip              // Once the focus is receieved on an element that can show a tool tip, it notifies the tool tip
//                           // and set ups the tool tip data. 
//                           //This is the only state which the tool tip should go to from WaitingToBeCalled
// WaitingToBeShown          // Since the tool tip is not shown immediately, it goes into the WaitingToBeShown state
//                           //In this state the tool tip waits for the focus to stay on the element for the waitingToBeShownDelay
//                           //If the focus stays on the element for the delay, the tool tip moves onto the SmallTipAppearing state
//                           //Otherwise it goes back to the WaitingToBeCalled state
//
// SmallTipAppearing         // Once the tool tip has the information that it needs, it will use the appearance animation to show
// SmallTipFullyDisplayed    // This is the state in which the tip has appeared completely but has not taken focus away 
//                           // from the element displaying the tooltip
//                           // If the user takes their focus (e.g mouse) away from the element the tool tip will move to the 
//                           // Tip Disappearing state
// DetailedTipAppearing      // If the tool tip is in the small tip fully displayed state and the user keeps the focus on the
//                           // element it will move to the detailed tip appearing state (If a detailed tip is available)
// DetailedTipFullyDisplayed // This is the state in which the tip has appeared completely but has not taken focus away 
//                           // from the element displaying the tooltip
// TipDisappearing           // This is the state in which the tip was being displayed or has been completely displayed
//                           // but has taken focus away from the element hence the tool tip now needs to disappear
// Error                     // If something bad happens we transfer to this state and stay there, so that it is easier to debug
//************************************************************************************


//************************************************************************************
//========
// TODO
// 1. Add support for multiple types of tool tips
// 2. Add support for Images
// 3. Add support for cases where image or detailed summary is not available
// 4. Add support for multiple images
// 5. Add support for different tool tip templates that can be selected/cusotmized from the 
//    element that is calling it
// 6. Add support for different appearance and disappearance animations
// 7. Add Unit Tests
// 8. Tool Tip Obj should not be a public reference, need to find a better way to do this
//========
//************************************************************************************

//************************************************************************************
//========
// TEST CASES
// 1. Check if we have only one instance of the Tool Tip Manager class
// 2. Check if we have some data to show before moving to the showing states
// 3. Validate data to be shown
//  3a. Data should be text or image (Will it be sprite??)
//  3b. Text should not be more than a certain length
//  3c. Image should not be greater than certain resolution
//  3d. 
// 4. Check if we are coming from a valid state
// 5. Make sure we clear tool tip data once the it disappears
// 6. Make sure tool tip stays on screen
// 7. Validate the position of the element on which we are trying to show a tool tip (should be on screen)
// 8. Write Test Cases that try to place the tool tip in different positions (random + controlled)
// 9. Write test cases in which multiple objects try to show the tool tip at once (Should not be possible in the real world)
// 10. Write test cases in which an object tries to show the tool tip multiple times (Show function called multiple times)
// 11. Write test cases in which an object which does not have tool tip data tries to show tool tip
//========
//************************************************************************************





public enum ToolTipState
{
	Initializing = 0,          // Error checking and initialization, make sure we have everything we need
	WaitingToBeCalled,         // This will be the most common state, in which the Tool Tip is waiting to be invoked
	SetupToolTip,              // Once the focus is receieved on an element that can show a tool tip, it notifies the tool tip
	                           // and set ups the tool tip data. 
	                           // This is the only state which the tool tip should go to from WaitingToBeCalled
	WaitingToBeShown,          // Since the tool tip is not shown immediately, it goes into the WaitingToBeShown state
	                           // In this state the tool tip waits for the focus to stay on the element for the waitingToBeShownDelay
	                           // If the focus stays on the element for the delay, the tool tip moves onto the SmallTipAppearing state
	                           // Otherwise it goes back to the WaitingToBeCalled state
	SmallTipAppearing,         // Once the tool tip has the information that it needs, it will use the appearance animation to show
	SmallTipFullyDisplayed,    // This is the state in which the tip has appeared completely but has not taken focus away 
	                           // from the element displaying the tooltip
	                           // If the user takes their focus (e.g mouse) away from the element the tool tip will move to the 
	                           // Tip Disappearing state
	DetailedTipAppearing,      // If the tool tip is in the small tip fully displayed state and the user keeps the focus on the
	                           // element it will move to the detailed tip appearing state (If a detailed tip is available)
	DetailedTipFullyDisplayed, // This is the state in which the tip has appeared completely but has not taken focus away 
	                           // from the element displaying the tooltip
	TipDisappearing,           // This is the state in which the tip was being displayed or has been completely displayed
	                           // but has taken focus away from the element hence the tool tip now needs to disappear
	Error                      // If something bad happens we transfer to this state and stay there, so that it is easier to debug
}

public class ToolTipController : StateImplementor 
{

	#region Events/ Delegates

	public static event Action <ToolTipState> OnToolTipStateChanged;

	#endregion
	#region Member Variables

	// Preferred anchor of the tooltip
	// The ToolTipController will try to position the tool tip as per the specified anchor but the tool tip object will
	// update it if there isn't enough space to display it on this anchor
	// For Example, if the preferred anchor is set to bottom right and the button trying to display the tool tip
	// is located at the bottom right of the screen it will reposition it 
	public ToolTipAnchor preferredToolTipAnchor;

	// The reference to the tool tip object, that will actually show the tool tip
	// This should not be a public reference, need to find a better way to do this
	public ToolTipObj toolTipObj;

	// Need a variable for the initial delay after which tool tip will start appearing
	public float tipAppearingDelay;
	private float tipWaitingStartTime;

	// Need a variable for the time in which the small tool tip will appear
	public float smallTipAppearingDelay;
	private float smallTipAppearStartTime;

	// Need a variable for the time after the detailed tool tip will start appearing 
	// (after the small tool tip has completely displayed)

	// Need a variable for the time in which the detailed tool tip will appear

	// Need a variable for the time in which the tool will disappear
	public float tipDisappearDuration;
	private float tipDisappearStartTime;

	// Need a Dictionary that stores the tool tip data

	// Need a variable that stores the positioning information of the tool tip relative to the element that shows it

	// Need a variable that stores the position of the element on which we are trying to show the tool tip

	// Might need a variable (boolean) that allows resizing of image data

	// Might need a variable that defines the max size possible for image data
	// and will try to resize if the supplied image data is larger than the max size
	// This might be constrained by the screen size

	#endregion

	#region Unity Methods

	protected override void Start () 
	{
		base.Start();
		InitializeStates();
	}

	#endregion


	#region ToolTip States

	State Initializing;
	State WaitingToBeCalled;
	State SetupToolTip;
	State WaitingToBeShown;
	State SmallTipAppearing;
	State SmallTipFullyDisplayed;
	State DetailedTipAppearing;
	State DetailedTipFullyDisplayed;
	State TipDisappearing;

	#region State Management
	public ToolTipState CurrentToolTipState
	{
		get {
			if( CurrentState == null)
			{
				Debug.LogError("ToolTip::CurrentToolTipState is null which should not be so");
				UpdateState( Initializing);

				return ToolTipState.Initializing;
			}

			return (ToolTipState)CurrentState.StateId; 
		}
	}

//	public bool IsWaitingToBeShownGame
//	{
//		get { return currentState == WaitingToBeShown; }
//	}

	void InitializeStates()
	{
	
		Initializing = new State(ToolTipState.Initializing.ToString(), (int)ToolTipState.Initializing);
		Initializing.OnStateBegin = new State.StateBegin(Initializing_Begin);
		Initializing.OnStateEnded = new State.StateEnded(Initializing_End);

		WaitingToBeCalled = new State(ToolTipState.WaitingToBeCalled.ToString(), (int)ToolTipState.WaitingToBeCalled);
		WaitingToBeCalled.OnStateBegin = new State.StateBegin(WaitingToBeCalled_Begin);
		WaitingToBeCalled.Update = new State.StateUpdate(WaitingToBeCalled_Update);
		WaitingToBeCalled.OnStateEnded = new State.StateEnded(WaitingToBeCalled_End);

		SetupToolTip = new State(ToolTipState.SetupToolTip.ToString(), (int)ToolTipState.SetupToolTip);
		SetupToolTip.OnStateBegin = new State.StateBegin(SetupToolTip_Begin);
		SetupToolTip.Update = new State.StateUpdate(SetupToolTip_Update);
		SetupToolTip.OnStateEnded = new State.StateEnded(SetupToolTip_End);

		WaitingToBeShown = new State(ToolTipState.WaitingToBeShown.ToString(), (int)ToolTipState.WaitingToBeShown);
		WaitingToBeShown.OnStateBegin = new State.StateBegin(WaitingToBeShown_Begin);
		WaitingToBeShown.Update = new State.StateUpdate(WaitingToBeShown_Update);
		WaitingToBeShown.OnStateEnded = new State.StateEnded(WaitingToBeShown_End);

		SmallTipAppearing = new State(ToolTipState.SmallTipAppearing.ToString(), (int)ToolTipState.SmallTipAppearing);
		SmallTipAppearing.OnStateBegin = new State.StateBegin(SmallTipAppearing_Begin);
		SmallTipAppearing.Update = new State.StateUpdate(SmallTipAppearing_Update);
		SmallTipAppearing.OnStateEnded = new State.StateEnded(SmallTipAppearing_End);

		SmallTipFullyDisplayed = new State(ToolTipState.SmallTipFullyDisplayed.ToString(), (int)ToolTipState.SmallTipFullyDisplayed);
		SmallTipFullyDisplayed.OnStateBegin = new State.StateBegin(SmallTipFullyDisplayed_Begin);
		SmallTipFullyDisplayed.Update = new State.StateUpdate(SmallTipFullyDisplayed_Update);
		SmallTipFullyDisplayed.OnStateEnded = new State.StateEnded(SmallTipFullyDisplayed_End);

		DetailedTipAppearing = new State(ToolTipState.DetailedTipAppearing.ToString(), (int)ToolTipState.DetailedTipAppearing);
		DetailedTipAppearing.OnStateBegin = new State.StateBegin(DetailedTipAppearing_Begin);
		DetailedTipAppearing.OnStateEnded = new State.StateEnded(DetailedTipAppearing_End);

		DetailedTipFullyDisplayed = new State(ToolTipState.DetailedTipFullyDisplayed.ToString(), (int)ToolTipState.DetailedTipFullyDisplayed);
		DetailedTipFullyDisplayed.OnStateBegin = new State.StateBegin(DetailedTipFullyDisplayed_Begin);
		DetailedTipFullyDisplayed.OnStateEnded = new State.StateEnded(DetailedTipFullyDisplayed_End);

		TipDisappearing = new State(ToolTipState.TipDisappearing.ToString(), (int)ToolTipState.TipDisappearing);
		TipDisappearing.OnStateBegin = new State.StateBegin(TipDisappearing_Begin);
		TipDisappearing.Update = new State.StateUpdate(TipDisappearing_Update);
		TipDisappearing.OnStateEnded = new State.StateEnded(TipDisappearing_End);

		UpdateState(Initializing);
		DebugStates ();
	}

	protected override void UpdateState(State newState)
	{
		base.UpdateState(newState);

		if (newState == CurrentState)
		{
			if (OnToolTipStateChanged != null)
			{
				OnToolTipStateChanged(CurrentToolTipState);
			}
		}
	}

	#endregion

	#region State Functions

	// Initialize the tool tip here
	// Do initialization error checking here
	// Make sure that we have the objects we need
	void Initializing_Begin()
	{
		//Check if we have the Tool Tip object
		if (toolTipObj == null) 
		{
			Debug.LogError ("ToolTipController::Tool Tip Obj is NULL, it should never be NULL");
		}

		// Must set it to inactive ??

		// Must set opacity to 0 ?? - Depends on animation

		// TODO: Need to remove GetComponent Code from here
		toolTipObj.GetComponent<ToolTipObj> ().Initialize ();

		// Once finished initialization, wait for some element to ask for tool tip
		// This waiting is done in the WaitingToBeCalled state

		UpdateState (WaitingToBeCalled);

	}

	bool Initializing_End(State nextState)
	{
		// This state can only transition to the WaitingToBe Called State
		// Therefore it returns true only in that case
		return nextState == WaitingToBeCalled;
	}

	// Hidden Tool Tip State
	//
	void WaitingToBeCalled_Begin()
	{
		DebugStates ();
	}

	void WaitingToBeCalled_Update()
	{
		//UpdateState(WaitingToBeShown);
	}

	bool WaitingToBeCalled_End(State nextState)
	{
		return (nextState == WaitingToBeShown || nextState == SetupToolTip);
	}

	void SetupToolTip_Begin() { }

	void SetupToolTip_Update()
	{
		UpdateState(WaitingToBeCalled);
	}

	bool SetupToolTip_End(State nextState)
	{
		return (nextState == WaitingToBeCalled);
	}

	void WaitingToBeShown_Begin()
	{
		tipWaitingStartTime = Time.time;
	}

	void WaitingToBeShown_Update()
	{
		if (Time.time > tipWaitingStartTime + tipAppearingDelay) 
		{
			UpdateState (SmallTipAppearing);
		}
	}

	bool WaitingToBeShown_End(State nextState)
	{
		return (nextState == SmallTipFullyDisplayed || nextState == SmallTipAppearing || nextState == TipDisappearing);
	}

	// SmallTipAppearing State
	void SmallTipAppearing_Begin()
	{
		smallTipAppearStartTime = Time.time;
	}

	void SmallTipAppearing_Update()
	{
		float scaleVal = Mathf.Lerp (0, 1, ((Time.time - smallTipAppearStartTime) / smallTipAppearingDelay));
		toolTipObj.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
		if (Time.time > smallTipAppearStartTime + smallTipAppearingDelay) 
		{
			UpdateState (SmallTipFullyDisplayed);
		}
	}

	bool SmallTipAppearing_End(State nextState)
	{
		return (nextState == SmallTipFullyDisplayed || nextState == TipDisappearing);
	}

	void SmallTipFullyDisplayed_Begin()
	{
		
	}

	void SmallTipFullyDisplayed_Update()
	{
		

	}

	bool SmallTipFullyDisplayed_End(State nextState)
	{
		DebugStates ();
		return true;
	}

	void DetailedTipAppearing_Begin()
	{

	}

	bool DetailedTipAppearing_End(State nextState)
	{
		return true;
	}

	void DetailedTipFullyDisplayed_Begin()
	{

	}

	bool DetailedTipFullyDisplayed_End(State nextState)
	{
		return true;
	}

	void TipDisappearing_Begin()
	{
		
		tipDisappearStartTime = Time.time;
		Debug.Log ("<color = brown>ToolTipController::Tip is Disappearing: CurrentScale is: " + toolTipObj.transform.localScale.x+"</color>");
	}

	void TipDisappearing_Update()
	{
		// This is needed to handle the case where the appear animation has not completed 
		// but the user has taken away the focus. TheresmallTipAppearingDelayfore it 
		// since currently the scale goes from 0 to 1, the delay can be multiplied by the scale to get the 
		// updated delay
		if (toolTipObj.transform.localScale.x > 0) {
			float startingScale = toolTipObj.transform.localScale.x;
			float tempTipDisappearDuration = startingScale * tipDisappearDuration;
			float scaleVal = Mathf.Lerp (startingScale, 0, ((Time.time - tipDisappearStartTime) / tempTipDisappearDuration));
			toolTipObj.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
			//TODO: This should be dependent on the animation finishing instead of the time
			if (Time.time > tipDisappearStartTime + tempTipDisappearDuration) 
			{
				toolTipObj.ResetToolTip ();
				UpdateState (WaitingToBeCalled);
			}
		}
		else 
		{
			toolTipObj.ResetToolTip ();
			UpdateState (WaitingToBeCalled);
		}
	}

	bool TipDisappearing_End(State nextState)
	{
		return true;
	}

	#endregion

	#endregion

	#region Game State Handlers

//	public void StartGame()
//	{
//		UpdateState(WaitingToBeCalled);
//	}
//
//	public void PlayGame()
//	{
//		UpdateState(WaitingToBeShown);
//	}
//
//	public void EndCurrentGame()
//	{
//		UpdateState(SmallTipAppearing);
//	}

	#endregion

	#region Helper Functions


	#endregion

	#region FunctionsThatOtherModulesWillCall

	//Other modules/elements that can display a tool tip will call this 
	public void ToolTipNeedsToBeShown(Vector2 positionOnScreen, ToolTipData data)
	{
		// TODO: Add a return Status
		// This might return a status based on validation of the data

		// A tool tip can only be setup is it is in the waiting to be called state
		// If it is not in the waiting to be called state, it means that either it has not been 
		// initialized yet (case where the mouse pointer is on a button etc right from when the application was
		// launched
		// or it is already trying to show a tool tip somewhere else (Should not be possible in a system where there is 
		// only one tool tip obj)
		// In both these cases the object that was trying to setup a tool tip should be notified so that it can act accordingly
		// In the case of initialization scenario we might try to queue it 
		// In other scenarios it might be an error (which needs to be handled accordingly)
		// Debug.Log ("ToolTipController::ToolTipNeedsToBeShown:State: "+CurrentState.Name);
		if (CurrentState == WaitingToBeCalled) 
		{
			
			// We were in the right state, lets setup the tooltip
			// First thing that we need to do is to Validate that we have the valid data to show it
			// This means that the position on the screen is valid
			// And we have valid tool tip data
//			if (objOnWhichNeedsToolTip == null) 
//			{
//				Debug.LogError ("ToolTipContorller::Obj that wants to show tool tip is NULL, it should never be NULL");
//				return;
//			}

			if (!toolTipObj.SetupToolTip (positionOnScreen, data, preferredToolTipAnchor)) 
			{
				Debug.LogError ("ToolTipContorller::Invalid Tool Tip Data!");
				return;
			}


			// We need to change the state so that it does not interfere with another call
			UpdateState(WaitingToBeShown);
			DebugStates ();
		}
		else 
		{
			Debug.LogError ("ToolTipController::Trying to show a tool tip when it is not ready, ToolTipController state was: " + CurrentState.Name);
			DebugStates ();
			//Currently we are just showing a log 
			//Need to handle this situation better
		}

	}

	public void RemoveToolTip()
	{
		UpdateState (TipDisappearing);
	}

	#endregion

}
