namespace PaParchar.Utils.Results
{
    public interface IResult<T>
    {
        public T Data { get; set; }
        public bool? Found { get; set; }
        public bool? Successful { get; set; }
        public string? Message { get; set; }
        public string? ExceptionMessage { get; set; }
    }
}
