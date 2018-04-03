using UnityEngine;
using Rewired;

public class MyInputManager : MonoBehaviour 
{
	public static MyInputManager instance;
	private Player rewiredPlayer; 

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		rewiredPlayer = ReInput.players.GetPlayer (0);
	}

	public Vector2 GetMoveAxis()
	{
		return new Vector2 (rewiredPlayer.GetAxis ("Move Horizontal"), rewiredPlayer.GetAxis ("Move Vertical"));
	}

	public bool IsActionDone(ModelAction i_ModelAction)
	{
		return rewiredPlayer.GetButtonDown (i_ModelAction.controlName);
	}

	public bool IsActionHolded(ModelAction i_ModelAction)
	{
		return rewiredPlayer.GetButton (i_ModelAction.controlName);
	}

	public bool IsAnyActionDone()
	{
		return IsActionDone (ModelActionEnum.A) || IsActionDone (ModelActionEnum.B) || IsActionDone (ModelActionEnum.Y) || IsActionDone (ModelActionEnum.X) || IsActionDone (ModelActionEnum.START);
	}
}