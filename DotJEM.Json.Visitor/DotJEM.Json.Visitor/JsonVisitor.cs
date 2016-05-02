using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DotJEM.Json.Visitor
{
    //NOTE: This is actually not a true visitor implementation.
    //      But it's the closest descriptive name I could find for now.
    public interface IJsonVisitor<in TContext> where TContext: IJsonVisitorContext<TContext>
    {
        void Accept(JToken json, TContext context);
    }

    public interface IJsonVisitorContext<out TContext>
    {
        TContext Index(int index);
        TContext Property(string name);
    }

    public class NullJsonVisitorContext : IJsonVisitorContext<NullJsonVisitorContext>
    {
        public NullJsonVisitorContext Index(int index)
        {
            return this;
        }

        public NullJsonVisitorContext Property(string name)
        {
            return this;
        }
    }

    public abstract class JValueVisitor<TContext> : JsonVisitor<TContext> where TContext : IJsonVisitorContext<TContext>
    {
        protected virtual void VisitComment(JToken json, TContext context) { }
        protected virtual void VisitInteger(JValue json, TContext context) { }
        protected virtual void VisitFloat(JValue json, TContext context) { }
        protected virtual void VisitString(JValue json, TContext context) { }
        protected virtual void VisitBoolean(JValue json, TContext context) { }
        protected virtual void VisitNull(JValue json, TContext context) { }
        protected virtual void VisitUndefined(JValue json, TContext context) { }
        protected virtual void VisitDate(JValue json, TContext context) { }
        protected virtual void VisitRaw(JRaw json, TContext context) { }
        protected virtual void VisitBytes(JValue json, TContext context) { }
        protected virtual void VisitGuid(JValue json, TContext context) { }
        protected virtual void VisitUri(JValue json, TContext context) { }
        protected virtual void VisitTimeSpan(JValue json, TContext context) { }

        protected override void Visit(JRaw json, TContext context)
        {
            VisitRaw(json, context);
        }

        protected override void Visit(JValue json, TContext context)
        {
            switch (json.Type)
            {
                case JTokenType.Comment:
                    VisitComment(json, context);
                    break;
                case JTokenType.Integer:
                    VisitInteger(json, context);
                    break;
                case JTokenType.Float:
                    VisitFloat(json, context);
                    break;
                case JTokenType.String:
                    VisitString(json, context);
                    break;
                case JTokenType.Boolean:
                    VisitBoolean(json, context);
                    break;
                case JTokenType.Null:
                    VisitNull(json, context);
                    break;
                case JTokenType.Undefined:
                    VisitUndefined(json, context);
                    break;
                case JTokenType.Date:
                    VisitDate(json, context);
                    break;
                case JTokenType.Bytes:
                    VisitBytes(json, context);
                    break;
                case JTokenType.Guid:
                    VisitGuid(json, context);
                    break;
                case JTokenType.Uri:
                    VisitUri(json, context);
                    break;
                case JTokenType.TimeSpan:
                    VisitTimeSpan(json, context);
                    break;
            }
        }
    }

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
                Accept(token, context.Index(i++));
        }

        protected virtual void Visit(JConstructor json, TContext context) => Visit((JContainer) json, context);

        protected virtual void Visit(JObject json, TContext context)
        {
            foreach (JProperty property in json.Properties())
                Visit(property, context);
        }

        protected virtual void Visit(JProperty json, TContext context)
        {
            Accept(json.Value, context.Property(json.Name));   
        }

        protected virtual void Visit(JValue json, TContext context) => Visit((JToken) json, context);

        protected virtual void Visit(JRaw json, TContext context) => Visit((JValue) json, context);

        public virtual void Accept(JToken json, TContext context)
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
