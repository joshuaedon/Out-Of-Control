using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings {
    public static int rows = 12;
    public static int roadVerticalLength = rows / 2;
    public static int roadHorizontalLength = rows / 2;
    public static float roadChance = 0.75f;

    public static float camSpeed = 0.1f;
    public static float playerSpeed = 2f;
    public static float turningSpeed = 7.5f;
    public static float stopDistance = 0.1f;
    public static float pointDeviation = 0.15f;
    public static float pointLerp = 0.5f;

    public static float hoseDistance = 1f;

    public static float houseMargin = 0.1f;
}
