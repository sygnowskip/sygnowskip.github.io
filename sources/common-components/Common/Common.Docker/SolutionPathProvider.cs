using System;
using System.IO;
using System.Linq;

namespace Common.Docker
{
    public static class SolutionPathProvider
    {
        public static FileInfo GetSolutionPath(string solutionName)
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (directory != null)
            {
                var solutionFileInfo = directory.GetFiles($"{solutionName}.sln").SingleOrDefault();

                if (solutionFileInfo != null)
                {
                    return solutionFileInfo;
                }

                directory = directory.Parent;
            }

            throw new FileNotFoundException($"Solution {solutionName}.sln not found");
        }
    }
}