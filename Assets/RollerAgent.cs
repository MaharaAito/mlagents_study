using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class RollerAgent : Agent
{
    public Transform target;
    Rigidbody rBody;

    public override void Initialize()
    {
        this.rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Roller Agentが床から落ちているとき
        if (this.transform.position.y < 0)
        {
            // 位置と速度をリセット
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
        }

        // Target の位置のリセット
        target.position = new Vector3(
            Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    // 観察取得時に呼ばれる (Visual Observation時は使用しない)
    //public override void CollectObservations(VectorSensor sensor)
    //{
    //    sensor.AddObservation(target.position);
    //    sensor.AddObservation(this.transform.position);
    //    sensor.AddObservation(rBody.velocity.x);
    //    sensor.AddObservation(rBody.velocity.z);
    //}

    // 行動実行時に呼ばれる
    public override void OnActionReceived(float[] vectorAction)
    {
        // RollerAgentに力を加える Continuous
        //Vector3 controlSignal = Vector3.zero;
        //controlSignal.x = vectorAction[0];
        //controlSignal.z = vectorAction[1];
        //rBody.AddForce(controlSignal * 10);

        // RollerAgentに力を加える Discrete
        Vector3 controlSignal = Vector3.zero;
        int action = (int)vectorAction[0];
        if (action == 1) controlSignal.z = 1.0f; // 上に移動
        if (action == 2) controlSignal.z = -1.0f; // 下に移動
        if (action == 3) controlSignal.x = -1.0f; // 左に移動
        if (action == 4) controlSignal.x = 1.0f; // 右に移動
        rBody.AddForce(controlSignal * 5);

        // RollerAgentがTargetの位置に到達したとき
        float distanceToTarget = Vector3.Distance(
            this.transform.position, target.position);
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        // RollerAgentが床から落ちた時
        if (this.transform.position.y < 0)
        {
            EndEpisode();
        }

    }

    public override void Heuristic(float[] actionsOut)
    {
        // Continuous
        //actionsOut[0] = Input.GetAxis("Horizontal");
        //actionsOut[1] = Input.GetAxis("Vertical");

        // Discrete
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow)) actionsOut[0] = 1;
        if (Input.GetKey(KeyCode.DownArrow)) actionsOut[0] = 2;
        if (Input.GetKey(KeyCode.LeftArrow)) actionsOut[0] = 3;
        if (Input.GetKey(KeyCode.RightArrow)) actionsOut[0] = 4;


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
