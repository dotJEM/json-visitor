using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DotJEM.Json.Visitor
{
    /// <summary>
    /// A Json.NET JToken Visitor class that traverses a JSON object and calls the apropriate visit methods for all nodes in
    /// the object.
    /// 
    /// This is not a true visitor aproach since that would require changes in JSON.NET, instead it bases it's dispatch
    /// of the <see cref="JToken.Type"/>.
    /// 
    /// DoAccept performs the dispatch and any type of dispatch can be implemented by overwriting this method.
    /// 
    /// The visitor accepts a Context object which can be used as an alternative to tracking during the visiting process.
    /// </summary>
    public abstract class JsonVisitor<TContext> : IJsonVisitor<TContext> where TContext : IJsonVisitorContext<TContext>
    {
        // JToken
        //  ├─ JContainer
        //  │   ├─ JArray
        //  │   ├─ JConstructor
        //  │   ├─ JObject
        //  │   └─ JProperty
        //  └─ JValue
        //      └─ JRaw

        protected virtual void Visit(JToken json, TContext context) { }

        protected virtual void Visit(JContainer json, TContext context) => Visit((JToken)json, context);

        protected virtual void Visit(JArray json, TContext context)
        {
            int i = 0;
            foreach (JToken token in json)
                token.Accept(this, context.Next(i++));
        }

        protected virtual void Visit(JConstructor json, TContext context) => Visit((JContainer) json, context);

        protected virtual void Visit(JObject json, TContext context)
        {
            foreach (JProperty property in json.Properties())
                Visit(property, context);
        }

        protected virtual void Visit(JProperty json, TContext context)
        {
            json.Value.Accept(this, context.Next(json.Name));
        }

        protected virtual void Visit(JValue json, TContext context) => Visit((JToken) json, context);

        protected virtual void Visit(JRaw json, TContext context) => Visit((JValue) json, context);

        public virtual void DoAccept(JToken json, TContext context)
        {
            switch (json.Type)
            {
                case JTokenType.Object:
                    Visit((JObject)json, context);
                    break;

                case JTokenType.Array:
                    Visit((JArray)json, context);
                    break;

                case JTokenType.Raw:
                    Visit((JRaw)json, context);
                    break;

                case JTokenType.Constructor:
                    Visit((JConstructor)json, context);
                    break;

                case JTokenType.Property:
                    Visit((JProperty)json, context);
                    break;

                case JTokenType.Comment:
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Null:
                case JTokenType.Undefined:
                case JTokenType.Date:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    Visit((JValue)json, context);
                    break;

                default:
                    throw new InvalidOperationException($"Invalid jtoken type '{json.Type}'.");
            }
        }
    }
}
