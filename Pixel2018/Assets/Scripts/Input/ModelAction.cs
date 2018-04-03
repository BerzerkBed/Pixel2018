using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAction 
{
	public int actionID;
	public string controlName;

	public ModelAction(int i_ActionID , string i_ControlName)
	{
		actionID = i_ActionID;
		controlName = i_ControlName;
	}
}