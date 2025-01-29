#include "Builder.h"
#include <iostream>
#include <cstdlib>

void Builder::CompileProject(const std::string& projectPath) {
    std::cout << "🔨 Compilation du projet Unreal...\n";

    std::string ubtCommand = "Engine\\Build\\BatchFiles\\RunUAT.bat BuildCookRun"
                             " -project=\"" + projectPath + "\""
                             " -noP4 -build -clientconfig=Development -serverconfig=Development -nocompileeditor";

    int result = std::system(ubtCommand.c_str());

    if (result == 0) {
        std::cout << "Compilation réussie !\n";
    } else {
        std::cerr << "Erreur lors de la compilation.\n";
    }
}
