
namespace Json2DataSet
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class DataSetBuildVisitor : JsonVisitor
    {
        private const string BaseTableIdentifier = "#";
        private readonly Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();
        private readonly ConvertOptions options;
        private long rowNumber;
        private DataSet dataSet;

        internal DataSetBuildVisitor()
            : this(ConvertOptions.Default)
        { }

        internal DataSetBuildVisitor(ConvertOptions options)
        {
            this.options = options;
        }

        #region Private Static Methods
        private static string GetDictionaryKey(JToken token)
        {
            for (JToken c = token; c != null; c = c.Parent)
            {
                if (c is JProperty)
                {
                    return (c as JProperty).Name;
                }
            }
            return BaseTableIdentifier;
        }

        private static bool IsPrimitiveValue(JValue value)
        {
            return value.Type == JTokenType.Boolean ||
                value.Type == JTokenType.Date ||
                value.Type == JTokenType.Float ||
                value.Type == JTokenType.Guid ||
                value.Type == JTokenType.Integer ||
                value.Type == JTokenType.String ||
                value.Type == JTokenType.TimeSpan;
        }
        #endregion

        #region Private Methods
        private DataTable PrepareDataTable(JToken token)
        {
            var key = GetDictionaryKey(token);
            if (key == BaseTableIdentifier)
            {
                ++rowNumber;
            }
            DataTable dataTable = null;
            if (this.dataTables.ContainsKey(key))
            {
                dataTable = dataTables[key];
            }
            else
            {
                dataTable = new DataTable(key == BaseTableIdentifier ? options.BaseTableName : key);
                dataTable.Columns.Add(options.KeyColumnName);
                this.dataTables.Add(key, dataTable);
            }
            return dataTable;
        }
        #endregion

        #region Protected Methods
        protected override void VisitObject(JObject obj)
        {
            var dataTable = PrepareDataTable(obj);
            var row = dataTable.NewRow();
            row[options.KeyColumnName] = rowNumber;
            dataTable.Rows.Add(row);
        }

        protected override void VisitProperty(JProperty property)
        {
            var propertyValue = property.Value as JValue;
            if (propertyValue != null && propertyValue.Value != null)
            {
                if (IsPrimitiveValue(propertyValue))
                {
                    var key = GetDictionaryKey(property.Parent);

                    var dataTable = this.dataTables[key];

                    if (!dataTable.Columns.Contains(property.Name))
                    {
                        dataTable.Columns.Add(property.Name, propertyValue.Value.GetType());
                    }

                    var row = dataTable.Rows[dataTable.Rows.Count - 1];
                    row[property.Name] = propertyValue.Value;
                }
            }
        }

        protected override void VisitArray(JArray array)
        {
            if (!string.IsNullOrEmpty(array.Path)) // TODO: Performance improvement
            {
                var table = this.PrepareDataTable(array);
                foreach (JValue val in array.Children<JValue>())
                {
                    if (val != null && val.Value != null && IsPrimitiveValue(val))
                    {
                        if (!table.Columns.Contains(options.ArrayValueColumnName))
                        {
                            table.Columns.Add(options.ArrayValueColumnName, val.Value.GetType());
                        }

                        var row = table.NewRow();
                        row[options.KeyColumnName] = rowNumber;
                        row[options.ArrayValueColumnName] = val.Value;
                        table.Rows.Add(row);
                    }
                }
            }
        }
        #endregion

        #region Internal Methods
        internal DataSet DataSet
        {
            get
            {
                if (dataSet == null)
                {
                    dataSet = new DataSet();
                    foreach (var dataTable in this.dataTables.Values)
                    {
                        dataSet.Tables.Add(dataTable);
                    }
                    if (dataSet.Tables.Count > 0)
                    {
                        var parentTable = dataSet.Tables[options.BaseTableName];
                        var parentIndex = dataSet.Tables.IndexOf(parentTable);
                        for (int idx = 0; idx < dataSet.Tables.Count; idx++)
                        {
                            if (idx != parentIndex)
                            {
                                var childTable = dataSet.Tables[idx];
                                var relation = new DataRelation(string.Format("{0}.{1}", parentTable.TableName, childTable.TableName), 
                                    parentTable.Columns[options.KeyColumnName], 
                                    childTable.Columns[options.KeyColumnName]);
                                dataSet.Relations.Add(relation);
                            }
                        }
                    }
                }
                return dataSet;
            }
        }
        #endregion
    }
}
