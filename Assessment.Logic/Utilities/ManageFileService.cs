namespace Assessment.Logic.Utilities
{
    public static class ManageFileService
    {
        public static void CopyFileToLocation(string currentFileName, string fileName, string sourcePath, string targetPath)
        {
            string sourceFile = Path.Combine(sourcePath, currentFileName);
            string destinationFile = Path.Combine(targetPath, fileName);

            Directory.CreateDirectory(targetPath);

            File.Copy(sourceFile, destinationFile, true);
        }

        public static void DeleteImage(string imageName)
        {
            var targetPath = @$"{Directory.GetCurrentDirectory()}{StringConstants.IMG_FOLDER}";
            string filePath = Path.Combine(targetPath, imageName);

            File.Delete(filePath);
        }
    }
}
