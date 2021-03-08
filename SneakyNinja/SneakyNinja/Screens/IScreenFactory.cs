// Obtained from GameArchitectureExample. Edited to fit with project. 

using System;

namespace SneakyNinja.Screens
{
    /// <summary>
    /// Defines an object that can create a screen when given its type.
    /// </summary>
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
