#include "Packager.h"
#include <iostream>
#include <cstdlib>

void Packager::PackageProject(const std::string& projectPath, const std::string& packagePath) {
    std::cout << "📦 Packaging du projet Unreal...\n";

    std::string uatCommand = "Engine\\Build\\BatchFiles\\RunUAT.bat BuildCookRun"
                             " -project=\"" + projectPath + "\""
                             " -noP4 -cook -stage -package -archive"
                             " -archivedirectory=\"" + packagePath + "\""
                             " -platform=Win64 -clientconfig=Development -serverconfig=Development";

    int result = std::system(uatCommand.c_str());

    if (result == 0) {
        std::cout << "Packaging réussi !\n";
    } else {
        std::cerr << "Erreur lors du packaging.\n";
    }
}
