namespace BancolombiaStarter.Backend.Infrastructure.Extensions
{
    public static class PathExtension
    {
        // Method to find the base directory in a Docker container or local environment
        public static string FindSolutionBaseDirectory()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string solutionFolderName = "StackOverFlight.ErrorReport"; // Folder name of the solution

            // Attempt to locate the solution folder in the current directory hierarchy
            while (currentDirectory != null && !currentDirectory.EndsWith(solutionFolderName))
            {
                var parentDirectory = Directory.GetParent(currentDirectory);
                if (parentDirectory == null)
                {
                    break;
                }
                currentDirectory = parentDirectory.FullName;
            }

            // If running in Docker, the directory might be different
            if (currentDirectory == null)
            {
                // Adjust to point to the root directory in the container
                currentDirectory = "/app"; // Adjust this path if your Dockerfile uses a different directory
            }

            return currentDirectory;
        }
    }
}
