namespace DotJEM.Json.Visitor
{
    /// <summary>
    /// Interface for the a context used by the <see cref="IJsonVisitor{TContext}"/> interface.
    /// <br/>
    /// This interface defines methods for altering the context as the Json is traversed.
    /// <br/>
    /// It is up to the implementor to choose if that changes a state in the context or if it returns a new context.
    /// </summary>
    /// <typeparam name="TContext">The actual type of the context it self, this is to guide in using the right context type at all times.</typeparam>
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