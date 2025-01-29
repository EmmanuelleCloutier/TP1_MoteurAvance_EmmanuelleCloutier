using System;
using System.Diagnostics;  
using Newtonsoft.Json.Linq;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Exemple pour l'argument "show-infos"
        if (args.Length > 1 && args[1] == "show-infos")
        {
            ShowProjectInfo(args[0]);
        }
        // Exemple pour l'argument "build"
        else if (args.Length > 1 && args[1] == "build")
        {
            BuildProject(args[0]);
        }
        // Exemple pour l'argument "package"
        else if (args.Length > 1 && args[1] == "package")
        {
            PackageProject(args[0], args[2]);
        }
    }

    static void ShowProjectInfo(string projectPath)
    {
        try
        {
            // Lire le fichier UPROJECT
            string jsonContent = File.ReadAllText(projectPath);
            JObject project = JObject.Parse(jsonContent);

            // Extraire les informations du fichier JSON
            string gameName = project["ProjectName"]?.ToString();
            string unrealVersion = project["EngineVersion"]?.ToString();
            bool fromSource = project["FromSource"]?.ToString() == "true";
            var plugins = project["Plugins"]?.ToString();

            // Afficher les informations à l'utilisateur
            Console.WriteLine($"Game Name: {gameName}");
            Console.WriteLine($"Unreal Version: {unrealVersion}");
            Console.WriteLine(fromSource ? "From Source: Yes" : "From Source: No");
            Console.WriteLine($"Plugins: {plugins}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la lecture du fichier UPROJECT: {ex.Message}");
        }
    }

    static void BuildProject(string projectPath)
    {
        string ubtPath = "./Engine/Build/BatchFiles/Build.bat";  // Chemin vers UBT
        string arguments = $"\"{projectPath}\" Win64 Development";

        // Démarrer le processus pour compiler le projet
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = ubtPath,
            Arguments = arguments,
            RedirectStandardOutput = true,  // Pour récupérer la sortie du processus
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = Process.Start(startInfo);
        process.WaitForExit();  // Attendre la fin de la compilation

        Console.WriteLine("Build completed.");
    }

    static void PackageProject(string projectPath, string packagePath)
    {
        string uatPath = "./Engine/Build/BatchFiles/RunUAT.bat";  // Chemin vers UAT
        string arguments = $"BuildCookRun -project=\"{projectPath}\" -noP4 -platform=Win64 -clientconfig=Shipping -cook -allmaps -archive -archivedirectory=\"{packagePath}\"";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = uatPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = Process.Start(startInfo);
        process.WaitForExit();
        Console.WriteLine("Packaging completed.");
    }
}
