namespace terminus.shared.models
{
    public class AttachmentViewModel
    {
        public string id { get; set; }
        public string displayName { get; set; } 
        public string fileName { get; set; }
        public string documentType { get; set; }
        public string url { get; set; }
        public bool isDeleted { get; set; }
    }
}