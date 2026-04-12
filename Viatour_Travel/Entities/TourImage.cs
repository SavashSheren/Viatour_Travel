using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Viatour_Travel.Entities
{
    public class TourImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourImageId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }

        public string ImageUrl { get; set; }
    }
}