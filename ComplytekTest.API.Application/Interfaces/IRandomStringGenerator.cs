namespace ComplytekTest.API.Application.Interfaces
{
    public interface IRandomStringGenerator
    {
        Task<string> GenerateAsync();
    }
}
