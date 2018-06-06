using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateImplementor : MonoBehaviour 
{

	// These were previously public, made these private since we do not want people to be able to change these 
	// without performing some error checking on the state transition. 
	// All state changes should be done via the update state function

	private State _currentState;
	private State _prevState;
	private List<State> listOfStates;
	public State CurrentState{
		get { return _currentState;}
	}


	protected virtual void Awake()
	{
		listOfStates = new List<State> ();
	}

	// Use this for initialization
	protected virtual void Start()
	{
		SetupStateHandlers();
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		if (_currentState != null &&
			_currentState.Update != null)
		{
			_currentState.Update();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (_currentState != null &&
			_currentState.FixedUpdate != null)
		{
			_currentState.FixedUpdate();
		}
	}

	protected virtual void LateUpdate()
	{
		if (_currentState != null &&
			_currentState.LateUpdate != null)
		{
			_currentState.LateUpdate();
		}
	}

	protected virtual void UpdateState(State newState)
	{
		// Switch case for Previous State handling
		if (!PrevStateEnd(newState))
		{
			Debug.Log("---------------------Not going to update state::"+newState.Name+"::CurrentState::"+_currentState.Name);
			// return if prev state wants to not continue further
			return;
		}

		//First check if this is a valid state to transit to
		if (!CanTransitTo (newState)) 
		{
			return;
		}

		// Update state variables
		_prevState = _currentState;
		listOfStates.Add (newState);
		_currentState = newState;

		CurrStateBegin(newState);
	}

	protected virtual bool PrevStateEnd(State newState)
	{
		bool retVal = true;
		if (_currentState != null && _currentState.OnStateEnded != null)
			retVal = _currentState.OnStateEnded(newState);
		return retVal;
	}

	protected virtual void CurrStateBegin(State newState)
	{
		if (_currentState != null && _currentState.OnStateBegin != null)
			_currentState.OnStateBegin();
	}

	protected virtual void SetupStateHandlers()
	{

	}

	protected virtual void UpdateToPrevState()
	{
		UpdateState(_prevState);
	}

	//***************************
	// This is the primary function where we will be checking our FSM rules
	// This funciton should be overloaded by each class to add their own rules
	// TODO: Implement a rule system
	//***************************

	protected virtual bool CanTransitTo(State stateToTransitTo)
	{
		// For now we will return true since only the implementing class knows what the rules are
		// Later this will include a rule system with a list of Next Possible States that we will be checking here
		return true;	
	}

	//***************************
	// This is the primary function where provide debug information for the states
	//***************************

	protected virtual void DebugStates()
	{
		Debug.Log ("<color=magenta>Outputting States for: " + gameObject.name + "</color>");
		int stateIndex = 1;
		foreach (State state in listOfStates) 
		{
			Debug.Log ("<color=magenta>"+stateIndex+". "+state.Name + "</color>");
			stateIndex++;
		}	
	}

}
