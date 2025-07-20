using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] List<Cell> cells;
    [SerializeField] DecisionCell DecisionCellPrefab;
    [SerializeField] Cell defaultCellPrefab;
    [SerializeField] Cell entrancePrefab;
    [SerializeField] int initialBuildToken;
    [SerializeField] Placeable treasuryPrefab;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] Transform buttons;
    [SerializeField] float cellSpacing = 1f; // <-- added

    bool isDecisionMade;

    public List<Cell> DefaultCells = new List<Cell>();
    List<DecisionCell> DecisionCells = new List<DecisionCell>();

    public int ExpansionCost;
    public Transform GridOrigin;
    Placeable treasury;
    public bool IsGameStart = true;
    public static GridManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        Cell entrance = Instantiate(entrancePrefab, GridOrigin);
        navMeshSurface.BuildNavMeshAsync();
        entrance.Init(Vector2.zero, cellSpacing); // <-- modified
        cells.Add(entrance);
        for (int i = 0; i < initialBuildToken; i++)
        {
            CreateDecisionCells();
            navMeshSurface.BuildNavMeshAsync();
            isDecisionMade = false;
            yield return new WaitUntil(() => isDecisionMade == true);
        }
        //yield return new WaitUntil(() => cells.All(c => c.GetComponent<BoxCollider2D>().isActiveAndEnabled));
        yield return new WaitForSeconds(0.1f);
        navMeshSurface.BuildNavMeshAsync();
        yield return PlaceTreasure();
        IsGameStart = false;
    }

    public void OnPlaceTreasureButton()
    {
        StartCoroutine(PlaceTreasure());
    }

    IEnumerator PlaceTreasure()
    {
        if (treasury != null)
        {
            Destroy(treasury.gameObject);
            buttons.gameObject.SetActive(false);
        }
        treasury = Instantiate(treasuryPrefab, GridOrigin);
        isDecisionMade = false;
        while (!isDecisionMade)
        {
            //Debug.Log(treasury.CheckPlacmentRequirments());
            treasury.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && treasury.CheckPlacmentRequirments())
            {
                isDecisionMade = true;
            }
            yield return null;
        }
        buttons.gameObject.SetActive(true);
    }

    public void InBetweenWaveProcess()
    {
        StartCoroutine(BetweenWavesProcess());
    }

    IEnumerator BetweenWavesProcess()
    {
        while (WaveSpawner.gameState == WaveSpawner.WaveSpawnerState.BUILDING)
        {
            isDecisionMade = false;
            CreateDecisionCells();
            yield return new WaitUntil(() => isDecisionMade || (WaveSpawner.gameState != WaveSpawner.WaveSpawnerState.BUILDING));
            yield return new WaitForSeconds(0.1f);
            navMeshSurface.BuildNavMeshAsync();
        }
        DeleteDecisionCells();
    }

    void OnDecisionMade(DecisionCell decisionCell)
    {
        // Cell defaultCell = Instantiate(defaultCellPrefab, GridOrigin);
        // cells.Add(defaultCell);
        // defaultCell.Init(decisionCell.Coordinates);
        // foreach (Vector2 direction in decisionCell.WallsDirections)
        // {
        //     defaultCell.TweakWall(direction, false);
        //     Cell targetCell = cells.First(c => c.CoordinatesOnTheGrid == (defaultCell.CoordinatesOnTheGrid + direction));
        //     targetCell.TweakWall(-direction, false);
        // }
        // DeleteDecisionCells();
        // isDecisionMade = true;
        if(!IsGameStart)
            if (GoldTracker.gold < ExpansionCost)
                return;
            else
                GoldTracker.SpendGold(ExpansionCost);
        StartCoroutine(CreateDefaultCell(decisionCell));
    }

    IEnumerator CreateDefaultCell(DecisionCell decisionCell)
    {
        Cell defaultCell = Instantiate(defaultCellPrefab, GridOrigin);
        DefaultCells.Add(defaultCell);
        yield return new WaitUntil(() => defaultCell.gameObject.activeInHierarchy);
        cells.Add(defaultCell);
        defaultCell.Init(decisionCell.Coordinates, cellSpacing); // <-- modified
        yield return new WaitUntil(() => (Vector2)defaultCell.transform.localPosition == decisionCell.Coordinates * cellSpacing); // <-- modified
        foreach (Vector2 direction in decisionCell.WallsDirections)
        {
            defaultCell.TweakWall(direction, false);
            Cell targetCell = cells.First(c => c.CoordinatesOnTheGrid == (defaultCell.CoordinatesOnTheGrid + direction));
            targetCell.TweakWall(-direction, false);
            yield return new WaitUntil(() => defaultCell.CheckWallState(direction, false) && targetCell.CheckWallState(-direction, false));
        }
        DeleteDecisionCells();
        isDecisionMade = true;
    }

    void DeleteDecisionCells()
    {
        while (DecisionCells.Count > 0)
        {
            DecisionCell decisionCell = DecisionCells[0];
            DecisionCells.Remove(decisionCell);
            Destroy(decisionCell.gameObject);
        }
    }

    public void CreateDecisionCells()
    {
        foreach (Cell cell in cells)
        {
            List<Vector2> validDirections = cell.GetActiveWalls();
            foreach (Vector2 dir in validDirections)
            {
                if ((DecisionCells.Count == 0) || !DecisionCells.Any(c => c.Coordinates == (cell.CoordinatesOnTheGrid + dir)))
                {
                    DecisionCell newDecisionCell = Instantiate(DecisionCellPrefab, GridOrigin);
                    newDecisionCell.Init(cell.CoordinatesOnTheGrid + dir, cellSpacing); // <-- modified
                    newDecisionCell.OnCellClick += OnDecisionMade;
                    newDecisionCell.AddWallDirection(-dir);
                    DecisionCells.Add(newDecisionCell);
                }
                else
                {
                    DecisionCell decisionCell = DecisionCells.Where(c => c.Coordinates == (cell.CoordinatesOnTheGrid + dir)).First();
                    decisionCell.AddWallDirection(-dir);
                }
            }
        }
    }
}
