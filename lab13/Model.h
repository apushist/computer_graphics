#pragma once
#include <GL/glew.h>
#include <vector>
#include <string>

struct Vertex {
    float x, y, z;
};

struct TexCoord {
    float u, v;
};

struct Normal {
    float x, y, z;
};

class Model {
private:
    GLuint VAO, VBO_Vertices, VBO_TexCoords, VBO_Normals, EBO;
    bool isInitialized;

    std::vector<Vertex> vertices;
    std::vector<TexCoord> texCoords;
    std::vector<Normal> normals;
    std::vector<unsigned int> indices;

public:
    Model();
    ~Model();

    bool LoadFromOBJ(const std::string& filename);
    void Draw() const;
    bool IsInitialized() const { return isInitialized; }

private:
    void SetupBuffers();
};