using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void DispatchGameplayStartEvent()
	{
		CodeControl.Message.Send(new GameplayStartEvent());
	}

	public void DispatchNextWaveStartRequestEvent()
	{
		CodeControl.Message.Send(new NextWaveStartRequestEvent());
	}
}
