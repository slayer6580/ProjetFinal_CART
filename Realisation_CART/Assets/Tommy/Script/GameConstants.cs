using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
	public const float BASE_ADD_FORCE = 1000;
	public const float DEADZONE = 0.1f;

    public const int MEDIUM_SIZE = 2;
    public const int LARGE_SIZE = 4;

    // Layers
    public const int DEFAULT = 0;
    public const int PLAYER_BODY = 3;
    public const int PLAYER_COLLIDER = 6;
    public const int CLIENT_COLLIDER = 7;
    public const int GROUND_COLLIDER = 10;
    public const int SHELF_COLLIDER = 11;
    public const int CAMERA = 17;
}

