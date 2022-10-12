using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, ITargeter, IStellarObject
{
    public float mass = 0.0f;
    private float prevMass = 0.0f, massMin = 50.0f, massMax = 500.0f;
    private float sizeScalar = 5.0f, size = 0.0f, influenceRadius = 3.0f, massInfluenceScalar = 0.002f, gravityForceScalar = 0.09f;
    private Material material;
    private SphereCollider[] colliders;
    private List<GameObject> registeredObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        colliders = this.gameObject.GetComponents<SphereCollider>();
        material = gameObject.GetComponent<Renderer>().material;
        this.mass = Random.Range(massMin, massMax);
    }

    // Update is called once per frame
    void Update()
    {
        this.size = (this.mass / this.sizeScalar);
        this.gameObject.transform.localScale = new Vector3(size, size, size);

        this.updateInfluenceRadius();
        
        colliders[0].radius = influenceRadius;
        colliders[1].radius = 0.25f;

        this.influenceObjects();
        this.consumeStellarObjects();
        this.updateColor();

        if (transform.position.y != 0.0f)
        {
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }

    private void updateColor()
    {
        if (this.mass < 167)
        {
            material.SetColor("_Color", new Color(0.9843137f, 0.7333333f, 0.6784314f));
            material.SetColor("_EmissionColor", new Color(0.9843137f, 0.7333333f, 0.6784314f) * 0.4f);
        }
        else if (this.mass > 167 && this.mass <= 334)
        {
            material.SetColor("_Color", new Color(0.9333333f, 0.5254902f, 0.5843138f));
            material.SetColor("_EmissionColor", new Color(0.9333333f, 0.5254902f, 0.5843138f) * 0.5f);
        }
        else if (this.mass > 334)
        {
            material.SetColor("_Color", new Color(0.2901961f, 0.4784314f, 0.5882353f));
            material.SetColor("_EmissionColor", new Color(0.2901961f, 0.4784314f, 0.5882353f));
        }
    }

    private void consumeStellarObjects()
    {
        for (int i = 0; i < registeredObjects.Count; i++)
        {
            if (registeredObjects[i] != null && registeredObjects[i].gameObject.GetComponent<IStellarObject>() != null && registeredObjects[i].gameObject.GetComponent<BlackHole>() == null && registeredObjects[i].gameObject.GetComponent<IStellarObject>().getMass() < this.mass)
            {
                if (Vector3.Distance(registeredObjects[i].gameObject.transform.position, transform.position) <= (this.colliders[1].radius * this.transform.localScale.x) + 5.0f)
                {
                    this.mass += registeredObjects[i].gameObject.GetComponent<IStellarObject>().getMass();
                    GameObject.Destroy(registeredObjects[i].gameObject);
                }
            }
        }
    }

    private void updateInfluenceRadius()
    {
        if (mass != prevMass)
        {
            float massDifference = Mathf.Abs(mass - prevMass);

            if (mass > prevMass)
            {
                influenceRadius += (massDifference * massInfluenceScalar);
            }
            else
            {
                influenceRadius -= (massDifference * massInfluenceScalar);
            }
            prevMass = mass;
        }
    }

    private void influenceObjects()
    {
        for (int i = 0; i < this.registeredObjects.Count; i++)
        {
            this.pull(this.registeredObjects[i]);
        }
    }

    private void pull(GameObject target)
    {
        if (target != null && !(target.GetComponent<IStellarObject>().getMass() > this.mass) && target.GetComponent<BlackHole>() == null)
        {
            target.GetComponent<Rigidbody>().AddForce(this.directionVector(target) * this.pullForce(this.distanceToTarget(target)), ForceMode.Force);
        }
    }

    private Vector3 directionVector(GameObject target)
    {
        Vector3 curPosition, objPosition;

        curPosition = transform.position;
        objPosition = target.transform.position;

        Vector3 direction = curPosition - objPosition;

        return direction;
    }

    private float distanceToTarget(GameObject target)
    {
        float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
        return distance;
    }

    private float pullForce(float distance)
    {
        float force = this.gravityForceScalar * (this.mass / (distance * distance));
        return force;
    }

    public void Register(GameObject target)
    {
        if (!(registeredObjects.Contains(target)) && target.GetComponent<IStellarObject>() != null)
        {
            //Debug.Log(target.gameObject.name + " Was Registered With " + gameObject.name);
            registeredObjects.Add(target);
        }
    }

    public void Unregister(GameObject target)
    {
        if (registeredObjects.Contains(target) && target.GetComponent<IStellarObject>() != null)
        {
            //Debug.Log(target.gameObject.name + " Was Unregistered With Player");
            registeredObjects.Remove(target);
        }
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

    public float getSize() { return this.size; }
    public float getMass() { return this.mass; }
}
