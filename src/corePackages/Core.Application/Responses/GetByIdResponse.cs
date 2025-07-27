using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Responses;
public class GetByIdResponse<TResponse>: IResponse where TResponse : class
{
    public TResponse Data { get; set; } = default!;

    public GetByIdResponse(TResponse responses)
    {
        Data = responses;
    }
}
