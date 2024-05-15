namespace SF_DSS.Data.Entities
{
    public class CropGrowthPlan
    {
        public string Crop {  get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public float Soil_Ph { get; set; }

        public string ToFormattedString()
        {
            return $"{Crop}: Temperature={Temperature}, Humidity={Humidity}, Soil pH={Soil_Ph}";
        }
    }
}
