


namespace Json2DataSet
{
    using Newtonsoft.Json.Linq;

    internal abstract class JsonVisitor
    {
        internal JsonVisitor()
        { }

        protected virtual void VisitArray(JArray array) { }

        protected virtual void VisitObject(JObject obj) { }

        protected virtual void VisitProperty(JProperty property) { }

        internal void Visit(JToken token)
        {
            switch(token.Type)
            {
                case JTokenType.Array:
                    VisitArray((JArray)token);
                    break;
                case JTokenType.Property:
                    VisitProperty((JProperty)token);
                    break;
                case JTokenType.Object:
                    VisitObject((JObject)token);
                    break;
            }
            foreach(var child in token.Children())
            {
                this.Visit(child);
            }
        }
    }
}
