using Newtonsoft.Json.Linq;

namespace DotJEM.Json.Visitor
{
    /// <summary>
    /// Adds a range of extra visited methods for JValues such as Integer, Float, String, Boolean etc.
    /// 
    /// Default implementation for all specific times routes back to <see cref="JsonVisitor`1{T}.Visit(JToken,TContext)"/>
    /// </summary>
    public abstract class JValueVisitor<TContext> : JsonVisitor<TContext> where TContext : IJsonVisitorContext<TContext>
    {
        protected virtual void VisitComment(JValue json, TContext context) => Visit((JToken) json, context);
        protected virtual void VisitInteger(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitFloat(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitString(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitBoolean(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitNull(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitUndefined(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitDate(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitRaw(JRaw json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitBytes(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitGuid(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitUri(JValue json, TContext context) => Visit((JToken)json, context);
        protected virtual void VisitTimeSpan(JValue json, TContext context) => Visit((JToken)json, context);

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
}