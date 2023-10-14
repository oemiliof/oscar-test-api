using prueba_oscar_api.Interfaces;

namespace prueba_oscar_api.DTO.Bll.EconomicIndicatorsBll
{
    public class GetIndicatorEconomicoResult
    {
        public DateTime date { get; set; }
        public decimal amount { get; set; }
        public int indicator{ get; set; }
    }
}
