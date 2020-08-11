using System.Threading.Tasks;
using Blazor.Extensions;

namespace JuliaMazeGame.Services.Interfaces
{
    public interface IMazeRenderer
    {
        BECanvasComponent CanvasRef { get; set; }

        int CanvasWidth { get; }

        int CanvasHeight { get; }

        Task RenderMazeAsync();

    }
}