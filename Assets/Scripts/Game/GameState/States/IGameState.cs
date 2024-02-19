using System;

namespace Game
{
    public interface IGameState
    {
        void Run(float passedTime);
    }
}