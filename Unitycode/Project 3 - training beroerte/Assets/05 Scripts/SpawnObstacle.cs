using UnityEngine;
using System.Collections;

public class SpawnObstacle : MonoBehaviour
{
  //for randomly spawning obstacles (poop, garbage, bananaPeel...) near waypoints

  public GameObject bananaPeel;
  public GameObject paperTrash;
  public GameObject poop;
  Vector3 obstaclePos; //to place an obstacle at a chosen position
  GameObject[] waypointContainer;
  float heightBetweenWpAndGround = 0.35f; //to make sure the obstacles are on the ground
  byte maxRandomObstacle = 13;
  byte showObstacleMinimum = 6;
  string wayPointContainerTag = "wpLocForObst";

  void Start()
  {
    //usually the first waypoint in each path was tagged with this tag
    waypointContainer = GameObject.FindGameObjectsWithTag(wayPointContainerTag);

    foreach (GameObject wp in waypointContainer)
    {
      obstaclePos = new Vector3(wp.transform.position.x, wp.transform.position.y - heightBetweenWpAndGround, wp.transform.position.z); ///use current WP position but lower Y-pos to place on ground
      byte randomDeciderPlaceObstacle = (byte)Random.Range(1, maxRandomObstacle); // will decide if an obstacle will be placed or not (between 1 and 12 > 12 to even out the odds)

      if (randomDeciderPlaceObstacle > showObstacleMinimum)
      {
        switch (randomDeciderPlaceObstacle) //decides which obstacle will be instantiated
        {
          case 7: //when number is 7 or 8
          case 8:
            Instantiate(poop, obstaclePos, Quaternion.identity);
            break;
          case 9:
          case 10:
            Instantiate(paperTrash, obstaclePos, Quaternion.identity);
            break;
          case 11:
          case 12:
            Instantiate(bananaPeel, obstaclePos, Quaternion.identity);
            break;
          default://do nothing
            break;
        }
      }
    }
  }
}
