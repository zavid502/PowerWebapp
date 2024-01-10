using Data;

namespace Logic;

public class ApiRequestException<T>(ApiRequest<T> apiRequest) : Exception(message: apiRequest.Message)
{
    
}

public class ApiRequestException(ReturnlessApiRequest apiRequest) : Exception(message: apiRequest.Message)
{
    
}