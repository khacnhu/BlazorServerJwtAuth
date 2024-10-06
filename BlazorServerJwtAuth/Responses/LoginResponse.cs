namespace BlazorServerJwtAuth.Responses
{
    public record LoginResponse(bool Flag = false, string Message = null!, string JWTToken = null!);

}
