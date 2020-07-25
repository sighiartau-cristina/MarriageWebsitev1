namespace BusinessModel.Contracts
{
    public class ResponseEntity<T>
    {
        public bool CompletedRequest { get; set; }

        public string ErrorMessage { get; set; }

        public T Entity { get; set; }
    }
}
