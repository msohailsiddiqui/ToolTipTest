using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    #region singleton class
    private static ReferenceManager instance = null;
    private ReferenceManager() { }

    public static ReferenceManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ReferenceManager();

            return instance;
        }
    }

    #endregion

    public  ToolTipController toolTipControllerRef;
    public  LocalizationManager localizationManagerRef;
    public UIElementsDataManager uiElementsDataManagerRef;

    // Use this for initialization
    void Awake ()
    {
        instance = this;

        if(toolTipControllerRef == null)
        {
            Debug.LogError("ReferenceManager:: toolTipControllerRef is NULL");
        }

        if (localizationManagerRef == null)
        {
            Debug.LogError("ReferenceManager:: localizationManagerRef is NULL");
        }

        if (uiElementsDataManagerRef == null)
        {
            Debug.LogError("ReferenceManager:: uiElementsDataManagerRef is NULL");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
