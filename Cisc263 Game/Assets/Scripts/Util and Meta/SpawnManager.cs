using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    /*Note: Map should be set to (-10,-10,0) rather than (0,0,0) to account for boundaries so there's grass under them
     * 
     * all item prefabs need to have their focal point in the center and x,y buffers should be set based on the distance from center!
     * 
     * example: an item with a buffer of (5, 10) can't have anything spawn within 5 units to right or left or within 10 units above or below center point.
     * 
     * TODO: set minimap camera relative to map
     * 
     */
    // pools for each of the items
    private ObjectPooler enemies;
    private ArrayPooler powerUps;
    private ArrayPooler obstacles;
    private ArrayPooler paths;
    private ObjectPooler exit;
    private ObjectPooler chargingStations;
    private ObjectPooler flames;
    private ObjectPooler chests;
    private BoundaryPool borders;

    [SerializeField] private GameObject map;    // map
    private Transform mapTransform;
    
    [SerializeField] private GameObject player; // player
    [SerializeField] private Vector2 playerSpawnPos;
    [SerializeField] private Vector2 playerBuffer; // required distance certain objects must be from player when level starts

    [SerializeField] private Camera miniMapCam; // minimap camera has to be adjusted to fit new dimensions
    private Transform camTransform;

    private bool[,] mapGrid; // 2d array representing map. element false = map space empty, true = occupied
    // private int[,] debuggingGrid; // mirrors mapGrid but keeps track of order placed in for debugging, dissable when done debugging
    // private int debuggingCounter;

    private List<Path> activePaths;   // array holding current path objects on map
    private List<ChargingStationScript> activeStations;   // array holding current station objects on map

    private ScentSpawner scentSpawner;  // scent stuff used for dissabling and reenabling scent spawning on new level
    private ScentPool scentPool;

    int fullCounter; // increments each time an item doesn't fit anywhere on the map, spawning stops after 5 misses 

    protected override void Awake()
    {
        base.Awake();
        enemies = gameObject.GetComponent<EnemyPool>();
        chargingStations = gameObject.GetComponent<ChargingStationPool>();
        powerUps = gameObject.GetComponent<PowerUpPool>();
        exit = gameObject.GetComponent<ExitPool>();
        obstacles = gameObject.GetComponent<ObstaclesPool>();
        paths = gameObject.GetComponent<PathPool>();
        chests = gameObject.GetComponent<ChestPool>();
        flames = gameObject.GetComponent<FlamePool>();
        borders = gameObject.GetComponent<BoundaryPool>();

        scentSpawner = player.GetComponent<ScentSpawner>();
        scentPool = player.GetComponent<ScentPool>();

        mapTransform = map.transform;
        camTransform = miniMapCam.transform;
    }

    private void Start()
    {
        // add listener for when new level done fading in to activate scent spawning
        EventManager.Instance.FadeComplete.AddListener(OnFadeOut);
    }


    #region level spawner

    public void LevelSpawner(int enemyNum, int pathNum, int chargingStationNum, int flamesNum, Vector2 mapDimensions)
    {
        // reset fullCounter and activePaths
        activePaths = new List<Path>();
        activeStations = new List<ChargingStationScript>();
        fullCounter = 0;
        // scale background, set miniMapCam and setup mapGrid
        mapTransform.localScale = new Vector3 (mapDimensions.x + 20, mapDimensions.y + 20, 1);    // makes a little wider to account for boundaries
        camTransform.position = new Vector3(mapDimensions.x / 2, mapDimensions.y / 2, -10);
        miniMapCam.orthographicSize = Mathf.Max(mapDimensions.x / 2, mapDimensions.y / 2);
        mapGrid = new bool[(int) mapDimensions.x, (int) mapDimensions.y];   // 2d array with dimensions of the map

        // place border around edge
        SpawnBorder(mapDimensions);

        // place player at middle of map (activate scent spawning once ui faded out)
        playerSpawnPos = mapDimensions/2;
        player.transform.position = playerSpawnPos;
        placeInGrid(playerSpawnPos, playerBuffer);

        // spawn paths
        for (int i = 0; i < pathNum; i++)
        {
            SpawnRandomPos(paths);
        }
        // spawn enemies
        for (int i = 0; i < enemyNum; i++)
        {
            SpawnRandomPos(enemies);
        }
        // spawn charging stations
        for (int i = 0; i < chargingStationNum; i++)
        {
            SpawnRandomPos(chargingStations);
        }
        // spawn flames
        for (int i = 0; i < flamesNum; i++)
        {
            SpawnRandomPos(flames);
        }
        // spawn powerups
        SpawnInPath(powerUps);
        // place exit
        SpawnInPath(exit);
        // spawn obstacles until map totally full
        while (fullCounter < 5)
        {
            SpawnRandomPos(obstacles);
        }
    }

    private void SpawnBorder(Vector2 mapDimensions)
    {
        // iterate the values by 33 since that's how far each of the colliders extent, couldn't find how to determine that from a variable
        GameObject wall;
        // spawn left walls
        for (int y = -10; y < mapDimensions.y; y += 33)
        {
            wall = borders.GetLeft();
            wall.transform.position = new Vector2(0, y);
            wall.SetActive(true);
        }
        // spawn upper walls
        for (int x = -10; x < mapDimensions.x; x += 33)
        {
            wall = borders.GetUpper();
            wall.transform.position = new Vector2(x, mapDimensions.y);
            wall.SetActive(true);
        }
        // spawn lower walls
        for (int x = -10; x < mapDimensions.x; x += 33)
        {
            wall = borders.GetLower();
            wall.transform.position = new Vector2(x, 0);
            wall.SetActive(true);
        }
        // spawn left walls
        for (int y = -10; y < mapDimensions.y; y += 33)
        {
            wall = borders.GetRight();
            wall.transform.position = new Vector2(mapDimensions.x, y);
            wall.SetActive(true);
        }
    }

    private void SpawnRandomPos(Pooler pool)
    {
        GameObject obj = pool.GetObject();
        SpaceBuffer bufferScript = obj.GetComponent<SpaceBuffer>();
        Vector2 buffer = bufferScript.spaceBuffer;
        Transform objTransform = obj.transform;

        intAndVector2 spotAndQuad = findOpeningInMapGrid(mapGrid,obj);
        Vector2 spawnPos = spotAndQuad.spawnLocation;
        // make sure the pos is valid and didn't just not find any space, if not valid just return and increment fullCounter
        if (spawnPos.Equals(new Vector2(-1, -1)))
        {
            fullCounter++;
            return;
        }
        //Debug.Log("adding " + obj);
        //Debug.Log("item placed at: " + spawnPos);
        placeInGrid(spawnPos, buffer);
        objTransform.position = spawnPos;
        obj.SetActive(true);
        if (obj.tag == "ChargingStation")
        {
            ChargingStationScript newStation = obj.GetComponent<ChargingStationScript>();
            newStation.inQuadrant = spotAndQuad.quadrant;
            activeStations.Add(newStation);
        }
        // if it's a path add it to the path array and set inQuadrant to the quadrant it was placed in
        if (obj.tag == "Path")
        {
            Path newPath = obj.GetComponent<Path>();
            newPath.inQuadrant = spotAndQuad.quadrant;
            activePaths.Add(newPath);
        }
    }

    // spawns random items from pool into paths on the map
    private void SpawnInPath(Pooler pool)
    {
        if (activePaths.Count == 0)
        {
            return;
        }
        Path pathScript;
        // if exit, just throw it in and return
        if (pool.Equals(exit))
        {
            GameObject exit = pool.GetObject();
            Transform exitTransform = exit.transform;
            int index = Random.Range(0, activePaths.Count);
            pathScript = activePaths[index];
            exitTransform.position = pathScript.exitPos;
            exit.SetActive(true);
            return;
        }
        // otherwise it's a powerup and go through each path with 75% chance to spawn
        foreach (Path activePath in activePaths)
        {
            int rng = Random.Range(0, 4);
            /*TODO: add it so that powerups get removed from options as they get put in, but only really removed from spawn pool if collected
             * set chest pos to just pathScript.powerUpPos remove z axis just for testing
             * 
             * 
             */
            // if rng == 0,1,2 but not 3
            if (rng < 3)
            {
                GameObject chest = chests.GetObject();
                Chest chestScipt = chest.GetComponent<Chest>();
                GameObject powerUp = powerUps.GetObject();
                PowerUp powerUpScript = powerUp.GetComponent<PowerUp>();
                pathScript = activePath;
                chestScipt.contains = powerUpScript;
                chest.transform.position = pathScript.powerUpPos;
                chest.transform.position += new Vector3(0,0,-10); // remove this line when chests put in, just for testing w 3d prefab
                chest.SetActive(true);
            }
        }
    }

    #region mapGrid manipulation

    struct intAndVector2
    {
        public int quadrant;
        public Vector2 spawnLocation;
    }

    /* 
     * performs a sort of tree search through mapGrid honing in to a random valid opening for the item in question.
     * recursively randomly picks a quadrant and searches it to verify item has enough open space to fit in quadrant
     * picks smaller and smaller quadrants until it doesnt fit in any quadrants of an area.
     * quad 1 = top left, quad 2 = top right, quad 3 = bottom left, quad 4 = bottom right
     * 
     * returns a vector repesenting a position it can safely spawn
    */
    private intAndVector2 findOpeningInMapGrid(bool[,] arrayToSearch, GameObject item)
    {
        intAndVector2 result;
        Vector2 buffer = item.GetComponent<SpaceBuffer>().spaceBuffer;
        Vector2 minSpace = (buffer * 2) + new Vector2(1,1); // ex an item with a buffer of (2,3) needs a 5x7 space to accomodate it
        bool[,] quadrant = new bool[(int) (arrayToSearch.GetLength(0) / 2), (int) (arrayToSearch.GetLength(1) / 2)]; // new 2d array quarter size of previous
        int[] quadOptions;
        // if item is a path, or cStation limit which quadrant it can go in based on which ones have them already
        // can't add to an occupied quadrant until all have same amount
        if (item.tag == "Path" || item.tag == "ChargingStation")
        {
            List<int> options = new List<int>();
            // each elem represents a quadrant, value is incremented for each item in it
            int[] quadrants = new int[4];
            switch (item.tag)
            {
                case "Path":
                    foreach (Path activePath in activePaths)
                    {
                        switch (activePath.inQuadrant)
                        {
                            case 1:
                                quadrants[0]++;
                                break;
                            case 2:
                                quadrants[1]++;
                                break;
                            case 3:
                                quadrants[2]++;
                                break;
                            case 4:
                                quadrants[3]++;
                                break;
                        }
                    }
                    break;
                case ("ChargingStation"):
                    foreach (ChargingStationScript station in activeStations)
                    {
                        switch (station.inQuadrant)
                        {
                            case 1:
                                quadrants[0]++;
                                break;
                            case 2:
                                quadrants[1]++;
                                break;
                            case 3:
                                quadrants[2]++;
                                break;
                            case 4:
                                quadrants[3]++;
                                break;
                        }
                    }
                    break;
            }

            int quadMax = Mathf.Max(quadrants);
            for (int i=0; i<quadrants.Length; i++)
            {
                if (quadrants[i] < quadMax)
                {
                    options.Add(i+1);   // if quadrant has less than others, add it to options
                }
            }

            if (options.Count == 0)
            {
                quadOptions = new int[] { 1, 2, 3, 4 }; // all quads are equivalent
            }
            else
            {
                quadOptions = options.ToArray();    // not all equal
            }
        }
        else
        {
            quadOptions = new int[] { 1, 2, 3, 4 };
        }
        int startingRow = 0;
        int endingRow = 0;
        int startingCol = 0;
        int endingCol = 0;
        // keep trying random different quadrants until you find one that can fit item
        while (true)
        {
            int index = Random.Range(0, quadOptions.Length);
            int quadrantNum = quadOptions[index];
            result.quadrant = quadrantNum;
            switch (quadrantNum)
            {
                case (1):
                    startingRow = 0;
                    endingRow = arrayToSearch.GetLength(0) / 2;
                    startingCol = 0;
                    endingCol = arrayToSearch.GetLength(1) / 2;
                    //Debug.Log("Going quad 1");
                    break;
                case (2):
                    startingRow = arrayToSearch.GetLength(0) / 2;
                    endingRow = arrayToSearch.GetLength(0);
                    startingCol = 0;
                    endingCol = arrayToSearch.GetLength(1) / 2;
                    // if array not divisible by 2, need to decrease edge values to account for halfs being floored
                    if (arrayToSearch.GetLength(0) % 2 == 1)
                    {
                        endingRow--;
                    }
                    //Debug.Log("Going quad 3");
                    break;
                case (3):
                    startingRow = 0;
                    endingRow = arrayToSearch.GetLength(0) / 2;
                    startingCol = arrayToSearch.GetLength(1) / 2;
                    endingCol = arrayToSearch.GetLength(1);
                    // if array not divisible by 2, need to decrease edge values to account for halfs being floored
                    if (arrayToSearch.GetLength(0) % 2 == 1)
                    {
                        endingCol--;
                    }
                    //Debug.Log("Going quad 2");
                    break;
                case (4):
                    startingRow = arrayToSearch.GetLength(0) / 2;
                    endingRow = arrayToSearch.GetLength(0);
                    startingCol = arrayToSearch.GetLength(1) / 2;
                    endingCol = arrayToSearch.GetLength(1);
                    // if array not divisible by 2, need to decrease edge values to account for halfs being floored
                    if(arrayToSearch.GetLength(0) % 2 == 1)
                    {
                        endingCol--;
                        endingRow--;
                    }
                    //Debug.Log("Going quad 4");
                    break;
                default:
                    Debug.Log("findOpeningInMapGrid fucked up in SpawnManager, sorry bud, please check");
                    break;
            }
            // copy the correct quadrant of arrayToSearch to quadrant
            int a = 0;
            int b = 0;
            for (int i = startingRow; i < endingRow; i++)
            {              
                for (int j = startingCol; j < endingCol; j++)
                {
                    quadrant[a, b] = arrayToSearch[i, j];                 
                    b = (b + 1) % quadrant.GetLength(1);
                }
                a++;
            }


            boolAndVector2 quadrantTest = hasSpaceFor(quadrant, minSpace, quadrantNum);
            if (quadrantTest.hasSpace)
            {
                // call this function recusively to find smallest quadrant in which this fits
                result.spawnLocation = findOpeningInMapGrid(quadrant, item).spawnLocation;
                // if the call of this function returned vector (-1,-1) that means this one was the smallest area item could fit in so use the location found in this quadrant
                if (result.spawnLocation.Equals(new Vector2(-1, -1)))
                {
                    // set location to the location found relative to this quadrant
                    result.spawnLocation = quadrantTest.locationFound;
                }
                // translate spawnLocation based on what quadrant this is
                switch (quadrantNum)
                {
                    case (1):
                        // don't do anything, quad one is just based at zero zero
                        // Debug.Log("quadrant 1: sitting");
                        break;
                    case (2):
                        // translate right half of parent grid's cols
                        result.spawnLocation += new Vector2(arrayToSearch.GetLength(1) / 2, 0);
                        // Debug.Log("quadrant 2: translating right: " + arrayToSearch.GetLength(1) / 2);
                        break;
                    case (3):
                        // translate up half of parent grid's rows
                        result.spawnLocation += new Vector2(0, arrayToSearch.GetLength(0) / 2);
                        // Debug.Log("quadrant 3: translating up: " + arrayToSearch.GetLength(0) / 2);
                        break;
                    case (4):
                        // translate right half of parent grid's cols and up half of parent grid's rows
                        result.spawnLocation += new Vector2(arrayToSearch.GetLength(0) / 2, arrayToSearch.GetLength(1) / 2);
                        // Debug.Log("quadrant 4: translating right: " + arrayToSearch.GetLength(0) / 2 + "and up: " + arrayToSearch.GetLength(1) / 2);
                        break;
                    default:
                        Debug.Log("findOpeningInMapGrid fucked up in SpawnManager, sorry bud, please check");
                        break;
                }
                return result;
            }

            // remove the quadrant just checked from the available choises
            int[] newOptions = new int[quadOptions.Length - 1];
            int x = 0;
            for(int i=0; i < quadOptions.Length; i++)
            {
                if (quadOptions[i] != quadrantNum)
                {
                    newOptions[x] = quadOptions[i];
                    x++;
                }
            }
            quadOptions = newOptions;
            // Debug.Log(quadOptions);

            // if none of the quadrants in this area could fit the item
            if (quadOptions.Length == 0)
            {
                result.spawnLocation = new Vector2(-1, -1);
                return result;
            }
        }
    }



    struct boolAndVector2
    {
        public bool hasSpace;
        public Vector2 locationFound;
    }

    // returns true if quadrant contains enough free space to accomodate space required
    // uses [i,j] as a pointer for iterating over the whole array, and [k,l] as a pointer to iterate over an open area in the array
    private boolAndVector2 hasSpaceFor(bool[,] quadrant,Vector2 space, int debuggingQuad)
    {
        bool lastElem = true;   // value of last element checked
        bool traversingOpening = false;
        bool movedPointer = false;
        int lCounter = 0;
        int kCounter = 0;
        Vector2 spaceMidPoint = new Vector2((int)space.x / 2, (int) space.y / 2);
        boolAndVector2 output;

        // traverse quadrant
        for (int i=0; i< quadrant.GetLength(0) - space.x; i++)  // only needs to check up to space.x rows away from the edge since if it gets that far it won't be able to fit anyway
        {
            for (int j=0; j< quadrant.GetLength(1) - space.y; j++)  // only needs to check up to space.y columns away from the edge since if it gets that far it won't be able to fit anyway
            {
                // entering a new opening once moved pointer has been set to true, will continue running this part on openings
                if ((lastElem == true || movedPointer) && quadrant[i, j] == false)
                {
                    traversingOpening = true;
                    // start scanning this opening until it hits a used space or finds it large enough to accomodate item
                    while (traversingOpening)
                    {
                        int upperBound = (int)(i + space.x);   // how many rows down to check to verify opening large enough
                        int rightBound = (int)(j + space.y);   // how many columns over to check to verify ,, ,, ,,
                        // replace this later please if u have time, this is not a great fix
                        if (upperBound >= quadrant.GetLength(0) || rightBound >= quadrant.GetLength(1))
                        {
                            output.hasSpace = false;
                            output.locationFound = Vector2.zero;
                            return output;
                        }
                        //Debug.Log("upperBound: " + upperBound + " rightBound: " + rightBound + " quadrantBound: (" + quadrant.GetLength(0) + ", " + quadrant.GetLength(1) + ") space required: (" + space.x + ", " + space.y + ")" + " starting from: (" + i + ", " + j + ") in quad: " + debuggingQuad);
                        for (int k = i; k < upperBound; k++)
                        {
                            for (int l = j; l < rightBound; l++)
                            {
                                // Debugging
                                //if (k >= quadrant.GetLength(0) || k < 0 || l >= quadrant.GetLength(1) || l < 0)
                                //{
                                //Debug.Log("upperBound: " + upperBound + " rightBound: " + rightBound + " quadrantBound: (" + quadrant.GetLength(0) + ", " + quadrant.GetLength(1) + ") space required: (" + space.x + ", " + space.y + ")" + " starting from: (" + i + ", " + j + ")");
                                //}
                                // if it hits an occupied entry while searching the opening
                                if (quadrant[k, l] == true)
                                {
                                    movedPointer = true;
                                    /* keep moving over until you find an empty column, then set j to that free column, 
                                     * otherwise it will just keep failing once it hits this row in subsequent searches
                                     * 
                                     * if no free column found within space.y coloumns from the edge, just set i to the next row down as we can count out all rows above this one
                                    */
                                    while (true)
                                    {
                                        // if col within last space.x cols of quadrand
                                        if (l >= quadrant.GetLength(1) - space.x)
                                        {
                                            // if this impassable row is within the last space.x rows of quadrant, not enough space in quadrant, just quit
                                            if (k >= quadrant.GetLength(0) - space.y)
                                            {
                                                // made it through entire quadrant without finding valid space return false
                                                output.hasSpace = false;
                                                output.locationFound = Vector2.zero;
                                                return output;
                                            }
                                            // otherwise 
                                            i = k + 1;  // set i to the row below this one since this row is impassable
                                            j = 0;  // start from the beginning of this new row
                                            traversingOpening = false;
                                            break;
                                        }
                                        l++;
                                        // hit an empty col
                                        if (quadrant[k, l] == false)
                                        {
                                            j = l;  // keep i to the row it was, but jump the column ahead to after the last occupied column in this row
                                            traversingOpening = false;
                                            break;
                                        }
                                    }
                                }
                                if (!traversingOpening)
                                {
                                    break;
                                }
                                lCounter = l;
                            }
                            if (!traversingOpening)
                            {
                                break;
                            }
                            kCounter = k;
                        }
                        // if it makes it though all l's and k's, that means it searched enough cells to guarantee enough space
                        if (kCounter == upperBound-1 && lCounter == rightBound-1)
                        {
                            output.hasSpace = true;
                            // [i,j] pointer is at botom left corner of valid space searched, [k,l] pointer is at top right corner of valid space searched
                            output.locationFound = spaceMidPoint + new Vector2(i, j);
                            // Debug.Log("vecor being added to spawn point: " + new Vector2(i,j));
                            return output;
                        }
                    }
                }
                lastElem = quadrant[i, j];
            }
        }
        // if it makes it through the entire quadrant without finding valid space return false
        output.hasSpace = false;
        output.locationFound = Vector2.zero;
        return output;
    }


    // sets the appropriate elements in the grid to true to represent the space taken by an item
    // sets elements from bufferRadius.y above and bufferRadius.x to the left of location down to 
    // bufferRadius.y below and bufferRadius.x to the right of location
    private void placeInGrid(Vector2 location, Vector2 bufferRadius)
    {
        int leftBound = (int) (location.y - (bufferRadius.y));
        int rightBound = (int)(location.y + (bufferRadius.y));
        int lowerBound = (int)(location.x - (bufferRadius.x));
        int upperBound = (int) (location.x + (bufferRadius.x));
        if (lowerBound < 0 || upperBound >= mapGrid.GetLength(0) || leftBound < 0 || rightBound >= mapGrid.GetLength(1))
        {
            Debug.Log("attempted to place item out of bounds, check SpawnManager, location: " + location + " buffer: " + bufferRadius);
            return;
        }
        // Debug.Log("setting: (" + lowerBound + ", " + leftBound + ") to (" + upperBound + ", " + rightBound + ") to true.");
        //debuggingCounter += 1; // comment out when not debugging

        for (int i = lowerBound; i <= upperBound; i++)
        {
            for (int j = leftBound; j <= rightBound; j++)
            {
                mapGrid[i, j] = true;
                //debuggingGrid[i, j] = debuggingCounter; // comment out
            }
        }
        /*
        Debug.Log("updated grid:");
        for (int i = 0; i < mapGrid.GetLength(0); i++)
        {
            Debug.Log("ROW " + i + ": ");
            string line = "";
            for (int j = 0; j < mapGrid.GetLength(1); j++)
            {
                int value = debuggingGrid[i,j];

                if (i == mapGrid.GetLength(0) / 2)
                {
                    line += "-";
                }
                if (j == mapGrid.GetLength(1) / 2)
                {
                    line += "|";
                }
                line += "  " + value;
            }
            Debug.Log(line);
        }
        */
    }

    #endregion

    // activate scent spawning and enemies
    private void OnFadeOut()
    {
        scentSpawner.enabled = true;
        // TODO: setup enemy toggleing
    }
    #endregion

    public void LevelUnloader()
    {
        enemies.ResetPool();
        powerUps.ResetPool();
        chests.ResetPool();
        chargingStations.ResetPool();
        obstacles.ResetPool();
        exit.ResetPool();
        paths.ResetPool();
        borders.ResetPool();
        flames.ResetPool();

        // disable scent spawning and remove lingering scent nodes
        scentSpawner.enabled = false;
        scentSpawner.CancelInvoke();
        scentPool.ResetPool();
    }
}
