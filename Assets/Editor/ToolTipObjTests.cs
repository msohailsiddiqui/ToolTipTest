using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ToolTipObjTests {

	[Test]
	//Basic Unit Test to check if a Tool Tip has the minimum required components to display a tool tip
	//i.e. A Tool tip small text and a tool tip bg
	public void ToolTipObjHasMiniumRequiredComponentsToShowToolTip() 
	{
		
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator ToolTipObjTestsWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
