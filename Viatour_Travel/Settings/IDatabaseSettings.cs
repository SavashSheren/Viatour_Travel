namespace Viatour_Travel.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TourCollectionName{ get; set; }
        string TourImageCollectionName { get; set; }
        string TourPlanCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ReviewCollectionName { get; set; }
        public string ReservationCollectionName { get; set; } 

    }
}
