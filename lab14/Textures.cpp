#include "Textures.h"
#include <SFML/Graphics.hpp>
#include <iostream>
#include <filesystem>
#include <algorithm>


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
