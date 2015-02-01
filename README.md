# Overview
Json2DataSet can convert the given json string to the ADO.NET DataSet, by using the [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) library. Json2DataSet can handle most of the json token structure and generates the ADO.NET DataTables and DataRelations correspondingly.

# How to Use
Simply use the following two lines of code to convert a json string to ADO.NET DataSet object.
> var json = File.ReadAllText(@"d:\test.json");
> 
> var dataSet = Json2DataSetConverter.Convert(json);




