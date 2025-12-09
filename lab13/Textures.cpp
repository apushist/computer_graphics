#include "Textures.h"
#include <SFML/Graphics.hpp>
#include <iostream>
#include <filesystem>
#include <algorithm>

GLuint CreateSimpleTexture() {
    const int width = 256;
    const int height = 256;
    std::vector<unsigned char> imageData(width * height * 4);

    unsigned char r = 200;
    unsigned char g = 150;
    unsigned char b = 100;

    for (int y = 0; y < height; ++y) {
        for (int x = 0; x < width; ++x) {
            int index = (y * width + x) * 4;
            imageData[index] = r;
            imageData[index + 1] = g;
            imageData[index + 2] = b;
            imageData[index + 3] = 255;
        }
    }

    GLuint texID;
    glGenTextures(1, &texID);
    glBindTexture(GL_TEXTURE_2D, texID);

    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0,
        GL_RGBA, GL_UNSIGNED_BYTE, imageData.data());

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

    return texID;
}

GLuint LoadTextureFromFile(const std::string& path)
{
    sf::Image image;
    if (!image.loadFromFile(path)) {
        std::cerr << "Failed to load texture: " << path << std::endl;
        return 0;
    }
    image.flipVertically();

    GLuint texID;
    glGenTextures(1, &texID);
    glBindTexture(GL_TEXTURE_2D, texID);

    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, image.getSize().x, image.getSize().y, 0,
        GL_RGBA, GL_UNSIGNED_BYTE, image.getPixelsPtr());

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

    return texID;
}

void ScanTexturesFolder() {
    textureFiles.clear();
    try {
        if (!std::filesystem::exists("textures")) {
            std::cout << "Warning: 'textures/' folder not found. Creating it..." << std::endl;
            std::filesystem::create_directory("textures");
            return;
        }

        int count = 0;
        for (const auto& entry : std::filesystem::directory_iterator("textures")) {
            if (entry.path().extension() == ".png" || entry.path().extension() == ".jpg") {
                std::string filename = entry.path().filename().string();
                textureFiles.push_back(filename);
                count++;
            }
        }

        std::sort(textureFiles.begin(), textureFiles.end());

        if (count == 0) {
            std::cout << "No texture files found in 'textures/' folder." << std::endl;
        }
        else {
            std::cout << "Found " << count << " texture file(s)." << std::endl;
        }
    }
    catch (const std::exception& e) {
        std::cerr << "Error scanning textures folder: " << e.what() << std::endl;
    }
}

bool LoadTexture(const std::string& filename) {
    std::string fullPath = "textures/" + filename;
    GLuint texID = LoadTextureFromFile(fullPath);
    if (texID == 0) {
        std::cout << "Failed to load texture: " << filename << std::endl;
        return false;
    }

    currentTextureID = texID;
    currentTextureName = filename;

    std::cout << "Successfully loaded texture: " << filename << std::endl;
    return true;
}

void ShowTextureSelectionMenu() {
    ScanTexturesFolder();

    if (textureFiles.empty()) {
        std::cout << "\nNo textures available in 'textures/' folder." << std::endl;
        std::cout << "Please add PNG/JPG files to the 'textures/' folder." << std::endl;
        return;
    }

    std::cout << "\nSelect texture to load:" << std::endl;
    std::cout << "Available textures in 'textures/' folder:" << std::endl;

    for (size_t i = 0; i < textureFiles.size(); i++) {
        std::cout << "  " << i + 1 << ". " << textureFiles[i];
        if (textureFiles[i] == currentTextureName) {
            std::cout << " [CURRENT]";
        }
        std::cout << std::endl;
    }

    std::cout << "\nEnter texture number (1-" << textureFiles.size() << "), or 0 to cancel: ";

    int choice;
    if (!(std::cin >> choice)) {
        std::cin.clear();
        std::cin.ignore(10000, '\n');
        std::cout << "Invalid input. Please enter a number." << std::endl;
        return;
    }

    if (choice == 0) {
        std::cout << "Selection cancelled." << std::endl;
        return;
    }

    if (choice < 1 || choice > static_cast<int>(textureFiles.size())) {
        std::cout << "Invalid choice. Please enter a number between 1 and "
            << textureFiles.size() << "." << std::endl;
        return;
    }

    std::string selectedTexture = textureFiles[choice - 1];
    if (LoadTexture(selectedTexture)) {
        std::cout << "Texture switched to: " << selectedTexture << std::endl;
    }
}