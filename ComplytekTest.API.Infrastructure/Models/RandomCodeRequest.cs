namespace ComplytekTest.API.Infrastructure.Models
{
    public class RandomCodeRequest
    {
        public int CodesToGenerate { get; set; }
        public bool OnlyUniques { get; set; }
        public string[] CharactersSets { get; set; } = Array.Empty<string>();
    }
}
