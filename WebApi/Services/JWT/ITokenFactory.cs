namespace Services.JWT
{
    public interface ITokenFactory
    {
        string GenerateToken(int size= 32);
    }
}
