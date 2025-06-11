using System;

namespace OtdamDarom.Web.Requests
{
    public class DealResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        
        public string ImageURL { get; set; }
    }

}