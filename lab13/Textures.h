#pragma once
#include <GL/glew.h>
#include <string>
#include <vector>

extern GLuint currentTextureID;     
extern std::string currentTextureName; 
extern std::vector<std::string> textureFiles;

GLuint CreateSimpleTexture();

GLuint LoadTextureFromFile(const std::string& path);

bool LoadTexture(const std::string& filename);

void ScanTexturesFolder();

void ShowTextureSelectionMenu();