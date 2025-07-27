using Microsoft.AspNetCore.Http;

namespace Core.Application.Responses;

public class IResponse  
{
    
    public int Status { get; set; } = 200;
    public string Detail { get; set; } = "Success" ;

}
