using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllicShotGenerator : Game_Process_Object {
    
    public EllicTarget TargetPrefab;
    public EllicTarget BossPrefab;

    public EllicTarget StartObject;

    Transform[] m_targetPosition_array;
    Transform m_hide_position;
    EllicTarget[] m_target_array;
    int m_num = 0;

    float m_time;
    float[] m_next_time = new float[] { 6.0f, 3.0f, 2.5f};
    float[] m_scaler = new float[] { 0.9f, 0.9f, 0.9f };

    int m_difficulty_level = 0;


    public override void GameReady() {
        base.GameReady();

        m_difficulty_level = (int)SceneController.context.CurrentLevel.Difficulty;
        m_time = m_next_time[m_difficulty_level] - 2.0f;

        StartObject.gameObject.SetActive(true);
    }

    void Start() {
        // base.Start();
        m_hide_position = this.transform.Find("Hide_Position");
        m_targetPosition_array = new Transform[m_hide_position.childCount];
        m_target_array = new EllicTarget[m_hide_position.childCount];

        for (int i = 0; i < m_hide_position.childCount; i++)
            m_targetPosition_array[i] = m_hide_position.GetChild(i);
    }

    void Update() {
        // base.Update();
        if (m_state == GameState.RUNNING) {
            m_time += Time.deltaTime;

            if (m_time > m_next_time[m_difficulty_level]) {
                m_time = 0.0f;
                Create_Target();
            }
        }
    }

    public EllicTarget Create_Target() {
        int num = 0;

        bool has_empty = false;
        foreach (EllicTarget target in m_target_array) {
            if (target == null) {
                has_empty = true;
                break;
            }
        }

        if (has_empty == false)
            return null;

        do {
            num = Random.Range(0, m_targetPosition_array.Length);
        } while (m_target_array[num] != null);

        m_num++;
        if (m_num % 5 == 0) {
            this.SpeedUp();
            return Create_Target(num, BossPrefab);
        }
        else
            return Create_Target(num, TargetPrefab);
    }

    void SpeedUp() {
        m_next_time[m_difficulty_level] *= m_scaler[m_difficulty_level];
    }

    public EllicTarget Create_Target(int _position, EllicTarget _prefab) {
        EllicTarget target = Instantiate(_prefab);
        target.transform.position = m_targetPosition_array[_position].position;
        target.transform.LookAt(Camera.main.transform.position);
        m_target_array[_position] = target;
        return target;
    }
    
}
