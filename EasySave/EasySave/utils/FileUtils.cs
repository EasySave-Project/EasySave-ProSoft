namespace EasySave.utils
{
    public static class FileUtils
    {
        public static void CompleteCopyDirectory(string sourceDir, string targetDir)
        {
            VerifyDirectoryEmpty(sourceDir);
            VerifyDriveAvailable(targetDir);

            // créer le répertoire target s'il n'existe pas déjà
            Directory.CreateDirectory(targetDir);
            CopyFilesTo(sourceDir, targetDir);
            DeleteObsoleteFiles(sourceDir, targetDir);
            CopySubdirectoriesRecursively(sourceDir, targetDir);

        }

        private static void VerifyDirectoryExists(string sourceDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Le répertoire source n'existe pas ou n'a pas pu être trouvé: {sourceDir}");
            }
        }

        private static void VerifyDirectoryEmpty(string sourceDir)
        {
            VerifyDirectoryExists(sourceDir);

            if (!Directory.EnumerateFileSystemEntries(sourceDir).Any())
            {
                throw new DirectoryNotFoundException($"Le répertoire source est vide: {sourceDir}");
            }
        }

        private static void VerifyDriveAvailable(string targetDir)
        {
            if (!DriveInfo.GetDrives().Any(d => d.IsReady && targetDir.StartsWith(d.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DriveNotFoundException($"Le lecteur spécifié dans le chemin cible '{targetDir}' n'est pas disponible.");
            }
        }

        private static void CopyFilesTo(string sourceDir, string targetDir)
        {
            foreach (FileInfo file in new DirectoryInfo(sourceDir).GetFiles())
            {
                string tempPath = Path.Combine(targetDir, file.Name);
                file.CopyTo(tempPath, true);
            }
        }

        private static void DeleteObsoleteFiles(string sourceDir, string targetDir)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles().Select(f => f.Name).ToHashSet();
            foreach (FileInfo file in new DirectoryInfo(targetDir).GetFiles())
            {
                if (!sourceFiles.Contains(file.Name))
                {
                    file.Delete();
                }
            }
        }

        private static void CopySubdirectoriesRecursively(string sourceDir, string targetDir)
        {
            foreach (DirectoryInfo subdir in new DirectoryInfo(sourceDir).GetDirectories())
            {
                string tempPath = Path.Combine(targetDir, subdir.Name);
                Directory.CreateDirectory(tempPath); // Assure que le sous-répertoire cible existe
                CompleteCopyDirectory(subdir.FullName, tempPath);
            }

            DeleteObsoleteDirectories(sourceDir, targetDir);
        }

        private static void DeleteObsoleteDirectories(string sourceDir, string targetDir)
        {
            var sourceDirs = new DirectoryInfo(sourceDir).GetDirectories().Select(d => d.Name).ToHashSet();
            foreach (DirectoryInfo dir in new DirectoryInfo(targetDir).GetDirectories())
            {
                if (!sourceDirs.Contains(dir.Name))
                {
                    dir.Delete(true);
                }
            }
        }

    }
}
