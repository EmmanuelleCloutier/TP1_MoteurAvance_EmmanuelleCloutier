using System;
using System.Diagnostics;  
using Newtonsoft.Json.Linq;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        //pour declancher les bonnes fonctions selon les arguments entres 
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
            //lire contenu du UPROJECT
            string jsonContent = File.ReadAllText(projectPath);
            JObject project = JObject.Parse(jsonContent);
            
            //Prendre informations du fichier 
            string gameName = project["ProjectName"]?.ToString();
            string unrealVersion = project["EngineVersion"]?.ToString();
            bool fromSource = project["FromSource"]?.ToString() == "true";
            var plugins = project["Plugins"]?.ToString();
            
            //Affiche les demandes 
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
        //pour le chemin du Build.bat 
        string unrealEnginePath = @"C:\Program Files\Epic Games\UE_5.4\Engine\Build\BatchFiles";
        string ubtPath = Path.Combine(unrealEnginePath, "Build.bat");
        
        if (!File.Exists(ubtPath))
        {
            Console.WriteLine("Build.bat introuvable.");
            return;
        }
        
        //definir des arguments pour le processus 
        string arguments = $"\"{projectPath}\" Win64 Development";

        //creer un objet processStartInfo pour configurer le processus
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = ubtPath,
            Arguments = arguments,
            RedirectStandardOutput = true,  
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = unrealEnginePath  
        };

        try
        {
            //demarre le processus 
            Process process = Process.Start(startInfo);
            process.WaitForExit();  

            //Lecture de la reponse du processus 
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            //Afficher pour utilisateur 
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
        //Chemine vers le RunUAT.bat
        string uatPath = @"C:\Program Files\Epic Games\UE_5.4\Engine\Build\BatchFiles\RunUAT.bat";

        
        if (!File.Exists(uatPath))
        {
            Console.WriteLine("RunUAT.bat introuvable.");
            return;
        }

       //Verification si dossier build existe
        if (!Directory.Exists(packagePath))
        {
            Directory.CreateDirectory(packagePath);
        }

        //definir settings de packaging 
        string arguments = $"BuildCookRun -project=\"{projectPath}\" -platform=Win64 -clientconfig=Development -cook -allmaps -build -stage -archive -archivedirectory=\"{packagePath}\"";

        //Creer un object comme dans le build 
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
            //demarre le processus 
            Process process = Process.Start(startInfo);

            //lire sa reponse 
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            
            process.WaitForExit();  
            
            if (process.ExitCode != 0)
            {
                Console.WriteLine("Le processus a échoué avec le code de sortie : " + process.ExitCode);
            }

            //Affiche resultat sur la console 
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
