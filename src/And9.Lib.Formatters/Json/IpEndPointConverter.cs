﻿using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace And9.Lib.Formatters.Json;

public class IpEndPointConverter : JsonConverter<IPEndPoint>
{
    public override IPEndPoint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? raw = reader.GetString();
        return raw is null ? null : IPEndPoint.Parse(raw);
    }

    public override void Write(Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}