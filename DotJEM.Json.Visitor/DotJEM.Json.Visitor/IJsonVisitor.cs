using Newtonsoft.Json.Linq;

namespace DotJEM.Json.Visitor
{
    public interface IJsonVisitor<in TContext> where TContext: IJsonVisitorContext<TContext>
    {
        void DoAccept(JToken json, TContext context);
    }

}