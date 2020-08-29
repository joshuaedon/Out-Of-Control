using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {
	public StructureType type;
	public Vector2 pos;

	public void setRandomType() {
		if(Random.Range(0f, 1f) < 0.5f)
			this.type = Resources.Load<StructureType>("Structures/House");
		else
			this.type = Resources.Load<StructureType>("Structures/Tree");
		setSprite();
	}

	public void setSprite() {
		GetComponent<SpriteRenderer>().sprite = this.type.sprite;
    }
}
