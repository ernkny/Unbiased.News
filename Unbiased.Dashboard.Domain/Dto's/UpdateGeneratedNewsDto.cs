namespace Unbiased.Dashboard.Domain.Dto_s
{
    public class UpdateGeneratedNewsDto
    {
        public string Id { get; set; }              
        public string Title { get; set; }       
        public string Detail { get; set; }        
        public int CategoryId { get; set; }     
        public DateTime CreatedTime { get; set; }
        public bool IsApproved { get; set; }   
        public bool IsActive { get; set; }      
        public string ImagePath { get; set; }
        public string Language { get; set; }
    }
}
