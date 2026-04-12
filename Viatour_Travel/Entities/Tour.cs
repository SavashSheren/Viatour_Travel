using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Viatour_Travel.Entities
{
    public class Tour
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }
        public string TourTitle { get; set; }
        public string TourDescription{ get; set; }
        public string CoverImageUrl{ get; set; }
        public string Badge{ get; set; }
        public int DayCount{ get; set; }
        public int Capacity{ get; set; }
        public decimal Price{ get; set; }
        public bool IsStatus{ get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public string Location { get; set; }
        public string MapLocationImageUrl { get; set; }
    }
}
