using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    // Starting position for parallax game object
    private Vector2 startingPosition;
    
    // Start Z value of parallax game object
    private float startingZ;
    
    // Distance that the camera has moved from the starting position of the parallax object
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    // If object is in front of target, use near clip plane. If behind target, use farClipPlane
    private float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
   
    // Further the object from the player, faster the ParallaxEffect object will move. Drag it's Z value closer to the target to make it move slower
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget ) / (clippingPlane);
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // When target moves, move parallax object the same distance * a multiplier 
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        
        // X/Y position changes based on target travel speed * parallax factor, Z stays consistent
        transform.position =  new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
