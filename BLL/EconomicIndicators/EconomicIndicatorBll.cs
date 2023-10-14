using prueba_oscar_api.DTO.Bll.EconomicIndicatorsBll;
using prueba_oscar_api.DTO.Bll.Results;
using prueba_oscar_api.Interfaces;
using System.Xml.Linq;
using WsIndicadoresEconomicos;

namespace prueba_oscar_api.BLL.EconomicIndicators
{
    public class EconomicIndicatorBll
    {
        // Asynchronous method to retrieve data
        public async Task<IBllResult> GetIndicator()
        {
            try
            {
                // Create an instance of the web service client
                var client = new wsindicadoreseconomicosSoapClient(wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap);

                // Get today's date in the "dd/MM/yyyy" format
                var today = DateTime.Now.ToString("dd/MM/yyyy");

                // Create a request for the "ObtenerIndicadoresEconomicosXML" operation
                var request = new ObtenerIndicadoresEconomicosXMLRequest
                {
                    Indicador = "317",
                    FechaInicio = today, // Use today's date
                    FechaFinal = today, // Use today's date
                    Nombre = "Test",
                    SubNiveles = "n",
                    CorreoElectronico = "user_test2023@yopmail.com",
                    Token = "12E4SE0P2O"
                };

                // Call the operation and get the response
                var response = await client.ObtenerIndicadoresEconomicosXMLAsync(request);

                // Access the result as an XML string
                string result = response.ObtenerIndicadoresEconomicosXMLResult;

                // Check if the result contains errors
                if (string.IsNullOrWhiteSpace(result) || result.Contains("<ERROR>"))
                {
                    return new ErrorResultDto()
                    {
                        message = "Error getting Info from BCCR. Code 002",
                        statusCode = -2,
                    };
                }

                // Check if the result is not valid XML
                if (!IsValidXml(result))
                {
                    return new ErrorResultDto()
                    {
                        message = "Error getting Info from BCCR, invalid XML. Code 003",
                        statusCode = -3,
                    };
                }

                // Parse the XML
                var doc = XDocument.Parse(result);

                // Find the NUM_VALOR element
                var numValorElement = doc.Descendants("NUM_VALOR").FirstOrDefault();
                // Find the indicator element
                var indicatorElement = doc.Descendants("COD_INDICADORINTERNO").FirstOrDefault();
                // Find the DES_FECHA element
                var dateElement = doc.Descendants("DES_FECHA").FirstOrDefault();

                // Check if NUM_VALOR is null or cannot be parsed as decimal
                if (numValorElement == null || !decimal.TryParse(numValorElement.Value, out decimal numValue))
                {
                    return new ErrorResultDto()
                    {
                        message = "Error getting Info from BCCR, invalid amount. Code 004",
                        statusCode = -4,
                    };
                }

                // Check if NUM_VALOR is greater than 0
                if (numValue <= 0)
                {
                    return new ErrorResultDto()
                    {
                        message = "Error getting Info from BCCR, invalid amount (under 0). Code 005",
                        statusCode = -5,
                    };
                }

                // Check if indicator element is null or cannot be parsed as int
                if (indicatorElement == null || !int.TryParse(indicatorElement.Value, out int indicatorValue))
                {
                    return new ErrorResultDto()
                    {
                        message = "Error getting Info from BCCR, invalid indicator. Code 006",
                        statusCode = -6,
                    };
                }

                // Check if DES_FECHA is null or cannot be parsed as DateOnly
                if (dateElement == null || !DateTime.TryParse(dateElement.Value, out DateTime dateValue))
                {
                    return new ErrorResultDto()
                    {
                        message = "Error getting Info from BCCR, invalid date. Code 007",
                        statusCode = -7,
                    };
                }

                // Return a success result with parsed data
                return new SuccessResultDto()
                {
                    message = "Success",
                    data = new GetIndicatorEconomicoResult
                    {
                        indicator = indicatorValue,
                        amount = numValue,
                        date = dateValue.Date
                    },
                    statusCode = 0
                };
            }
            catch (Exception ex)
            {
                // Handle any unhandled exceptions and return an error result
                return new ErrorResultDto()
                {
                    message = "Internal Error" + ex.Message + ". Code 001",
                    statusCode = -1,
                };
            }
        }

        // Function to validate if a string is valid XML
        static bool IsValidXml(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
