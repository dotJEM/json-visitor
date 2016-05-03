using Newtonsoft.Json.Linq;

namespace DotJEM.Json.Visitor
{
    public static class JTokenAcceptExtension
    {
        /// <summary>
        /// Performs dispatch for the current token using the Visitors DoAccept method.
        /// </summary>
        public static T Accept<T, TVisitor>(this T self, TVisitor visitor) 
            where T : JToken where TVisitor : IJsonVisitor<NullJsonVisitorContext>
        {
            visitor.DoAccept(self, new NullJsonVisitorContext());
            return self;
        }

        /// <summary>
        /// Performs dispatch for the current token using the Visitors DoAccept method.
        /// </summary>
        public static T Accept<T, TContext>(this T self, IJsonVisitor<TContext> visitor, TContext context) 
            where T : JToken where TContext : IJsonVisitorContext<TContext>
        {
            visitor.DoAccept(self, context);
            return self;
        }
    }
}