namespace geckserver.Utils
{
    public class Response
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class ResponseCount
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Total { get; set; }
    }
} 