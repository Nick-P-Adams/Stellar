using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour, ITargeter, IStellarObject
{
    public float mass = 0.0f;
    private float prevMass = 0.0f, massMin = 30.0f, massMax = 450.0f;
    private float sizeScalar = 10.0f, size = 0.0f, influenceRadius = 3.0f, massInfluenceScalar = 0.004f, gravityForceScalar = 0.2f;
    private SphereCollider[] colliders;
    private List<GameObject> registeredObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        colliders = this.gameObject.GetComponents<SphereCollider>();
        
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

        if (transform.position.y != 0.0f)
        {
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }

    private void consumeStellarObjects()
    {
        for (int i = 0; i < registeredObjects.Count; i++)
        {
            if (registeredObjects[i] != null && registeredObjects[i].gameObject.GetComponent<IStellarObject>() != null)
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
        if (target != null && target.GetComponent<IStellarObject>() != null)
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

    public void setMass(float mass) { this.mass = mass; }
    public float getSize() { return this.size; }
    public float getMass() { return this.mass; }
}
