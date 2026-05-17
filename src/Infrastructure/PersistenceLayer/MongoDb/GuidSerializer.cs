using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AmaniRobot.Infrastructure.PersistenceLayer.MongoDb;

//public sealed class GuidSerializer : SerializerBase<Guid>
//{
//    public override Guid Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//    {
//        var bsonType = context.Reader.GetCurrentBsonType();

//        return bsonType switch
//        {
//            BsonType.ObjectId => new Guid(context.Reader.ReadObjectId().ToString().Substring(0, 32).PadRight(32, '0')),
//            BsonType.String => Guid.Parse(context.Reader.ReadString()),
//            BsonType.Binary => new Guid(context.Reader.ReadBinaryData().Bytes),
//            _ => throw new BsonSerializationException($"Cannot deserialize Guid from BsonType '{bsonType}'.")
//        };
//    }

//    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Guid value)
//    {
//        context.Writer.WriteObjectId(new ObjectId(value.ToString().Substring(0, 24), 0));
//    }
//}