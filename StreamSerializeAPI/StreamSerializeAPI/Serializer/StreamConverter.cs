using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamSerializeAPI.Serializer
{
    public class StreamConverter : JsonConverter<Stream>
    {
        public override Stream ReadJson(JsonReader reader, Type objectType, [AllowNull] Stream existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] Stream value, JsonSerializer serializer)
        {
            PipeReader reader = PipeReader.Create(value);
            
            while (true)
            {
                // read sync, if JsonConverter supports async WriteJson in future, we should replace it.
                var read = reader.ReadAsync().Result;

                if (read.IsCompleted)
                    break;

                // get buffer
                var buffer = read.Buffer;

                // if it does not have a length, just dont write any value and skip this iteration.
                if (buffer.Length == 0)
                    continue;

                // maybe in future, writeraw supports byte[], then we shouldn't use GetString
                var raw = Encoding.UTF8.GetString(buffer.ToArray());
                writer.WriteRaw(raw);

                // advance to next buffer
                reader.AdvanceTo(buffer.End);
            }

            // final step
            // if you don't write a raw value, the converter will write a null value.
            string? empty = null;
            writer.WriteRawValue(empty);
        }
    }
}
