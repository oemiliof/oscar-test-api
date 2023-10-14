using prueba_oscar_api.Interfaces;

namespace prueba_oscar_api.DTO.Bll.Results
{
    public class ErrorResultDto : IBllResult
    {
        public string message { get; set; } = "Error"; //default value error
        public object? data { get; set; } = null; //default value null
        public int statusCode { get; set; } = -1; //default value -1 catch handled
    }
}
