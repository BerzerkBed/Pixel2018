using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelActionEnum {

	static public ModelAction A = CreateModelAction(0 , "A");
	static public ModelAction B = CreateModelAction(1 , "B");
	static public ModelAction Y = CreateModelAction(2 , "Y");
	static public ModelAction X = CreateModelAction (3, "X");
	static public ModelAction START = CreateModelAction(4 , "Start");
	
	static public ModelAction LEFT_SHOULDER = CreateModelAction(5 , "LeftShoulder");
	static public ModelAction RIGHT_SHOULDER = CreateModelAction(6 , "RightShoulder");

	static public ModelAction CreateModelAction(int i_ActionID , string i_ControlName)
	{
		return new ModelAction (i_ActionID, i_ControlName);
	}
}
