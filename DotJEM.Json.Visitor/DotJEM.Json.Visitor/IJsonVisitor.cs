using Newtonsoft.Json.Linq;

namespace DotJEM.Json.Visitor
{
    /// <summary>
    /// Basic interface for implementing a JToken visitor, the visitor specifies a context implementation.
    /// One implementation of the context could be used to track the location in the current full Json structure.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IJsonVisitor<in TContext> where TContext: IJsonVisitorContext<TContext>
    {
        void DoAccept(JToken json, TContext context);
    }

}