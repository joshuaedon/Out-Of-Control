using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
	public Vector2Int pos;

	public void setPos(int col, int row) {
		this.pos = new Vector2Int(col, row);
		transform.position = new Vector2(col, row);
	}

	// -1 - remove, 0 - nothing, 1 - replace
	public int replace() {
		List<Vector2Int> offs = new List<Vector2Int>() {new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1)};

		for(int i = 0; i < offs.Count; i++) {
			for(int j = i+1; j < offs.Count; j++) {
				if((getTile(offs[i]) is RoadTile && getTile(offs[j]) is RoadTile) || (getTile(offs[i]) is GrassTile && getTile(offs[j]) is GrassTile)) {
					if(getTile(offs[i] + offs[j]) is RoadTile)
						return -1;
				}
			}
		}

		foreach(Vector2Int off in offs) {
			if(getTile(off) is RoadTile) {
				if(Random.Range(0f, 1f) <= Settings.roadChance)
					return 1;
				else
					return -1;
			}
		}
		return 0;		
	}

	// Gets the tile at a certain offset from this tile
	private Tile getTile(Vector2Int off) {
		if(this.pos.x - Controller.startCol + off.x >= 0 && this.pos.x - Controller.startCol + off.x < Controller.grid.Count
			&& this.pos.y + off.y >= 0 && this.pos.y + off.y < Settings.rows)
			return Controller.getTile(this.pos + off);
		return null;
	}

	public Tile[] getAdjacent() {
		return new Tile[] {getTile(new Vector2Int(-1, 0)), getTile(new Vector2Int(0, -1)), getTile(new Vector2Int(1, 0)), getTile(new Vector2Int(0, 1))};
	}
}
