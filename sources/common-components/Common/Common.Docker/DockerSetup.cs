using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ductus.FluentDocker.Model.Compose;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Impl;

namespace Common.Docker
{
    public class DockerSetup : IDisposable
    {
        private readonly DockerComposeCompositeService _dockerSetup;

        private DockerSetup(IHostService hostService, IList<string> composeFiles)
        {
            _dockerSetup = new DockerComposeCompositeService(hostService, new DockerComposeConfig
            {
                ComposeFilePath = composeFiles,
                ForceBuild = true,
                ForceRecreate = true,
                RemoveOrphans = true,
                StopOnDispose = true
            });
        }

        public void Up()
        {
            _dockerSetup.Start();
        }

        public void Down()
        {
            _dockerSetup.Stop();
        }

        private static IHostService FindLocalHost()
        {
            var hosts = new Hosts().Discover();
            return hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default") ?? throw new InvalidOperationException("Unable to find local Docker service");
        }

        public static DockerSetup Create(string composeFilesPath, IEnumerable<string> composeFilesNames)
        {
            var composeFiles = new List<string>();
            foreach (var composeFilesName in composeFilesNames)
            {
                composeFiles.Add(Path.Combine(composeFilesPath, composeFilesName));
            }

            return new DockerSetup(FindLocalHost(), composeFiles);
        }

        public void Dispose()
        {
            _dockerSetup?.Dispose();
        }
    }
}