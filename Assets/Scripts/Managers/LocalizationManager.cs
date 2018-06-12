using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocalizationPreset
{
    EN = 0,
    PK 
}

public class LocalizationManager : MonoBehaviour
{
    // The localization preset should also be read form a localization file
    public LocalizationPreset currentLocalizationPreset;

    public void Initialize()
    {
        
    }

    public LocalizationPreset GetCurrentLocalizationPreset()
    {
        return currentLocalizationPreset;
    }

}
