namespace DotJEM.Json.Visitor
{
    public class NullJsonVisitorContext : IJsonVisitorContext<NullJsonVisitorContext>
    {
        public NullJsonVisitorContext Next(int index) => this;
        public NullJsonVisitorContext Next(string name) => this;
    }
}