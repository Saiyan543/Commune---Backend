namespace Main.Global.Library.GlobalExceptionHandling.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        { }
    }
}