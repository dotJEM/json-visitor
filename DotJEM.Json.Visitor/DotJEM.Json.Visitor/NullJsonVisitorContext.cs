namespace DotJEM.Json.Visitor
{
    /// <summary>
    /// A empty implementation of the <see cref="IJsonVisitorContext{TContext}"/>.
    /// This just returns it self as the json is traversed and otherwise does nothing.
    /// </summary>
    public class NullJsonVisitorContext : IJsonVisitorContext<NullJsonVisitorContext>
    {
        /// <summary>
        /// Returns <see langword="this" /> context.
        /// </summary>
        public NullJsonVisitorContext Next(int index) => this;

        /// <summary>
        /// Returns <see langword="this" /> context.
        /// </summary>
        public NullJsonVisitorContext Next(string name) => this;
    }
}