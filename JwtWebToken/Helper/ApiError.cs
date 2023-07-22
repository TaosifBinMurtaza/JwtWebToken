using System.Text.Json;

namespace JwtWebToken.Helper
{
    public class ApiError
    {
        private readonly long ErrorCode;
        private readonly string ErrorMessage;
        private readonly string ErrorDetails;


        public ApiError(long _ErrorCode, string _ErrorMessage, string _ErrorDetails = null)
        {
            ErrorCode= _ErrorCode;
            ErrorMessage= _ErrorMessage;
            ErrorDetails= _ErrorDetails;
        }

        public override string ToString()
        { 
            return JsonSerializer.Serialize(this);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
