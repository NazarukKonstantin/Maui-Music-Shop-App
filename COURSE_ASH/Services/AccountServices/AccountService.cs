namespace COURSE_ASH.Services.AccountServices;

public class AccountService
{
    protected static bool DoPasswordsMatch(string passwordOne, string passwordTwo)
    {
        return passwordOne == SHA256HashComputer.ComputeSha256Hash(passwordTwo);
    }

    protected static string SetPasswordSHA256(string newPassword)
    {
        return SHA256HashComputer.ComputeSha256Hash(newPassword);
    }
    public static async Task RecordNewImage(FileResult image, double imageRotation, double imageScale, string login)
    {
        string imageLink = await ImageManager<AccountData>.LinkImageToStorageAsync(image);
        AccountData account = await DataStorageService<AccountData>.GetItemByAsync(nameof(AccountData.CurrentLogin), login);
        string oldImageLink = account.ImageLink;
        account.ImageLink = imageLink;
        account.ImageRotation = imageRotation;
        account.ImageScale = imageScale;
        if (imageLink != null && await ImageSeekingService.ShouldDelete<AccountData>(oldImageLink))
            await ImageManager<AccountData>.RemoveImageAsync(oldImageLink);
        await DataStorageService<AccountData>.UpdateItemAsync(account, nameof(AccountData.CurrentLogin), login);
    }

}
