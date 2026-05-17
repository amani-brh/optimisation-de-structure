using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb.Serializers;

public sealed class ObjectIdToGuidSerializer : SerializerBase<Guid>
{
    public override Guid Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();

        return bsonType switch
        {
            BsonType.ObjectId => ConvertObjectIdToGuid(context.Reader.ReadObjectId()),
            BsonType.String => Guid.Parse(context.Reader.ReadString()),
            BsonType.Binary => new Guid(context.Reader.ReadBinaryData().Bytes),
            _ => throw new BsonSerializationException($"Cannot deserialize Guid from BsonType '{bsonType}'.")
        };
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Guid value)
    {
        // Serialize as string to preserve Guid
        context.Writer.WriteString(value.ToString());
    }

    private static Guid ConvertObjectIdToGuid(ObjectId objectId)
    {
        byte[] objectIdBytes = objectId.ToByteArray();
        byte[] guidBytes = new byte[16];

        // Copy first 12 bytes from ObjectId
        Array.Copy(objectIdBytes, 0, guidBytes, 0, 12);
        // Pad remaining 4 bytes with zeros
        for (int i = 12; i < 16; i++)
        {
            guidBytes[i] = 0;
        }

        return new Guid(guidBytes);
    }
}
