namespace COURSE_ASH.View;

public partial class ImageUploadPage : ContentPage
{
    ImageUploader Uploader { get; set; }

    public ImageUploadPage()
    {
        InitializeComponent();
        Uploader = new ImageUploader();
    }

    private async void UploadImage_Clicked(object sender, EventArgs e)
    {
        var img = await Uploader.OpenMediaPickerAsync();

        var imageFile = await Uploader.Upload(img);

        Image_Upload.Source = ImageSource.FromStream(() =>
            Uploader.ByteArrayToStream(Uploader.StringToByteBase64(imageFile.ByteBase64))
        );
    }
}