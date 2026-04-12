using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Viatour_Travel.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String CategoryId { get; set; }
        public String CategoryName { get; set; }
        public bool CategoryStatus { get; set; }
    }
}
