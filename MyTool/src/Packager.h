#ifndef PACKAGER_H
#define PACKAGER_H

#include <string>

class Packager {
public:
    static void PackageProject(const std::string& projectPath, const std::string& packagePath);
};

#endif
