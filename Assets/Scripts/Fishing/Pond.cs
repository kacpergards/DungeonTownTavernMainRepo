using UnityEngine;


public class Pond : MonoBehaviour
{
    public GameObject FishGO;
    public GameObject BobberGO;
    public int maxFish = 0;
    public float radius;
    public Bounds pondBounds;

    // Start is called before the first frame update
    void Start()
    {
        FishGO = Resources.Load<GameObject>("Prefabs/Fish_trial");
        pondBounds = gameObject.GetComponent<Renderer>().bounds;
        radius = (pondBounds.max.x - pondBounds.min.x) / 2;

        for (int i = 1; i <= maxFish; i++)
        {
            Vector2 randomPositionCircle = Random.insideUnitCircle * radius * 0.8f;
            Vector3 spawnPosition = new Vector3(transform.position.x + randomPositionCircle.x, pondBounds.max.y - 0.5f, transform.position.z + randomPositionCircle.y);
            GameObject spawnedFish = Instantiate(FishGO, spawnPosition, Quaternion.identity);
            FishBehaviour fish = spawnedFish.GetComponent<FishBehaviour>();
            fish.setPond(this);
        }

    }

    public void assignBobber(GameObject Bobber)
    {
        BobberGO = Bobber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
