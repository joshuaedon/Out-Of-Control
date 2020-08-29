using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public static List<List<Tile>> grid;
	public static int startCol;
	private int roadRow;

	private FireEngine fireEngine;

    void Start() {
        grid = new List<List<Tile>>();
    	startCol = 0;
    	this.roadRow = Random.Range(1, Settings.rows-1);

    	GameObject fireEngineObj = (GameObject)Instantiate(Resources.Load("prefabs/FireEngine"), transform);
    	this.fireEngine = fireEngineObj.GetComponent<FireEngine>();
    	transform.position = new Vector3(0, this.roadRow, 0);
    }

    void Update() {
    	// Add and remove tiles
        if(Camera.main.transform.position.x > startCol + 2 * Settings.rows) {
        	foreach(Tile tile in grid[0])
        		Destroy(tile.gameObject);
        	grid.RemoveAt(0);
        	startCol++;
        }
        if(Camera.main.transform.position.x > startCol + grid.Count - 2 * Settings.rows) {
        	addCols();
        }

        if(Input.GetMouseButtonDown(0)) {
        	Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this.fireEngine.route = findRoute(new Vector2(pos.x, pos.y));

        	// RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            
        //     if(hit.transform != null) {
        //     	Tile destination = null;
        //     	if(hit.transform.GetComponent<Tile>() != null)
        // 			destination = hit.transform.GetComponent<Tile>();

        // 		if(destination != null)
    				// this.fireEngine.route = findRoute(destination);
        //     }
        }
    }

    public static Tile getTile(Vector2Int pos) {
    	return grid[pos.x - startCol][pos.y];
    }

    private bool structureOverlap(Vector2 pos, Vector2 dimensions) {
    	for(int i = Mathf.Max(startCol, Mathf.RoundToInt(pos.x) - 1); i <= Mathf.Min(Mathf.RoundToInt(pos.x) + 1, startCol + grid.Count - 1); i++) {
    		for(int j = Mathf.Max(0, Mathf.RoundToInt(pos.y) - 1); j <= Mathf.Min(Mathf.RoundToInt(pos.y) + 1, Settings.rows - 1); j++) {
    			Tile tile = getTile(new Vector2Int(i, j));
    			if(tile is GrassTile) {
	    			foreach(Structure structure in ((GrassTile)tile).structures) {
	    				if(Mathf.Abs(structure.pos.x - pos.x) < (structure.type.dimensions.x + dimensions.x) / 2f && Mathf.Abs(structure.pos.y - pos.y) < (structure.type.dimensions.y + dimensions.y) / 2f)
	    					return true;
	    			}
	    		}
    		}
    	}
    	return false;
    }

    private void addCols() {
        GameObject referenceGrassTile = (GameObject)Instantiate(Resources.Load("prefabs/GrassTile"));
        GameObject referenceRoadTile = (GameObject)Instantiate(Resources.Load("prefabs/RoadTile"));

        int colOff = grid.Count;
        int cols = Random.Range(2, Settings.roadHorizontalLength);

        // Create horizontal road
        for(int c = 0; c < cols-1; c++) {
	    	grid.Add(new List<Tile>());
	    	for(int r = 0; r < Settings.rows; r++) {
	    		GameObject tileObj;
	    		if(r == this.roadRow)
	    			tileObj = (GameObject)Instantiate(referenceRoadTile, transform.GetChild(0));
	    		else
	    			tileObj = (GameObject)Instantiate(referenceGrassTile, transform.GetChild(0));

	    		Tile tile = tileObj.GetComponent<Tile>();
	    		grid[grid.Count - 1].Add(tile);
	    		tile.setPos(startCol + grid.Count - 1, r);
	    	}
	    }
	    // Create vertical road
    	int newRoadRow = Random.Range(Mathf.Max(1, this.roadRow - Settings.roadVerticalLength), Mathf.Min(Settings.rows-1, this.roadRow + Settings.roadVerticalLength));
	    grid.Add(new List<Tile>());
    	for(int r = 0; r < Settings.rows; r++) {
    		GameObject tileObj;
    		if((r <= this.roadRow && r >= newRoadRow) || (r >= this.roadRow && r <= newRoadRow))
    			tileObj = (GameObject)Instantiate(referenceRoadTile, transform.GetChild(0));
    		else
    			tileObj = (GameObject)Instantiate(referenceGrassTile, transform.GetChild(0));

    		Tile tile = tileObj.GetComponent<Tile>();
    		grid[grid.Count - 1].Add(tile);
    		tile.setPos(startCol + grid.Count - 1, r);
    	}
    	this.roadRow = newRoadRow;
    	// Fill gaps with roads
    	List<Tile> tiles = new List<Tile>();
        for(int c = colOff; c < grid.Count; c++) {
        	for(int r = 1; r < Settings.rows-1; r++) {
        		if(grid[c][r] is GrassTile)
        			tiles.Add(grid[c][r]);
        	}
        }
        int count = 0;
        while(tiles.Count > 0 && count < 1000) {
        	int index = Random.Range(0, tiles.Count);
        	Tile tile = tiles[index];
        	int replace = tile.replace();
        	if(replace == 1) {
        		tiles.RemoveAt(index);
        		GameObject tileObj = (GameObject)Instantiate(referenceRoadTile, transform.GetChild(0));
	    		Tile newTile = tileObj.GetComponent<Tile>();
	    		grid[tile.pos.x - startCol][tile.pos.y] = newTile;
	    		newTile.setPos(tile.pos.x, tile.pos.y);
	    		Destroy(tile.gameObject);
        	} else if(replace == -1) {
        		tiles.RemoveAt(index);
        	}

        	if(replace == 0)
        		count++;
        	else
        		count = 0;
        }

        for(int c = Mathf.Max(0, colOff - 1); c < grid.Count; c++) {
        	for(int r = 1; r < Settings.rows-1; r++) {
        		if(grid[c][r] is RoadTile)
        			((RoadTile)grid[c][r]).setSprite();
        	}
        }

        Destroy(referenceGrassTile);
        Destroy(referenceRoadTile);


        // Buildings
        // tiles = new List<Tile>();
        GameObject referenceStructure = (GameObject)Instantiate(Resources.Load("prefabs/Structure"));
        for(int c = colOff; c < grid.Count; c++) {
        	for(int r = 0; r < Settings.rows; r++) {
        		if(grid[c][r] is GrassTile) {
        			// tiles.Add(grid[c][r]);
        			// South facing
        			if(r > 0 && grid[c][r-1] is RoadTile) {
        				GameObject structureObj = (GameObject)Instantiate(referenceStructure, transform.GetChild(1));
        				Structure structure = structureObj.GetComponent<Structure>();
        				structure.setRandomType();
        			}

        		}
        	}
        }
		Destroy(referenceStructure);

    }

    private List<RoadTile> getClosestRoads(Vector2 pos) {
		List<RoadTile> closest = new List<RoadTile>();

		float radius = 0.5f;//fireEngine.getHoseDistance();

		while(closest.Count == 0) {
			for(int i = Mathf.Max(0, Mathf.CeilToInt(pos.x - radius)); i < Mathf.Min(Mathf.CeilToInt(pos.x + radius), grid.Count + startCol); i++) {
				for(int j = Mathf.Max(0, Mathf.CeilToInt(pos.y - radius)); j < Mathf.Min(Mathf.CeilToInt(pos.y + radius), Settings.rows); j++) {
					Tile tile = getTile(new Vector2Int(i, j));
					if(Vector2.Distance(pos, tile.pos) <= radius && tile is RoadTile)
						closest.Add((RoadTile)tile);
				}
			}

			radius += 0.9f;
		}

		return closest;
	}

    private List<Vector2> findRoute(Vector2 pos) {
		List<RoadTile> closestRoads = getClosestRoads(pos);
    	foreach(RoadTile tile in closestRoads)
    		tile.destination = true;

    	List<Vector2> route = new List<Vector2>();
    	Tile start = getTile(new Vector2Int(Mathf.RoundToInt(fireEngine.transform.position.x),
    										Mathf.RoundToInt(fireEngine.transform.position.y)));
    	Tile end = null;

    	List<Tile> open = new List<Tile>();
    	List<Tile> closed = new List<Tile>();
    	open.Add(start);

    	while(open.Count > 0 && end == null) {
    		foreach(Tile adjacent in open[0].getAdjacent()) {
    			if(adjacent is RoadTile && ((RoadTile)adjacent).from == null) {
    				((RoadTile)adjacent).from = open[0];
    				open.Add(adjacent);
    				if(((RoadTile)adjacent).destination) {
    					end = adjacent;
    					break;
    				}
    			}
    		}
    		closed.Add(open[0]);
    		open.RemoveAt(0);
    	}

    	while(end != start) {
    		Vector2 point = new Vector2(end.pos.x + Random.Range(-Settings.pointDeviation, Settings.pointDeviation),
    									end.pos.y + Random.Range(-Settings.pointDeviation, Settings.pointDeviation));
    		route.Insert(0, point);
    		end = ((RoadTile)end).from;
    	}
    	for(int i = 1; i < route.Count - 1; i++)
			route[i] = Vector2.Lerp(route[i], Vector2.Lerp(route[i-1], route[i+1], 0.5f), Settings.pointLerp);
    	if(route.Count == 0)
    		route.Add(start.pos);
    	// Move last coordinate closer to the clicked position
    	float dist = Vector2.Distance(route[route.Count-1], pos);
		route[route.Count-1] = Vector2.Lerp(route[route.Count-1], pos, 0.25f / dist);


    	foreach(Tile tile in open) {
    		if(tile is RoadTile)
    			((RoadTile)tile).from = null;
    	}
    	foreach(Tile tile in closed) {
    		if(tile is RoadTile)
    			((RoadTile)tile).from = null;
    	}
    	foreach(RoadTile tile in closestRoads) {
			tile.destination = false;
    	}

    	return route;
    }
}