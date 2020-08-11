using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuliaMazeGame.Services.Interfaces
{
    public interface IUserInputHandler
    {
        Task OnKeyPress(int key);

        void StartGameButtonPress();
    }
}
