﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DataSetDeserializer.cs                                  |
|                                                          |
|  DataSetDeserializer class for C#.                       |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Data;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class DataSetDeserializer : Deserializer<DataSet> {
        private static DataSet Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            DataSet dataset = new DataSet();
            reader.AddReference(dataset);
            var deserializer = Deserializer<DataTable>.Instance;
            for (int i = 0; i < count; ++i) {
                dataset.Tables.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return dataset;
        }
        public override DataSet Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new DataSet();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
