namespace DotJEM.Json.Visitor
{
    public interface IJsonVisitorContext<out TContext>
    {
        /// <summary>
        /// Gets the next context for an item in an Array.
        /// </summary>
        TContext Next(int index);

        /// <summary>
        /// Gets the next context for a property on an Object.
        /// </summary>
        TContext Next(string name);
    }
}