using System.Text.RegularExpressions;
using Xunit;

namespace Umbracool.Repo.Tests;

public sealed class RepositorySmokeTests
{
    private static readonly string RepoRoot = GetRepositoryRoot();

    [Fact]
    public void RequiredRepositoryFilesExist()
    {
        string[] requiredFiles =
        [
            "README.md",
            "Dockerfile",
            "docker-compose.coolify.yml",
            ".env.example",
            ".github/workflows/ci.yml",
            ".github/pull_request_template.md"
        ];

        foreach (string relativePath in requiredFiles)
        {
            string fullPath = Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
            Assert.True(File.Exists(fullPath), $"Expected file '{relativePath}' to exist.");
        }
    }

    [Fact]
    public void EnvironmentExampleContainsRequiredKeys()
    {
        string envPath = Path.Combine(RepoRoot, ".env.example");
        string envContents = File.ReadAllText(envPath);

        string[] requiredKeys =
        [
            "SQL_SA_PASSWORD",
            "UMBRACO_DB_NAME",
            "UMBRACO_SITE_NAME",
            "UMBRACO_ADMIN_NAME",
            "UMBRACO_ADMIN_EMAIL",
            "UMBRACO_ADMIN_PASSWORD",
            "UMBRACO_TELEMETRY_LEVEL"
        ];

        foreach (string key in requiredKeys)
        {
            Assert.Contains($"{key}=", envContents);
        }
    }

    [Fact]
    public void DockerComposeIncludesCoreServicesAndVolumes()
    {
        string composePath = Path.Combine(RepoRoot, "docker-compose.coolify.yml");
        string composeContents = File.ReadAllText(composePath);

        Assert.Contains("web:", composeContents);
        Assert.Contains("sqlserver:", composeContents);
        Assert.Contains("umbraco_media:", composeContents);
        Assert.Contains("sql_data:", composeContents);
        Assert.Contains("ConnectionStrings__umbracoDbDSN", composeContents);
        Assert.Contains("Umbraco__CMS__Unattended__InstallUnattended", composeContents);
    }

    [Fact]
    public void ReadmeDocumentsCiBadgePlaceholdersAndWorkflow()
    {
        string readmePath = Path.Combine(RepoRoot, "README.md");
        string readmeContents = File.ReadAllText(readmePath);

        Assert.Contains("github.com/OWNER/REPO/actions/workflows/ci.yml", readmeContents);
        Assert.Contains("[![CI]", readmeContents);
        Assert.Contains("GitHub repository path", readmeContents);
    }

    [Fact]
    public void PullRequestTemplateMentionsValidationCommands()
    {
        string templatePath = Path.Combine(RepoRoot, ".github", "pull_request_template.md");
        string templateContents = File.ReadAllText(templatePath);

        Assert.Matches(new Regex(@"dotnet test", RegexOptions.IgnoreCase), templateContents);
        Assert.Matches(new Regex(@"docker compose -f docker-compose\.coolify\.yml config", RegexOptions.IgnoreCase), templateContents);
    }

    private static string GetRepositoryRoot()
    {
        string? current = AppContext.BaseDirectory;

        while (current is not null)
        {
            string candidate = Path.Combine(current, "Umbracool.sln");
            if (File.Exists(candidate))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName;
        }

        throw new DirectoryNotFoundException("Could not locate the repository root.");
    }
}
