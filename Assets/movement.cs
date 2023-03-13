using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class movement : MonoBehaviour
{
    public Transform car;
    float forward = 0;
    float left = 0;
    public bool gameEnd = false;
    string key;
    public static double reward = 0.0;
    Vector3[] defaultPos;
    Vector3[] defaultScale;
    Quaternion[] defaultRot;
    Transform[] models;
    Vector3 car_posi;
    Quaternion car_rot;


    public void GameReset()
    {
        GameObject[] tempModels = GameObject.FindGameObjectsWithTag("Props");        
        defaultPos = new Vector3[tempModels.Length];
        defaultScale = new Vector3[tempModels.Length];
        defaultRot = new Quaternion[tempModels.Length];
        models = new Transform[tempModels.Length];
        for (int i = 0; i < tempModels.Length; i++)
        {
            models[i] = tempModels[i].GetComponent<Transform>();
            defaultPos[i] = models[i].localPosition;
            defaultScale[i] = models[i].localScale;
            defaultRot[i] = models[i].localRotation;
        }        

        car.localPosition = new Vector3(7, 6, -1);
        car.localRotation = new Quaternion(0, 0, 0, 0);

    }

    void Start()
    {
        GameObject[] tempModels = GameObject.FindGameObjectsWithTag("Props");
        defaultPos = new Vector3[tempModels.Length];
        defaultScale = new Vector3[tempModels.Length];
        defaultRot = new Quaternion[tempModels.Length];
        models = new Transform[tempModels.Length];
        for (int i = 0; i < tempModels.Length; i++)
        {
            models[i] = tempModels[i].GetComponent<Transform>();
            defaultPos[i] = models[i].position;
            defaultScale[i] = models[i].localScale;
            defaultRot[i] = models[i].rotation;
        }
        car_posi = car.position;
        car_rot = car.rotation;
    }
    public void UpdateMovement(float forward, float left, bool brake)
    {
        this.forward = forward;
        this.left = left;

        if (brake)
        {
            this.forward = 0;
        }

        transform.Translate(0, 0, this.forward * Time.deltaTime);
        transform.Rotate(0, this.left * this.forward * Time.deltaTime, 0);
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            forward = 5;
            left = 0;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forward = -2;
            left = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left = -10;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            left = 10;
        }

        if (Input.GetKey(KeyCode.P))
        {
            forward = 0;
        }

        transform.Translate(0, 0, forward * Time.deltaTime);
        transform.Rotate(0, left * forward * Time.deltaTime, 0);
    }


    public float[] getPosition()
    {
        float[] pos = new float[3];
        pos[0] = Convert.ToSingle(car.localPosition.x);
        pos[1] = Convert.ToSingle(car.localPosition.y);
        pos[2] = Convert.ToSingle(car.localPosition.z);
        return pos;
    }

    void OnCollisionEnter(Collision collision_info)
    {
        if (collision_info.collider.name != "Base")
        {
            reward -= 2.0;
        }
    }

}
