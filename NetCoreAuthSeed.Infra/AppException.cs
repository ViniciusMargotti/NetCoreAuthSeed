namespace NetCoreAuthSeed.Infra
{
    public class AppException : Exception
    {
        public Enum CodigoErro { get; private set; }

        public AppException(string message, Enum erro) : base(message)
        {
            CodigoErro = erro;
        }

        public AppException(string message, AppException ex, Enum erro) : base(message, ex)
        {
            CodigoErro = erro;
        }
    }
}
