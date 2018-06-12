using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour 
{

	//************************************************************************************
	// This class will serve as the base class that provides functionality for reading/writing
	// data from/to different sources
	// Different systems/modules will be using this as an interface to read/write data
	// Module authors will write their own specific implementations using the core functions 
	// provided by this class
	// This class/system should provide methods to read/write data from common sources
	// like player preferences, Local Text Formats like JSON or XML, Remote text formats like JSON or XML
	// Other systems that need to read/write data will use these functions and write their own parsers
	// Therefore the basic purpose is to have a base for common methods of data reading/writing
	// 
	//************************************************************************************



	// Define different data sources
	// Define a structure that will contain inputs
	// Define a structure that will contain outputs
	// So that we can have single functions that can behave differently based on implementations

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
