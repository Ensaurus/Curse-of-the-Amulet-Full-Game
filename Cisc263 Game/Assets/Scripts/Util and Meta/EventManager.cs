using UnityEngine.Events;
using UnityEngine;

// Container class. Holds event classes and event instances for other scripts to invoke/listen for.
public class EventManager : Singleton<EventManager>
{ 	
	[System.Serializable] public class EventJumpScare : UnityEvent { }	
	[System.Serializable] public class EventGameStateChange : UnityEvent<GameManager.GameState, GameManager.GameState> { }	
	[System.Serializable] public class EventLevelCompleted : UnityEvent { }	
	[System.Serializable] public class EventEnemyStateChange : UnityEvent<EnemyAI.State> { }	
	[System.Serializable] public class EventPlayerSeen : UnityEvent<EnemyAI> { }	
	[System.Serializable] public class EventDeath : UnityEvent { }	
	[System.Serializable] public class EventPlayerNoise : UnityEvent<float> { }     
	[System.Serializable] public class EventPowerUp : UnityEvent<GameObject> { }		
	[System.Serializable] public class EventFadeComplete : UnityEvent { }
	[System.Serializable] public class EventItemSwap : UnityEvent<Item> { }
	[System.Serializable] public class EventItemUsed : UnityEvent<Item> { }
	[System.Serializable] public class EventItemIncrease : UnityEvent<Item> { }
	[System.Serializable] public class EventFailedPortalEntry : UnityEvent<int> { }


	public EventJumpScare JumpScare; // called whenever a jumpscare should appear
	public EventGameStateChange GameStateChange; // called when game state changed <previous, new>
	public EventLevelCompleted LevelCompleted;  // called when level completed
	public EventEnemyStateChange EnemyStateChange;  // called when an enemy changes states <new state>
	public EventPlayerSeen PlayerSeen;  // called when an enemy sees the player <the EnemyAI script corresponding to the enemy that saw them>
	public EventDeath Death;    // called when player dies
	public EventPlayerNoise PlayerNoise;    // called when player makes a noise, gets heard by enemies
	public EventPowerUp PowerUpCollected;	// called when a power up is picked up <the powerup picked up
	public EventFadeComplete FadeComplete;  // called when transition screen is done fading out
	public EventItemSwap ItemSwap;  // called when player swaps active item in inventory, <Item> new active item
	public EventItemSwap ItemUsed;  // called when player uses active item in inventory, <Item> active item used
	public EventItemSwap ItemIncrease;  // called when player picks up more of an item
	public EventFailedPortalEntry FailedPortalEntry;  // called when player attmepts to enter portal but doesn't have enough charge to exit level <int> charge required
}
