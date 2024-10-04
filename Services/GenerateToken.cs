namespace SistemaOrdenes.Services
{
    public class GenerateToken
    {
        public static string Generate()
        {
            string token = Guid.NewGuid().ToString("N");
            return token;
        }
        public static string GenerateTempPass()
        {
            string tempPass = Guid.NewGuid().ToString("N");
            tempPass = HashSHA256.CSHA256(tempPass);
            return tempPass;
        }
    }
}
