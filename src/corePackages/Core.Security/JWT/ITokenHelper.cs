using Core.Security.Entities;
using System.Collections.Generic;

namespace Core.Security.JWT;

public interface ITokenHelper<TUserId, TOperationClaimId>
{
    RefreshToken<TUserId> CreateRefreshToken(User<TUserId> user, string ipAddress);
    AccessToken CreateToken(User<TUserId> user, IList<OperationClaim<TOperationClaimId>> operationClaims);
}
