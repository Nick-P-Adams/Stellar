using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebula : MonoBehaviour
{
    public List<GameObject> stellarObjPrefabs = new List<GameObject>();
    public int stellarObjCount = 0, randStellarObjCount = 0, maxStellarObj = 600;
    public Camera camera;
    public GameObject player;
    private List<GameObject> nebula = new List<GameObject>();
    private List<GameObject> stars = new List<GameObject>();
    private List<GameObject> blackHoles = new List<GameObject>();
    private float curX = 0.0f, curY = 0.0f, curZ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        randStellarObjCount = Random.Range(400, maxStellarObj);
        this.generateNebula();
        stars.Add(player);
    }

    // Update is called once per frame
    void Update()
    {
        this.trackStarMass();
    }

    private void generateNebula()
    {
        do
        {
            stellarObjFactory();

            curY = Random.Range(-4.0f, 4.0f);
            curX = Random.Range(-1500, 1500);
            curZ = Random.Range(-1500, 1500);

            nebula[stellarObjCount].transform.localPosition = new Vector3(curX, curY, curZ);

            stellarObjCount++;
        }
        while (stellarObjCount < randStellarObjCount);
    }

    private void stellarObjFactory()
    {
        int choice = Random.Range(0, 100);

        if (choice <= 40)
        {
            nebula.Add(Instantiate(stellarObjPrefabs[0], gameObject.transform, false));
        }
        else if (choice > 40 && choice <= 85)
        {
            nebula.Add(Instantiate(stellarObjPrefabs[1], gameObject.transform, false));
        }
        else if (choice > 85 && choice <= 92)
        {
            nebula.Add(Instantiate(stellarObjPrefabs[2], gameObject.transform, false));
            stars.Add(nebula[nebula.Count-1]);
        }
        else if (choice > 96)
        {
            nebula.Add(Instantiate(stellarObjPrefabs[3], gameObject.transform, false));
            blackHoles.Add(nebula[nebula.Count-1]);
        }
        else
        {
            GameObject empty = new GameObject();
            nebula.Add(empty);
        }
    }

    private void trackStarMass()
    {
        if (stars.Count > 0)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                if (stars[i] != null && stars[i].GetComponent<IStellarObject>() != null && stars[i].GetComponent<IStellarObject>().getMass() >= 550)
                {
                    blackHoles.Add(Instantiate(stellarObjPrefabs[3], stars[i].transform.position, Quaternion.identity));
                    blackHoles[blackHoles.Count-1].GetComponent<BlackHole>().setMass(stars[i].GetComponent<Star>().getMass());

                    if (stars[i].GetComponent<MovementScript>() != null && camera != null)
                    {
                        Debug.Log(blackHoles.Count - 1);
                        camera.GetComponent<FirstPersonScript>().setPlayer(blackHoles[blackHoles.Count-1]);
                        blackHoles[blackHoles.Count-1].AddComponent<MovementScript>(); 
                    }
                    GameObject.Destroy(stars[i]);
                    stars[i] = new GameObject();
                }
            }
        }
    }
}
