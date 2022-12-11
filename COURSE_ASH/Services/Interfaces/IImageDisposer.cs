namespace COURSE_ASH.Services.Interfaces;

public interface IImageDisposer
{
    int RecountImageLinks ();
    void DisposeImage(IImageDisposable item);
}
