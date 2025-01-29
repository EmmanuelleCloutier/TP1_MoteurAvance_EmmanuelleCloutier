#include <iostream>
#include "FileParser.h"
#include "Builder.h"
#include "Packager.h"

int main(int argc, char* argv[]) {
    if (argc < 3){
        std::cerr << "Usage : MyTool [CHEMIN DU UPROJECT] <command> [options]\n";
        return 1;
    }

    std::string command = argv[2];
    std::string projectPath = argv[1];

    if(command == "show-infos") {
        FileParser::ShowProjectInfo(projectPath);
    }
    else if(command == "build") {
        Builder::CompileProject(projectPath);
    }
    else if(command =="package" && argc == 4) {
        std::string packagePath = argv[3];
        Packager::PackageProject(projectPath, packagePath);
    }
    else {
        std::cerr << "Commande inconnue ou argements manquants dans commande. \n";
        return 1;
    }

    return 0;

}