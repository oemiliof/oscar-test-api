using prueba_oscar_api.Interfaces;

namespace prueba_oscar_api.DTO.Bll.Results
{
    public class SuccessResultDto : IBllResult
    {
        public string message { get; set; } = "Success";// default Success
        public object? data { get; set; } = null;//default null
        public int statusCode { get; set; } = 0; //default value 0
    }
}
