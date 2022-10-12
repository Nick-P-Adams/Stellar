using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour, IStellarObject
{
    public float mass = 0.0f, size = 0.0f, massMin = 40.0f, massMax = 100.0f;
    private Material material;
    public List<Texture> planetTextures;
    private float sizeScalar = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        float randX = Random.Range(-180, 180), randY = Random.Range(-180, 180), randZ = Random.Range(-180, 180);
        int textureChoice = Random.Range(0, 3);
        this.mass = Random.Range(massMin, massMax);
        this.size = (this.mass / this.sizeScalar);
        this.gameObject.transform.localScale = new Vector3(size, size, size);
        this.gameObject.transform.rotation = Quaternion.Euler(randX, randY, randZ);
        material = gameObject.GetComponent<Renderer>().material;
        material.SetTexture("_MainTex", planetTextures[textureChoice]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger != null && trigger.GetComponent<ITargeter>() != null)
        {
            trigger.gameObject.GetComponent<ITargeter>().Register(gameObject);
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (trigger != null && trigger.GetComponent<ITargeter>() != null)
        {
            trigger.gameObject.GetComponent<ITargeter>().Unregister(gameObject);
        }

    }

    public float getMass() { return this.size; }
    public float getSize() { return this.mass; }
}

