namespace ComplytekTest.API.Application.Services.External
{
    public interface IRandomStringGenerator
    {
        Task<string> GenerateAsync();
    }
}
