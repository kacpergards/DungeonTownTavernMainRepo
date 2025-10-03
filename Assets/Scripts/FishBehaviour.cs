using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    public float speed = 1.4f;
    public float lookSpeed = 2f;
    public Pond parentPond;
    private bool isMoving = false;
    private Vector3 currentDirection;
    public Vector3 direction;
    public Quaternion targetRotation;
    public float attracted = 0f;
    public float bobberDistance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (parentPond == null)
        {
        }
        else
        {
            if (parentPond.BobberGO != null)
            {
                checkAttraction(parentPond.BobberGO);
            }
                if (!isMoving)
                {
                    currentDirection = chooseDirection();
                    isMoving = true;
                    direction = currentDirection - transform.position;
                    targetRotation = Quaternion.LookRotation(direction);
                }
                else if (isMoving)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentDirection, speed * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, currentDirection) < 0.1f)
                    {
                        isMoving = false;
                    }
                }
            
        }
    }

    public void setPond(Pond pond)
    {
        parentPond = pond;
    }

    Vector3 chooseDirection()
    {
        Vector2 randomPositionCircle = Random.insideUnitCircle * parentPond.radius * 0.8f;
        float randomY = Random.Range(parentPond.pondBounds.max.y - 0.7f, parentPond.pondBounds.max.y - 0.3f);
        return new Vector3(parentPond.transform.position.x + randomPositionCircle.x, randomY, parentPond.transform.position.z + randomPositionCircle.y);
    }

    public void checkAttraction(GameObject bobberGO)
    {
        bobberDistance = Vector3.Distance(transform.position, bobberGO.transform.position);
        //fix the code kind of like this next time
        if (bobberDistance < 5f && bobberDistance > 4f)
        {
            attracted = 0.2f;
        }
        if (bobberDistance < 4f && bobberDistance > 3f)
        {
            attracted = 0.4f;
        }
        if (bobberDistance < 3f)
        {
            attracted = 0.7f;
        }
        else
        {
            attracted = 0f;
        }
    }

    public void attract()
    {

    }
}
