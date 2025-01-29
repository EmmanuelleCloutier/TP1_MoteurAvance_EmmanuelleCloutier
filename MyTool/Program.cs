using System;
using System.Diagnostics;  
using Newtonsoft.Json.Linq;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 1 && args[1] == "show-infos")
        {
            ShowProjectInfo(args[0]);
        }
        else if (args.Length > 1 && args[1] == "build")
        {
            BuildProject(args[0]);
        }
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
        string unrealEnginePath = @"C:\Program Files\Epic Games\UE_5.4\Engine\Build\BatchFiles";
        string ubtPath = Path.Combine(unrealEnginePath, "Build.bat");

        // Vérifier si le fichier Build.bat existe
        if (!File.Exists(ubtPath))
        {
            Console.WriteLine("Build.bat introuvable.");
            return;
        }

        // Arguments pour le processus
        string arguments = $"\"{projectPath}\" Win64 Development";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = ubtPath,
            Arguments = arguments,
            RedirectStandardOutput = true,  
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = unrealEnginePath  // Assure-toi que le répertoire de travail est le bon
        };

        try
        {
            // Démarrer le processus
            Process process = Process.Start(startInfo);
            process.WaitForExit();  // Attendre que le processus se termine

            // Lire la sortie et les erreurs
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            Console.WriteLine("Sortie du processus : " + output);
            if (!string.IsNullOrEmpty(errors))
            {
                Console.WriteLine("Erreurs : " + errors);
            }

            Console.WriteLine("Build terminé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors du démarrage du processus de compilation : " + ex.Message);
        }
    }


    static void PackageProject(string projectPath, string packagePath)
    {
        string uatPath = @"C:\Program Files\Epic Games\UE_5.4\Engine\Build\BatchFiles\RunUAT.bat";

        
        if (!File.Exists(uatPath))
        {
            Console.WriteLine("RunUAT.bat introuvable.");
            return;
        }

       
        if (!Directory.Exists(packagePath))
        {
            Directory.CreateDirectory(packagePath);
        }

        
        string arguments = $"BuildCookRun -project=\"{projectPath}\" -platform=Win64 -clientconfig=Development -cook -allmaps -build -stage -archive -archivedirectory=\"{packagePath}\"";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = uatPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Path.GetDirectoryName(uatPath)
        };

        try
        {
            
            Process process = Process.Start(startInfo);

            
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            process.WaitForExit();  
            
            if (process.ExitCode != 0)
            {
                Console.WriteLine("Le processus a échoué avec le code de sortie : " + process.ExitCode);
            }

            Console.WriteLine("Sortie du processus : " + output);
            if (!string.IsNullOrEmpty(errors))
            {
                Console.WriteLine("Erreurs : " + errors);
            }

            Console.WriteLine("Packaging terminé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors du démarrage du processus de packaging : " + ex.Message);
        }
    }


}
