
public enum Level : byte
{
    MainMenu,
    /// <summary>
    /// Talking with people, etc.
    /// </summary>
    Morning,
    /// <summary>
    /// Sending out scavengers.
    /// </summary>
    Noon,
    /// <summary>
    /// Kicking people out to fix overcrowding.
    /// </summary>
    Afternoon,
    /// <summary>
    /// Getting lore on the radio, leading into the door.
    /// </summary>
    Evening,
    /// <summary>
    /// Letting someone in or kicking them out.
    /// </summary>
    Door,
    /// <summary>
    /// The final epilogue about what you've done, etc.
    /// </summary>
    BorderEnd,
    /// <summary>
    /// Getting eaten by the bear, starving, dying on the way to the border, etc.
    /// </summary>
    EarlyEnd
}