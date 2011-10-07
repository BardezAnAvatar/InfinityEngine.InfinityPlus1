namespace Bardez.Projects.InfinityPlus1.Utility
{
    public interface IDeepCloneable
    {
        /// <summary>Performs a deep copy of the object and returns another, separate instace of it.</summary>
        /// <returns>A deeply copied separate instance clone of the insance called from.</returns>
        IDeepCloneable Clone();
    }
}