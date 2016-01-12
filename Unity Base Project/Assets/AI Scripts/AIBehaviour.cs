using UnityEngine;
using System.Collections;

public enum AI_STATE { IDLE, PATROL, MOVE_TO_PLAYER, SLASH_LEFT, SLASH_RIGHT, SLASH_TOP, THRUST, GUARD, DODGE, WINDOW_OF_OPPORTUNITY };

public class AIBehaviour
{
    public AI_STATE state;
    public COMPLETION_STATE complete = COMPLETION_STATE.NOT_STARTED;
    public float floatData = 0.0f;
    public Vector3 positionData = Vector3.zero;
    public int iterationCount = -1;

    public AIBehaviour(AI_STATE _state)
    {
        state = _state;
        positionData = Vector3.zero;
    }

    public AIBehaviour(AI_STATE _state, float _timerData)
    {
        state = _state;
        floatData = _timerData;
    }

    public AIBehaviour(AI_STATE _state, Vector3 _positionData, float _stoppingDistance)
    {
        state = _state;
        positionData = _positionData;
        floatData = _stoppingDistance;
    }

    public void SetIterationCount(int _iterationCount)
    {
        iterationCount = _iterationCount;
    }

    public void DecrementIteration()
    {
        iterationCount--;
    }
}
