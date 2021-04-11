public class ContinuityManager : Singleton<ContinuityManager>
{
    public bool chestOpened;
    public bool npcTalkedTo;
    public bool portalEntered;

    override protected void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
}
