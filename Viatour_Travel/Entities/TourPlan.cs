using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Viatour_Travel.Entities
{
    public class TourPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourPlanId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }

        public int DayNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}