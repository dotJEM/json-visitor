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
    [TestFixture, Category("Integration")]
    public class JsonVisitorTest
    {
        [Test]
        public void Visit_Simple_VisitsAllNodes()
        {
            JToken token = JToken.Parse("{ string: 'String', number: 42 }");

            NullTestVisitor visitor = new NullTestVisitor();
            token.Accept(visitor);

            Assert.That(visitor,
                Has.Exactly(1).EqualTo("string = String")
                & Has.Exactly(1).EqualTo("number = 42"));
        }

        [Test]
        public void Visit_WithObjects_VisitsAllNodes()
        {
            JToken token = JToken.Parse("{ string: 'String', obj: { child_string: 'Child String' } }");

            NullTestVisitor visitor = new NullTestVisitor();
            visitor.DoAccept(token, new NullJsonVisitorContext());

            Assert.That(visitor,
                Has.Exactly(1).EqualTo("string = String")
                & Has.Exactly(1).EqualTo("obj.child_string = Child String"));
        }

        [Test]
        public void Visit_WithSimpleArray_VisitsAllNodes()
        {
            JToken token = JToken.Parse("{ string: 'String', arr: ['Zero', 'One', 'Two'] }");

            NullTestVisitor visitor = new NullTestVisitor();
            visitor.DoAccept(token, new NullJsonVisitorContext());

            Assert.That(visitor,
                Has.Exactly(1).EqualTo("string = String")
                & Has.Exactly(1).EqualTo("arr[0] = Zero")
                & Has.Exactly(1).EqualTo("arr[1] = One")
                & Has.Exactly(1).EqualTo("arr[2] = Two")
                );
        }
    }

    [TestFixture, Category("Integration")]
    public class PathTrackerJsonVisitorContext
    {
        [Test]
        public void Visit_String_Some()
        {
            JToken token = JToken.Parse("{ name: 'String', other: 42 }");

            NullTestVisitor visitor = new NullTestVisitor();
            visitor.DoAccept(token, new NullJsonVisitorContext());

            Assert.That(visitor,
                Has.Count.EqualTo(2)
                & Has.Exactly(1).EqualTo("name = String")
                & Has.Exactly(1).EqualTo("other = 42"));
        }
    }

    public class NullTestVisitor : JsonVisitor<NullJsonVisitorContext>, IEnumerable<string>
    {
        private readonly List<string> lines = new List<string>();

        protected override void Visit(JToken json, NullJsonVisitorContext context)
        {
            lines.Add($"{json.Path} = {json}");
            base.Visit(json, context);
        }

        public int Count => lines.Count;
        public override string ToString() => lines.Aggregate(new StringBuilder(), (b, s) => b.AppendLine(s)).ToString();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<string> GetEnumerator() => lines.GetEnumerator();
    }
}
