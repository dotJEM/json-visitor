using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DotJEM.Json.Visitor.Test
{
    [TestFixture]
    public class JsonVisitorTest
    {
        [Test]
        public void Visit_String_Some()
        {
            JToken token = JToken.Parse("{ name: 'String', other: 42 }");

            JsonTestVistor visitor = new JsonTestVistor();
            visitor.Accept(token, new JsonTestVistorContext());
            
            Assert.That(visitor, 
                Has.Count.EqualTo(2)
                & Has.Exactly(1).EqualTo("   > name = String")
                & Has.Exactly(1).EqualTo("   > other = 42"));
        }
    }

    public class JsonTestVistor : JsonVisitor<JsonTestVistorContext>, IEnumerable<string>
    {
        private int indent;
        private readonly List<string> lines = new List<string>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return lines.GetEnumerator();
        }

        protected override void Visit(JToken json, JsonTestVistorContext context)
        {
            AppendLine($"{new string(' ', indent*2)} > {context.Name} = {json}");
            base.Visit(json, context);
        }

        private void AppendLine(string line)
        {
            lines.Add(line);
        }

        protected override void Visit(JArray json, JsonTestVistorContext context)
        {
            indent++;
            base.Visit(json, context);
            indent--;
        }

        protected override void Visit(JObject json, JsonTestVistorContext context)
        {
            indent++;
            base.Visit(json, context);
            indent--;
        }

        public int Count => lines.Count;
        public override string ToString() => lines.Aggregate(new StringBuilder(), (b, s) => b.AppendLine(s)).ToString();
    }

    public class JsonTestVistorContext : IJsonVisitorContext<JsonTestVistorContext>
    {
        public int Idx { get; set; }
        public string Name { get; set; }

        public JsonTestVistorContext Index(int index)
        {
            Idx = index;
            return this;
        }

        public JsonTestVistorContext Property(string name)
        {
            Name = name;
            return this;
        }
    }
}
