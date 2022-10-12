using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int asteroidCount = 0, randCount = 0, maxAsteroids = 50;
    private List<GameObject> asteroidField = new List<GameObject>();
    private float curAsterX = 0.0f, curAsterY = 0.0f, curAsterZ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        randCount = Random.Range(1, maxAsteroids);
        this.generateAsteriodField();
        float randY = Random.Range(-180, 180);
        gameObject.transform.rotation = Quaternion.Euler(0, randY, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void generateAsteriodField()
    {
        do
        {
            asteroidField.Add(Instantiate(asteroidPrefab, gameObject.transform, false));
            
            curAsterY = Random.Range(-4.0f, 4.0f);
            curAsterX += 2.0f;
            curAsterZ = 0.004f * (curAsterX * curAsterX);

            float asteroidJiggle = Random.Range(-4.0f, 4.0f);

            if (asteroidCount % 2 == 0)
            {
                asteroidField[asteroidCount].transform.localPosition = new Vector3(curAsterX - asteroidJiggle, curAsterY, curAsterZ + asteroidJiggle);
            }
            else
            {
                asteroidField[asteroidCount].transform.localPosition = new Vector3(-curAsterX + asteroidJiggle, curAsterY, curAsterZ + asteroidJiggle);
            }
            
            
            asteroidCount++;
        }
        while (asteroidCount < randCount);
    }
}
