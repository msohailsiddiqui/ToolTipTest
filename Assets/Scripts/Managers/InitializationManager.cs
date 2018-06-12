using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        // All initialization should be managed here, so that we have control over the sequence
        ReferenceManager.Instance.localizationManagerRef.Initialize();
        ReferenceManager.Instance.toolTipControllerRef.Initialize();
        ReferenceManager.Instance.uiElementsDataManagerRef.Initialize();
    }
	

}
