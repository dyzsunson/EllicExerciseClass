using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game_Process_Object : MonoBehaviour {
    public enum GameState {
        NOT_STARTED = 0,
        READY = 1,
        RUNNING = 2,
        PREPARE_ENDING = 3,
        END = 4
    }

    protected GameState m_state = GameState.NOT_STARTED;
    public GameState Current_State {
        get {
            return this.m_state;
        }
    }

    public virtual void GameReady() {
        m_state = GameState.READY;
    }
    public virtual void GameStart() {
        m_state = GameState.RUNNING;
    }
    public virtual void GameEndBuffer() {
        m_state = GameState.PREPARE_ENDING;
    }
    public virtual void GameEnd() {
        m_state = GameState.END;
    }
}
