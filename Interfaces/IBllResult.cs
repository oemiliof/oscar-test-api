namespace prueba_oscar_api.Interfaces
{
    public interface IBllResult
    {
        string message { get; set; }
        object? data { get; set; }
        int statusCode { get; set; }
    }
}
