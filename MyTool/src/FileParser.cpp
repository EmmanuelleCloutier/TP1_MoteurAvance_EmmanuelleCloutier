#include "FileParser.h"
#include <iostream>
#include <fstream>
#include <sstream>

void FileParser::ShowProjectInfo(const std::string& projectPath) {
    std::ifstream file(projectPath);
    if (!file.is_open()) {
        std::cerr << "Impossible d'ouvrir le fichier " << projectPath << "\n";
        return;
    }

    std::stringstream buffer;
    buffer << file.rdbuf(); 
    std::string jsonContent = buffer.str();  

    file.close(); 

    size_t namePos = jsonContent.find("\"Name\"");
    size_t enginePos = jsonContent.find("\"EngineVersion\"");

    if (namePos != std::string::npos) {
        size_t start = jsonContent.find(":", namePos) + 2;
        size_t end = jsonContent.find("\"", start);
        std::cout << "Nom du jeu : " << jsonContent.substr(start, end - start) << "\n";
    } else {
        std::cerr << "Impossible de trouver le nom du projet.\n";
    }

    if (enginePos != std::string::npos) {
        size_t start = jsonContent.find(":", enginePos) + 2;
        size_t end = jsonContent.find("\"", start);
        std::cout << "🛠 Version Unreal Engine : " << jsonContent.substr(start, end - start) << "\n";
    } else {
        std::cerr << "Impossible de trouver la version Unreal.\n";
    }
}
