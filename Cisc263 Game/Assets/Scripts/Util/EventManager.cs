using UnityEngine.Events;

// Container class. Holds event classes and event instances for other scripts to invoke/listen for.
public class EventManager : Singleton<EventManager> 
{ 	
	[System.Serializable] public class EventJumpScare : UnityEvent { }
	//[System.Serializable] public class EventGameStateChange : UnityEvent<GameManager.gameState, GameManager.gameState> { }
	[System.Serializable] public class EventLevelCompleted : UnityEvent { }
	[System.Serializable] public class EventEnemyStateChange : UnityEvent<EnemyAI.State> { }
	[System.Serializable] public class EventPlayerSeen : UnityEvent { }
	[System.Serializable] public class EventDeath : UnityEvent { }
	[System.Serializable] public class EventPlayerNoise : UnityEvent<float> { }
	//[System.Serializable] public class EventPowerUp : UnityEvent<powerUp> { }


	public EventJumpScare JumpScare;
	//public EventGameStateChange GameStateChange;
	public EventLevelCompleted LevelCompleted;
	public EventEnemyStateChange EnemyStateChange;
	public EventPlayerSeen PlayerSeen;
	public EventDeath Death;
	public EventPlayerNoise PlayerNoise;
	//public EventPowerUp PowerUp;
}
