using System.Xml;
using System.Collections.Generic;
using UnityEngine;

public class XMLDataHandler
{
    private string path;
    private string fileInfo;
    private XmlDocument xmlDoc;
    private WWW www;
    private TextAsset textXml;
    private string fileName;

    // load xml file from resouces folder under Assets
    public void loadXMLFromResources(string _filename, string subfolders)
    {
        fileName = _filename;
        xmlDoc = new XmlDocument();
        if (System.IO.File.Exists(getPath()))
        {
            Debug.Log("XMLDataHandler::loadXMLFromResources from path: " + getPath() + subfolders);
            xmlDoc.LoadXml(System.IO.File.ReadAllText(getPath()+subfolders));
        }
        else
        {
            Debug.Log("XMLDataHandler::loadXMLFromResources from Resouces: " + subfolders + fileName);
            textXml = (TextAsset)Resources.Load(subfolders+fileName, typeof(TextAsset));
            if(textXml != null)
            {
                Debug.Log("XMLDataHandler::loadXMLFromResources textXml: " + textXml);
                xmlDoc.LoadXml(textXml.text);
            }
            else
            {
                Debug.LogError("XMLDataHandler::loadXMLFromResources could not find specified file: " + subfolders + fileName);

            }

        }
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + fileName;
#elif UNITY_ANDROID
		return Application.persistentDataPath+fileName;
#elif UNITY_IPHONE
		return GetiPhoneDocumentsPath()+"/"+fileName;
#else
		return Application.dataPath +"/"+ fileName;
#endif
    }
    private string GetiPhoneDocumentsPath()
    {
        // Strip "/Data" from path 
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
        // Strip application name 
        path = path.Substring(0, path.LastIndexOf('/'));
        return path + "/Documents";
    }

    public XmlDocument GetXMLDoc()
    {
        return xmlDoc;
    }
}
