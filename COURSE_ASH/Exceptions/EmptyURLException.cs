namespace COURSE_ASH.Exceptions;

public class EmptyURLException : Exception
{
    public override string Message
    {
        get
        {
            return ExceptionAlerts.EMPTY_URL;
        }
    }

    public EmptyURLException()
    {

    }

    public EmptyURLException(string message) : base(message)
    {

    }

    public EmptyURLException(string message, Exception inner)
        : base(message, inner)
    {

    }
}
