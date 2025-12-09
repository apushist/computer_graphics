#ifndef MODEL_H
#define MODEL_H

#include <vector>
#include <map>
#include <tuple>
#include <string>
#ifdef _WIN32
#include <GL/glew.h>
#else
#include <OpenGL/gl3.h>
#endif

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
    unsigned int VAO, VBO_Vertices, VBO_TexCoords, VBO_Normals, EBO;
    bool isInitialized;

    std::vector<Vertex> vertices;
    std::vector<TexCoord> texCoords;
    std::vector<Normal> normals;
    std::vector<unsigned int> indices;

    void Cleanup();

public:
    Model();
    Model(Model&& other) noexcept;
    Model& operator=(Model&& other) noexcept;
    ~Model();

    void Clear();
    void SetupBuffers();
    bool LoadFromOBJ(const std::string& filename);

    void SetData(const std::vector<Vertex>& verts,
        const std::vector<TexCoord>& texs,
        const std::vector<Normal>& norms,
        const std::vector<unsigned int>& inds);

    void Draw() const;

    bool IsInitialized() const { return isInitialized; }
};

#endif // MODEL_H