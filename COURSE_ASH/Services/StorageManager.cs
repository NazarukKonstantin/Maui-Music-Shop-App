namespace COURSE_ASH.Services;

public class StorageManager
{
    public static async Task<string> UploadImage()
    {
        string storageURL = "musicshop-725ec.appspot.com";
        FileResult result = await MediaPicker.PickPhotoAsync();
        var pickedImage = await result.OpenReadAsync();

        var storage = await new FirebaseStorage(storageURL)
            .Child($"{result.FileName}")
            .PutAsync(pickedImage);

        Console.WriteLine(storage);

        var ImageURL = await new FirebaseStorage(storageURL)
            .Child($"{result.FileName}")
            .GetDownloadUrlAsync();

        Console.WriteLine(ImageURL);

        return ImageURL;
    }

}
