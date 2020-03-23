using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchWork
{
    public class Build
    {
        public void fetchRequest(string directory)
        {
            try
            {
                string logMessage = "";
                using (Repository repo = new Repository(directory))
                {
                    FetchOptions options = new FetchOptions();
                    options.CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = "hfugenGP",
                        Password = "71b08f48296d2100e2545fafcbf1ee8f75ffe84f"
                    });
                    foreach (Remote remote in repo.Network.Remotes)
                    {
                        IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                        Commands.Fetch(repo, remote.Name, refSpecs, options, logMessage);
                    }
                }
                Console.WriteLine(logMessage);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool pullRequest(string directory)
        {
            bool updated = false;
            try
            {
                using(Repository repo = new Repository(directory))
                {
                    PullOptions pullOptions = new PullOptions();
                    pullOptions.FetchOptions = new FetchOptions();
                    pullOptions.FetchOptions.CredentialsProvider = new CredentialsHandler(
                        (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = "hfugenGP",
                            Password = "71b08f48296d2100e2545fafcbf1ee8f75ffe84f"
                        }
                    );
                    pullOptions.MergeOptions = new MergeOptions();
                    pullOptions.MergeOptions.FastForwardStrategy = FastForwardStrategy.FastForwardOnly;
                    pullOptions.MergeOptions.FileConflictStrategy = CheckoutFileConflictStrategy.Normal;
                    Signature signature = new Signature(new Identity("MERGE_USER_NAME", "MERGE_USER_EMAIL"), DateTimeOffset.Now);
                    MergeResult mergeResult = Commands.Pull(repo, signature, pullOptions);
                    Console.WriteLine($"{mergeResult.Status}\n");
                    updated = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Console.WriteLine("New updates are available. Please run the patch tool.");
                resolveConflicts(directory);
                
            }

            return updated;
        }

        public string cloneRequest(string repository, string directory)
        {
            string path = "";
            try
            {
                var co = new CloneOptions();
                co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = "hfugenGP",
                    Password = "71b08f48296d2100e2545fafcbf1ee8f75ffe84f"
                };
                path = Repository.Clone(repository, directory, co);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return path;
        }

        public void resolveConflicts(string directory)
        {
            using (var repo = new Repository(directory))
            {
                Commands.Stage(repo, "*");
                repo.Stashes.Add(new Signature(new Identity("MERGE_USER_NAME", "MERGE_USER_EMAIL"), DateTimeOffset.Now), "Stash Test");
            }
            pullRequest(directory);
        }
    }
}
