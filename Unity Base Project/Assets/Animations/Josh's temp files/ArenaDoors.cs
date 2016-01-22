using UnityEngine;
using System.Collections;

public class ArenaDoors : MonoBehaviour 
{
    public EnemySpawner spawner = null;
    public GameObject[] doors = new GameObject[2];

    public bool doorsUp = false;

    public float startY;

    public float hideY = -10.0f;

	void Start () 
    {
        startY = doors[0].transform.position.y;

	    foreach(GameObject door in doors)
        {
            door.transform.position = new Vector3(door.transform.position.x, hideY, door.transform.position.z);
        }
	}
	
    public IEnumerator MoveDoors(bool moveUp, float time)
    {
        float elapsedTime = 0.0f;

        float currY = transform.position.y;

        while (elapsedTime < time)
        {
            // which direction are we lerping
            if (moveUp)
                currY = Mathf.Lerp(hideY, startY, elapsedTime / time);
            else
                currY = Mathf.Lerp(startY, hideY, elapsedTime / time);
            foreach(GameObject door in doors)
            {
                door.transform.position = new Vector3(door.transform.position.x, currY, door.transform.position.z);
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame(); // this is what waits for the next frame
        }

        // ensure the doors are at their final position
        float finalY = moveUp ? startY : hideY;
        foreach (GameObject door in doors)
        {
            door.transform.position = new Vector3(door.transform.position.x, finalY, door.transform.position.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("PLAYER HAS ENTERED");
            StartCoroutine(MoveDoors(true, 1.5f));

            if (spawner)
                spawner.SpawnAll();

            Destroy(GetComponent<BoxCollider>());
        }
    }

    public void MoveTheDoorsPls(bool moveUp)
    {
        Debug.Log("KILL COUNT ACHIEVED");
        StartCoroutine(MoveDoors(moveUp, 1.5f));
    }

}
