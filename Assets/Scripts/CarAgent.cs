using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    private movement carMovement;
    float Timeleft = 30f;
    int countOkParking = 0;
    int countBound = 0;
    private Vector3 lastPos = Vector3.zero;
    public Transform car;
    float forward = 0;
    float left = 0;
    bool gameEnd = false;
    string key;
    public static double reward = 0.0;
    Vector3[] defaultPos;
    Vector3[] defaultScale;
    Quaternion[] defaultRot;
    Transform[] models;
    Vector3 car_posi;
    Quaternion car_rot;


    public override void Initialize()
    {
        carMovement = GetComponent<movement>();
    }
    public override void OnEpisodeBegin()
    {
        carMovement.GameReset();
        Timeleft = 30f;
        countOkParking = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);

        if (lastPos == transform.localPosition)
        {
            AddReward(-0.1f);
            Debug.Log("Reward minus STAY!!");
        }

        Timeleft -= Time.deltaTime;
        AddReward(-0.01f);
    }

    public float distance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow((x1 - x2), 2) + Mathf.Pow((y1 - y2), 2));
    }

    public void Update()
    {


        lastPos = transform.localPosition;

        float zfinish = 4.01f;
        float xfinish = 11.55f;

        if (Timeleft < 0)
        {
            carMovement.GameReset();
            EndEpisode();
        }

        float[] pos = carMovement.getPosition();

        //bound
        if (transform.localPosition.x >= 3 && transform.localPosition.x <= 5 || transform.localPosition.z >= 8 && transform.localPosition.z <= 12)
        {
            if (countBound < 10)
            {
                AddReward(0.1f);
                countBound += 1;
            }
            else
            {
                AddReward(-0.05f);
                Debug.Log("STAY Bound Parking Position!!");
            }
            
        }

        // intermediate reward
        if (transform.localPosition.x >= 3 && transform.localPosition.x <= 4 && transform.localPosition.z >= 10 && transform.localPosition.z <= 12)
        {
            if (countOkParking < 10)
            {
                AddReward(10f);
                countOkParking += 1;
                Debug.Log("OK Parking Position");
            }
            else
            {
                AddReward(-0.5f);
                Debug.Log("STAY OK Parking Position!!");
            }
        }

        // won game
        if (transform.localPosition.x >= 4 && transform.localPosition.x <= 5 && transform.localPosition.z >= 11 && transform.localPosition.z <= 12)
        {
            Debug.Log(transform.localRotation.eulerAngles.y);
            if (transform.localRotation.eulerAngles.y >= 89 && transform.localRotation.eulerAngles.y <= 91)
            {
                AddReward(100f);
                Debug.Log("Perfect Parking Position Backward");
                carMovement.GameReset();
                EndEpisode();
            }
            else if (transform.localRotation.eulerAngles.y >= 268 && transform.localRotation.eulerAngles.y <= 272)
            {
                AddReward(100f);
                Debug.Log("Perfect Parking Position Forward");
                carMovement.GameReset();
                EndEpisode();
            }
        }
        else
        {
            float fscore = 0.08f;
            float calz = fscore / (zfinish - pos[2]);
            AddReward(-(0.02f * distance(pos[0], pos[2], xfinish, zfinish)));
        }

    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        int forward = actions.DiscreteActions[0];
        int left = actions.DiscreteActions[1];
        int brake = actions.DiscreteActions[2];

        float Horizontal = 0f;
        float Vertical = 0f;
        bool Brake = false;

        switch (forward)
        {
            case 0: Horizontal = 5f; break;
            case 1: Horizontal = 0f; break;
            case 2: Horizontal = -5f; break;
        }
        switch (left)
        {
            case 0: Vertical = 10f; break;
            case 1: Vertical = 0f; break;
            case 2: Vertical = -10f; break;
        }
        switch (brake)
        {
            case 0: Brake = false; break;
            case 1: Brake = true; break;
        }

        carMovement.UpdateMovement(Horizontal, Vertical, Brake);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-5f);
            Debug.Log("Wall Hit!!");
        }
        else if (collision.gameObject.TryGetComponent<Props>(out Props props))
        {
            AddReward(-5f);
            Debug.Log("Props Hit!!");
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-1f);
            Debug.Log("Wall Stay!!");
        }
        else if (collision.gameObject.TryGetComponent<Props>(out Props props))
        {
            AddReward(-1f);
            Debug.Log("Props Hit!!");
        }
    }

}
