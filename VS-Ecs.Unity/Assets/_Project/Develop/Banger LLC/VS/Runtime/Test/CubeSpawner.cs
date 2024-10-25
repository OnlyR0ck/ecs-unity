using Arch.Core;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    private World _world;

    void Start()
    {
        // Create a new world
        _world = World.Create();

        // Define cube's initial position and rotation
        Position cubePosition = new Position { Value = new Vector3(0, 0, 0) };
        RotationComponent cubeRotation = new RotationComponent { Rotation = Quaternion.identity };
        RotationSpeed cubeSpeed = new RotationSpeed { Speed = 30f };

        // Create an entity with Position, Rotation, and RotationSpeed
        _world.Create(cubePosition, cubeRotation, cubeSpeed);

        // Instantiate a Unity cube for visualization
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = cubePosition.Value;
        cube.name = "Cube"; // Assign a name for reference in the update loop
    }
}