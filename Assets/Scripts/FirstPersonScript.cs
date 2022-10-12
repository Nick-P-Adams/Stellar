using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonScript : MonoBehaviour
{
    public GameObject player;
    public float distance = 65.0f, prevSize = 0.0f, sensitivityX = 1.5f;

    private float currentX = 0.0f, timeCount = 0.0f;
    private Vector3 camPosition, camPrevPosition;
    private Quaternion rotation, bodyYRotation, camYRotation;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, distance, 0) + this.player.transform.position;
        camPrevPosition = transform.position;
        camPosition = transform.position;
        prevSize = this.player.GetComponent<IStellarObject>().getSize();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        this.currentX += Input.GetAxis("Mouse X");
        this.detectChangeInSize();
    }

    private void LateUpdate()
    {
        if(this.player != null)
        {
            transform.position = Vector3.Lerp(camPrevPosition, camPosition, timeCount) + this.player.transform.position + new Vector3(0, distance, 0);

            rotation = Quaternion.Euler(90.0f, currentX * sensitivityX, 0.0f);
            camYRotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f); //turns to Z rotation when we slerp
            bodyYRotation = Quaternion.Euler(0.0f, player.transform.rotation.eulerAngles.y, 0.0f);

            transform.rotation = rotation;

            player.transform.rotation = Quaternion.Slerp(bodyYRotation, camYRotation, timeCount);
            timeCount = timeCount + Time.deltaTime;
        }
    }

    private void detectChangeInSize()
    {
        if (this.player != null && prevSize != player.GetComponent<IStellarObject>().getSize())
        {
            float distanceDifference = Mathf.Abs(prevSize - player.GetComponent<IStellarObject>().getSize());
            
            if (prevSize < player.GetComponent<IStellarObject>().getSize())
            {
                camPosition.y += distanceDifference;
            }
            else
            {
                camPosition.y -= distanceDifference;
            }

            prevSize = player.GetComponent<IStellarObject>().getSize();
        }
    }

    public void setPlayer(GameObject player)
    {
        this.player = player;
    }
}
