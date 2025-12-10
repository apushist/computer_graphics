#ifndef MODEL_H
#define MODEL_H

#include <vector>
#include <map>
#include <tuple>
#include <string>
#include <glm/glm.hpp>

#ifdef _WIN32
#include <GL/glew.h>
#else
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

struct Planet {
    glm::vec3 position;
    float scale;
    float rotation;
    float orbitRadius;
    float orbitSpeed;
    float orbitAngle;        // Текущий угол на орбите
    float selfRotationSpeed;
};

extern std::vector<Planet> planets;

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

    glm::vec3 GetMin() const;
    glm::vec3 GetMax() const;
    glm::vec3 GetSize() const;

    void InitPlanets();
};

#endif // MODEL_H