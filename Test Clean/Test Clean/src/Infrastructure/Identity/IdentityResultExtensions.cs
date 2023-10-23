using Microsoft.AspNetCore.Identity;
using Test_Clean.Application.Common.Models;

namespace Test_Clean.Infrastructure.Identity;
public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}
