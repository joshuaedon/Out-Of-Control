using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile {
	public Tile from;
	public bool destination;

	void Start() {
		this.from = null;
	}

	void Update() {
		if(from != null || destination)
			Debug.Log("problem");
	}

	public void setSprite() {
		Sprite[] tileSprites = Resources.LoadAll<Sprite>("Sprites/RoadTileSet");
		int index = 0;

		Tile[] adjacent = getAdjacent();
		for(int i = 0; i < adjacent.Length; i++) {
			if(adjacent[i] is RoadTile)
				index += (int)Mathf.Pow(2, i);
		}

		GetComponent<SpriteRenderer>().sprite = tileSprites[index];
	}
}
