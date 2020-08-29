using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Type", menuName = "Structure Type")]
public class StructureType : ScriptableObject {
    public Sprite sprite;
    public Vector2 dimensions;
}
