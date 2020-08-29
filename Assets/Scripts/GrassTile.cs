using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile {
	public List<Structure> structures;

	void Start() {
		structures = new List<Structure>();
	}
}
