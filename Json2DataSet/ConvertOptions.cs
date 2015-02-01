

namespace Json2DataSet
{

    /// <summary>
    /// Represents the convert options used for converting the Json string to ADO.NET dataset.
    /// </summary>
    public sealed class ConvertOptions
    {
        #region Private Constants
        private const string DefaultBaseTableName = "#";
        private const string DefaultKeyColumnName = "_ref_id";
        private const string DefaultArrayValueColumnName = "Value";
        #endregion

        #region Pubic Fields        
        /// <summary>
        /// The default convert options.
        /// </summary>
        public static readonly ConvertOptions Default = new ConvertOptions(DefaultBaseTableName, DefaultKeyColumnName, DefaultArrayValueColumnName);
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertOptions"/> class.
        /// </summary>
        public ConvertOptions()
            : this(Default.BaseTableName, Default.KeyColumnName, Default.ArrayValueColumnName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertOptions"/> class.
        /// </summary>
        /// <param name="baseTableName">Name of the base table.</param>
        /// <param name="keyColumnName">Name of the key column that will be used as the relationship field between the tables.</param>
        /// <param name="arrayValueColumnName">Name of the array value column.</param>
        public ConvertOptions(string baseTableName, string keyColumnName, string arrayValueColumnName)
        {
            this.BaseTableName = baseTableName;
            this.KeyColumnName = keyColumnName;
            this.ArrayValueColumnName = arrayValueColumnName;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the base table.
        /// </summary>
        /// <value>
        /// The name of the base table.
        /// </value>
        public string BaseTableName { get; set; }

        /// <summary>
        /// Gets or sets the name of the key column.
        /// </summary>
        /// <value>
        /// The name of the key column that will be used as the relationship field between the tables.
        /// </value>
        public string KeyColumnName { get; set; }

        /// <summary>
        /// Gets or sets the name of the array value column.
        /// </summary>
        /// <value>
        /// The name of the array value column.
        /// </value>
        public string ArrayValueColumnName { get; set; }
        #endregion
    }
}
