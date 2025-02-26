namespace Unbiased.Dashboard.Domain.Dto_s
{
    public class EngineConfigurationDto
    {
        public Guid Id { get; set; } 
        public string Url { get; set; } 
        public bool IsActive { get; set; } 
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; } 
        public string Language { get; set; } 
        public DateTime LastUpdatedTime { get; set; }
        public DateTime NextRunTime { get; set; } 
    }
}
