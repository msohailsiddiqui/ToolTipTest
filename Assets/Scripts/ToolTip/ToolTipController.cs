﻿using System.Collections;
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
// WaitingToBeHidden         // When ever a tool tip needs to be hidden (UI element has lost focus), we dont hide it immediately
//                           // Instead we wait for a delay after which the tool tip will be hidden. This also works well for the scenario
//                           // Where a new tool tip needs to be shown immediately
// ShiftToolTip              // This is used when a tool tip is already displayed on an UI element and user quickly shifts to another element
//                           // This is a special scenario in which we don't show the appearing animation and show the small description fully
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


//************************************************************************************
//========
// Observed Behavior
// 1. Tool tip is shown below the mouse pointer and is centered at the mouse X pos
// 2. once the tool tip is displayed, the mouse can keep moving over the ui element but tool tip stays in the same position
// 3. If tool tip is displayed and mouse moves to another object with tool tip, it displays the tool tip for that object
// 4. Tool tip background has an outline
// 5. 
//========
//************************************************************************************



public enum ToolTipControllerState
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
    WaitingToBeHidden,         // When ever a tool tip needs to be hidden (UI element has lost focus), we dont hide it immediately
                               // Instead we wait for a delay after which the tool tip will be hidden. This also works well for the scenario
                               // Where a new tool tip needs to be shown immediately
	TipDisappearing,           // This is the state in which the tip was being displayed or has been completely displayed
	                           // but has taken focus away from the element hence the tool tip now needs to disappear
    ShiftToolTip,              // This is used when a tool tip is already displayed on an UI element and user quickly shifts to another element
                               // This is a special scenario in which we don't show the appearing animation and show the small description fully
	Error                      // If something bad happens we transfer to this state and stay there, so that it is easier to debug
}

public class ToolTipController : StateImplementor 
{

	#region Events/ Delegates

	public static event Action <ToolTipControllerState> OnToolTipStateChanged;

	#endregion
	#region Member Variables

	// Preferred placement of the tooltip
	// The ToolTipController will try to position the tool tip as per the specified placement but the tool tip object will
	// update it if there isn't enough space to display it on this anchor
	// For Example, if the preferred placement is set to bottom right and the button trying to display the tool tip
	// is located at the bottom right of the screen it will reposition it 
	public ToolTipPlacementData preferredToolTipPlacement;

	// The reference to the tool tip object, that will actually show the tool tip
	// This should not be a public reference, need to find a better way to do this
	public GameObject toolTipPrefab;
	public GameObject canvasObj;
    public GameObject animationHelperPrefab;
    private ToolTipObj toolTipObj;
    private AnimationHelper animationHelper;

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

    // Variable that stores the offset we need to add to the mouse position
    // The mouse cursor image is shown at the mouse position and it is pivoted at the top left
    // this is need so that the tool tip does not overalp the mouse cursor image
    // The default mouse cursor size in Unity is 32x32, this can be updated for custom cursors
    public float mouseCursorOffsetX;
    public float mouseCursorOffsetY;

    // Need a variable for the delay after which tool tip will start disappearing
    public float tipDisappearingDelay;
    private float tipDisappearWaitingStartTime;

    // Delay in which we shift the tool tip and get the new mouse coords
    public float shiftToolTipDelay;
    private float shiftToolTipStartTime;
    private ToolTipData dataToUseForShift;

    // stores the animation ID of the appear animation, so that it can be stopped when needed
    // In the case where tool tip disappears before appearing completely
    private float appearAnimID;
    private float disappearAnimID;

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
		
	}

	#endregion


	#region ToolTipController States

	State Initializing;
	State WaitingToBeCalled;
	State SetupToolTip;
    State WaitingToBeShown;
    State SmallTipAppearing;
    State SmallTipFullyDisplayed;
    State DetailedTipAppearing;
    State DetailedTipFullyDisplayed;
    State WaitingToBeHidden;
    State TipDisappearing;
    State ShiftToolTip;

    #endregion
    #region State Management
    public ToolTipControllerState CurrentToolTipControllerState
	{
		get {
			if( CurrentState == null)
			{
				Debug.LogError("ToolTip::CurrentToolTipState is null which should not be so");
				UpdateState( Initializing);

				return ToolTipControllerState.Initializing;
			}

			return (ToolTipControllerState)CurrentState.StateId; 
		}
	}

	void InitializeStates()
	{
		Initializing = new State(ToolTipControllerState.Initializing.ToString(), (int)ToolTipControllerState.Initializing);
		Initializing.OnStateBegin = new State.StateBegin(Initializing_Begin);
		Initializing.OnStateEnded = new State.StateEnded(Initializing_End);

		WaitingToBeCalled = new State(ToolTipControllerState.WaitingToBeCalled.ToString(), (int)ToolTipControllerState.WaitingToBeCalled);
		WaitingToBeCalled.OnStateBegin = new State.StateBegin(WaitingToBeCalled_Begin);
		WaitingToBeCalled.Update = new State.StateUpdate(WaitingToBeCalled_Update);
		WaitingToBeCalled.OnStateEnded = new State.StateEnded(WaitingToBeCalled_End);

		SetupToolTip = new State(ToolTipControllerState.SetupToolTip.ToString(), (int)ToolTipControllerState.SetupToolTip);
		SetupToolTip.OnStateBegin = new State.StateBegin(SetupToolTip_Begin);
		SetupToolTip.Update = new State.StateUpdate(SetupToolTip_Update);
		SetupToolTip.OnStateEnded = new State.StateEnded(SetupToolTip_End);

		WaitingToBeShown = new State(ToolTipControllerState.WaitingToBeShown.ToString(), (int)ToolTipControllerState.WaitingToBeShown);
		WaitingToBeShown.OnStateBegin = new State.StateBegin(WaitingToBeShown_Begin);
		WaitingToBeShown.Update = new State.StateUpdate(WaitingToBeShown_Update);
		WaitingToBeShown.OnStateEnded = new State.StateEnded(WaitingToBeShown_End);

		SmallTipAppearing = new State(ToolTipControllerState.SmallTipAppearing.ToString(), (int)ToolTipControllerState.SmallTipAppearing);
		SmallTipAppearing.OnStateBegin = new State.StateBegin(SmallTipAppearing_Begin);
		SmallTipAppearing.Update = new State.StateUpdate(SmallTipAppearing_Update);
		SmallTipAppearing.OnStateEnded = new State.StateEnded(SmallTipAppearing_End);

		SmallTipFullyDisplayed = new State(ToolTipControllerState.SmallTipFullyDisplayed.ToString(), (int)ToolTipControllerState.SmallTipFullyDisplayed);
		SmallTipFullyDisplayed.OnStateBegin = new State.StateBegin(SmallTipFullyDisplayed_Begin);
		SmallTipFullyDisplayed.Update = new State.StateUpdate(SmallTipFullyDisplayed_Update);
		SmallTipFullyDisplayed.OnStateEnded = new State.StateEnded(SmallTipFullyDisplayed_End);

		DetailedTipAppearing = new State(ToolTipControllerState.DetailedTipAppearing.ToString(), (int)ToolTipControllerState.DetailedTipAppearing);
		DetailedTipAppearing.OnStateBegin = new State.StateBegin(DetailedTipAppearing_Begin);
		DetailedTipAppearing.OnStateEnded = new State.StateEnded(DetailedTipAppearing_End);

		DetailedTipFullyDisplayed = new State(ToolTipControllerState.DetailedTipFullyDisplayed.ToString(), (int)ToolTipControllerState.DetailedTipFullyDisplayed);
		DetailedTipFullyDisplayed.OnStateBegin = new State.StateBegin(DetailedTipFullyDisplayed_Begin);
		DetailedTipFullyDisplayed.OnStateEnded = new State.StateEnded(DetailedTipFullyDisplayed_End);

        WaitingToBeHidden = new State(ToolTipControllerState.WaitingToBeHidden.ToString(), (int)ToolTipControllerState.WaitingToBeHidden);
        WaitingToBeHidden.OnStateBegin = new State.StateBegin(WaitingToBeHidden_Begin);
        WaitingToBeHidden.Update = new State.StateUpdate(WaitingToBeHidden_Update);
        WaitingToBeHidden.OnStateEnded = new State.StateEnded(WaitingToBeHidden_End);

        TipDisappearing = new State(ToolTipControllerState.TipDisappearing.ToString(), (int)ToolTipControllerState.TipDisappearing);
		TipDisappearing.OnStateBegin = new State.StateBegin(TipDisappearing_Begin);
		TipDisappearing.Update = new State.StateUpdate(TipDisappearing_Update);
		TipDisappearing.OnStateEnded = new State.StateEnded(TipDisappearing_End);

        ShiftToolTip = new State(ToolTipControllerState.ShiftToolTip.ToString(), (int)ToolTipControllerState.ShiftToolTip);
        ShiftToolTip.OnStateBegin = new State.StateBegin(ShiftToolTip_Begin);
        ShiftToolTip.Update = new State.StateUpdate(ShiftToolTip_Update);
        ShiftToolTip.OnStateEnded = new State.StateEnded(ShiftToolTip_End);

        GameObject temp = Instantiate (toolTipPrefab);
		if (temp != null) 
		{
			toolTipObj = temp.GetComponent<ToolTipObj> ();
			if (canvasObj != null && toolTipObj != null) 
			{
				toolTipObj.Initialize ();
                toolTipObj.GetRectTransform ().SetParent (canvasObj.transform);
			}
		}

        temp = Instantiate(animationHelperPrefab);
        if (temp != null)
        {
            animationHelper = temp.GetComponent<AnimationHelper>();
        }


            UpdateState(Initializing);
		//DebugStates ();
	}

	protected override void UpdateState(State newState)
	{
		base.UpdateState(newState);

		if (newState == CurrentState)
		{
			if (OnToolTipStateChanged != null)
			{
				OnToolTipStateChanged(CurrentToolTipControllerState);
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

		toolTipObj.Initialize ();

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
		//DebugStates ();
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
        //Debug.Log("ToolTipController::SmallTipAppearing_Begin: Mouse Position: " + Input.mousePosition);
        Vector2 posForToolTip = new Vector2(Input.mousePosition.x + mouseCursorOffsetX,
                                            Input.mousePosition.y - mouseCursorOffsetY );

        toolTipObj.ShowToolTip(posForToolTip);
		smallTipAppearStartTime = Time.time;

        if(animationHelper != null)
            appearAnimID = animationHelper.AnimateScale(toolTipObj.transform, 0, 1, smallTipAppearingDelay, EasingType.SmootherLerp, AppearAnimationComplete);
    }

	void SmallTipAppearing_Update()
	{
        //float scaleVal = Mathf.Lerp (0, 1, ((Time.time - smallTipAppearStartTime) / smallTipAppearingDelay));
        //float scaleVal = Easing.SmootherStepF(0, 1, ((Time.time - smallTipAppearStartTime) / smallTipAppearingDelay));

        //toolTipObj.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
        

        //if (Time.time > smallTipAppearStartTime + smallTipAppearingDelay) 
		//{
			
		//}
	}

	bool SmallTipAppearing_End(State nextState)
	{
        if(nextState != SmallTipFullyDisplayed)
        {
            //The Appearing animation did not get the chance to complete
            // We Need to Stop the animation (cancel animation Coroutine)
            animationHelper.StopAnimation(appearAnimID);
        }
		return (nextState == SmallTipFullyDisplayed || nextState == WaitingToBeHidden);
	}

	void SmallTipFullyDisplayed_Begin()
	{
		
	}

	void SmallTipFullyDisplayed_Update()
	{
		

	}

	bool SmallTipFullyDisplayed_End(State nextState)
	{
		//DebugStates ();
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

    void WaitingToBeHidden_Begin()
    {
        tipDisappearWaitingStartTime = Time.time;
    }

    void WaitingToBeHidden_Update()
    {
        if (Time.time > tipDisappearWaitingStartTime + tipDisappearingDelay)
        {
            UpdateState(TipDisappearing);
        }
    }

    bool WaitingToBeHidden_End(State nextState)
    {
        return true;
    }

    void TipDisappearing_Begin()
	{
        tipDisappearStartTime = Time.time;
		//Debug.Log ("<color=brown>ToolTipController::Tip is Disappearing: CurrentScale is: " + toolTipObj.transform.localScale.x+"</color>");
        float startingScale = toolTipObj.transform.localScale.x;
        if (animationHelper != null)
            disappearAnimID = animationHelper.AnimateScale(toolTipObj.transform, startingScale, 0, startingScale * tipDisappearDuration, EasingType.SmootherLerp, DisappearAnimationComplete);
    }

	void TipDisappearing_Update()
	{
		// This is needed to handle the case where the appear animation has not completed 
		// but the user has taken away the focus. TheresmallTipAppearingDelayfore it 
		// since currently the scale goes from 0 to 1, the delay can be multiplied by the scale to get the 
		// updated delay
		//if (toolTipObj.transform.localScale.x > 0)
		//{
		//	float startingScale = toolTipObj.transform.localScale.x;
		//	float tempTipDisappearDuration = startingScale * tipDisappearDuration;
		//	//float scaleVal = Mathf.Lerp (startingScale, 0, ((Time.time - tipDisappearStartTime) / tempTipDisappearDuration));
		//	//toolTipObj.transform.localScale = new Vector3 (scaleVal, scaleVal, scaleVal);
		//	//TODO: This should be dependent on the animation finishing instead of the time
		//	if (Time.time > tipDisappearStartTime + tempTipDisappearDuration) 
		//	{
		//		toolTipObj.ResetToolTip ();
		//		UpdateState (WaitingToBeCalled);
		//	}
		//}
		//else 
		//{
		//	toolTipObj.ResetToolTip ();
		//	UpdateState (WaitingToBeCalled);
		//}
	}

	bool TipDisappearing_End(State nextState)
	{
        return true;
	}

    void ShiftToolTip_Begin()
    {
        shiftToolTipStartTime = Time.time;
    }

    void ShiftToolTip_Update()
    {
        if(Time.time > shiftToolTipStartTime + shiftToolTipDelay)
        {
           

            if(toolTipObj.SetupToolTip(dataToUseForShift, null))
            {
                //Get the new mouse pos 
                Debug.Log("<color=blue>ToolTipController::From ShiftToolTip, updating state to SmallTipFullyDisplayed</color>");
                Vector2 posForToolTip = new Vector2(Input.mousePosition.x + mouseCursorOffsetX,
                                               Input.mousePosition.y - mouseCursorOffsetY);
                toolTipObj.ShowToolTip(posForToolTip);
                toolTipObj.GetRectTransform().localScale = Vector3.one;
                UpdateState(SmallTipFullyDisplayed);
            }
            else
            {
                Debug.Log("<color=blue>ToolTipController::From ShiftToolTip, updating state to WaitingToBeCalled</color>");
                UpdateState(WaitingToBeCalled);
            }
            
        }

    }

    bool ShiftToolTip_End(State nextState)
    {
        return true;
    }

    #endregion


    #region Helper Functions


    #endregion

    #region FunctionsThatOtherModulesWillCall

    //Other modules/elements that can display a tool tip will call this 
    public void ToolTipNeedsToBeShown(string uiElementID)
    {
        ToolTipData dataForUIElement;
        dataForUIElement = ReferenceManager.Instance.uiElementsDataManagerRef.GetToolTipForUIElement(uiElementID);
        if(dataForUIElement != null && dataForUIElement.HasElement(ToolTipElementID.SmallToolTipText))
        {
            ToolTipNeedsToBeShown(dataForUIElement);
        }
    }
    public void ToolTipNeedsToBeShown(ToolTipData data)
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

            
			if (!toolTipObj.SetupToolTip ( data, preferredToolTipPlacement)) 
			{
				Debug.LogError ("ToolTipContorller::Invalid Tool Tip Data!");
				return;
			}


			// We need to change the state so that it does not interfere with another call
			UpdateState(WaitingToBeShown);
			//DebugStates ();
		}
		else 
		{
            if (CurrentState == WaitingToBeShown || CurrentState == SmallTipAppearing || CurrentState == SmallTipFullyDisplayed
                || CurrentState == DetailedTipAppearing || CurrentState == DetailedTipFullyDisplayed)
            {
                Debug.Log("ShiftingToolTip: " + data);
                foreach (KeyValuePair<ToolTipElementID, string> entry in data.toolTipElementsDict)
                {
                    Debug.Log("data.Key: "+ entry.Key);
                    //toolTipElementsDict.Add(entry.Key, entry.Value);
                }
                dataToUseForShift = data;
                UpdateState(ShiftToolTip);

            }
            else if (CurrentState == WaitingToBeHidden || CurrentState == TipDisappearing) 
			{
				// This happens when the user quickly shifts from one UI element to another (or the same)
				// and the disappearing duration is long 
				Debug.Log ("<color=blue>ToolTipController::Trying to show a tool tip while it was disappearing: "+CurrentState.Name+"</color>");
                animationHelper.StopAnimation(disappearAnimID);
                //if (!toolTipObj.SetupToolTip(data, preferredToolTipPlacement))
                //{
                //    Debug.LogError("ToolTipContorller::Invalid Tool Tip Data!");
                //    return;
                //}


                // We need to change the state so that it does not interfere with another call
                //UpdateState(SmallTipFullyDisplayed);
                dataToUseForShift = data;
                UpdateState(ShiftToolTip);
            } 
			else 
			{
				// Something unexpected happened. This should not be the case
				// Currently we are just showing a log. Need to handle this situation better
				Debug.LogError ("ToolTipController::Trying to show a tool tip when it is not ready, ToolTipController state was: " + CurrentState.Name);
				DebugStates ();
			}

		}

	}

	public void RemoveToolTip()
	{
		UpdateState (WaitingToBeHidden);
	}

    public void AppearAnimationComplete()
    {
        //Debug.Log("ToolTipController::AppearAnimationComplete");
        UpdateState(SmallTipFullyDisplayed);
        //DebugStates();
    }

    public void DisappearAnimationComplete()
    {
        //Debug.Log("ToolTipController::DisappearAnimationComplete");
        toolTipObj.ResetToolTip();
        UpdateState (WaitingToBeCalled);
        //DebugStates();
    }

    public void Initialize()
    {
        base.Start();
        InitializeStates();
    }

    #endregion

}
