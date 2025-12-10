#include "Model.h"
#include <glm/glm.hpp> 
#include <fstream>
#include <sstream>
#include <iostream>
#include <utility>
#include <map>
#include <tuple>

Model::Model() : isInitialized(false) {
    VAO = 0;
    VBO_Vertices = 0;
    VBO_TexCoords = 0;
    VBO_Normals = 0;
    EBO = 0;
}

Model::Model(Model&& other) noexcept
    : VAO(other.VAO), VBO_Vertices(other.VBO_Vertices),
    VBO_TexCoords(other.VBO_TexCoords), VBO_Normals(other.VBO_Normals),
    EBO(other.EBO), isInitialized(other.isInitialized),
    vertices(std::move(other.vertices)), texCoords(std::move(other.texCoords)),
    normals(std::move(other.normals)), indices(std::move(other.indices)) {

    other.VAO = 0;
    other.VBO_Vertices = 0;
    other.VBO_TexCoords = 0;
    other.VBO_Normals = 0;
    other.EBO = 0;
    other.isInitialized = false;
}

Model& Model::operator=(Model&& other) noexcept {
    if (this != &other) {
        Cleanup();
        VAO = other.VAO;
        VBO_Vertices = other.VBO_Vertices;
        VBO_TexCoords = other.VBO_TexCoords;
        VBO_Normals = other.VBO_Normals;
        EBO = other.EBO;
        isInitialized = other.isInitialized;

        vertices = std::move(other.vertices);
        texCoords = std::move(other.texCoords);
        normals = std::move(other.normals);
        indices = std::move(other.indices);

        other.VAO = 0;
        other.VBO_Vertices = 0;
        other.VBO_TexCoords = 0;
        other.VBO_Normals = 0;
        other.EBO = 0;
        other.isInitialized = false;
    }
    return *this;
}

Model::~Model() {
    Cleanup();
}

void Model::Cleanup() {
    if (VAO != 0) glDeleteVertexArrays(1, &VAO);
    if (VBO_Vertices != 0) glDeleteBuffers(1, &VBO_Vertices);
    if (VBO_TexCoords != 0) glDeleteBuffers(1, &VBO_TexCoords);
    if (VBO_Normals != 0) glDeleteBuffers(1, &VBO_Normals);
    if (EBO != 0) glDeleteBuffers(1, &EBO);

    VAO = 0;
    VBO_Vertices = 0;
    VBO_TexCoords = 0;
    VBO_Normals = 0;
    EBO = 0;
    isInitialized = false;
}

void Model::Clear() {
    Cleanup();
    vertices.clear();
    texCoords.clear();
    normals.clear();
    indices.clear();
}

void Model::SetupBuffers() {
    Cleanup();

    if (vertices.empty() || indices.empty()) {
        std::cerr << "Cannot setup buffers: no data!" << std::endl;
        return;
    }

    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO_Vertices);
    glGenBuffers(1, &EBO);

    if (!texCoords.empty()) glGenBuffers(1, &VBO_TexCoords);
    if (!normals.empty()) glGenBuffers(1, &VBO_Normals);

    glBindVertexArray(VAO);

    // Вершины
    glBindBuffer(GL_ARRAY_BUFFER, VBO_Vertices);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), vertices.data(), GL_STATIC_DRAW);
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)0);
    glEnableVertexAttribArray(0);

    // Текстурные координаты
    if (!texCoords.empty()) {
        glBindBuffer(GL_ARRAY_BUFFER, VBO_TexCoords);
        glBufferData(GL_ARRAY_BUFFER, texCoords.size() * sizeof(TexCoord), texCoords.data(), GL_STATIC_DRAW);
        glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, sizeof(TexCoord), (void*)0);
        glEnableVertexAttribArray(1);
    }

    // Нормали
    if (!normals.empty()) {
        glBindBuffer(GL_ARRAY_BUFFER, VBO_Normals);
        glBufferData(GL_ARRAY_BUFFER, normals.size() * sizeof(Normal), normals.data(), GL_STATIC_DRAW);
        glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, sizeof(Normal), (void*)0);
        glEnableVertexAttribArray(2);
    }

    // Индексы
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size() * sizeof(unsigned int), indices.data(), GL_STATIC_DRAW);

    glBindVertexArray(0);
    isInitialized = true;

    std::cout << "Model buffers setup: " << vertices.size() << " vertices, "
        << indices.size() / 3 << " triangles" << std::endl;
}

bool Model::LoadFromOBJ(const std::string& filename) {
    std::ifstream file(filename);
    if (!file.is_open()) {
        std::cerr << "Failed to open OBJ file: " << filename << std::endl;
        return false;
    }

    std::vector<Vertex> tempVertices;
    std::vector<TexCoord> tempTexCoords;
    std::vector<Normal> tempNormals;

    std::vector<Vertex> finalVertices;
    std::vector<TexCoord> finalTexCoords;
    std::vector<Normal> finalNormals;
    std::vector<unsigned int> finalIndices;

    std::map<std::tuple<int, int, int>, unsigned int> vertexMap;
    unsigned int currentIndex = 0;

    std::string line;
    int lineNum = 0;

    while (std::getline(file, line)) {
        lineNum++;
        if (line.empty() || line[0] == '#') continue;

        std::istringstream iss(line);
        std::string type;
        iss >> type;

        if (type == "v") {
            Vertex v;
            if (iss >> v.x >> v.y >> v.z) {
                tempVertices.push_back(v);
            }
        }
        else if (type == "vt") {
            TexCoord tc;
            if (iss >> tc.u >> tc.v) {
                tempTexCoords.push_back(tc);
            }
        }
        else if (type == "vn") {
            Normal n;
            if (iss >> n.x >> n.y >> n.z) {
                tempNormals.push_back(n);
            }
        }
        else if (type == "f") {
            std::vector<std::string> faceVertices;
            std::string vertexStr;

            while (iss >> vertexStr) {
                faceVertices.push_back(vertexStr);
            }

            if (faceVertices.size() >= 3) {
                for (size_t i = 1; i + 1 < faceVertices.size(); i++) {
                    std::vector<std::string> triangle = {
                        faceVertices[0],
                        faceVertices[i],
                        faceVertices[i + 1]
                    };

                    for (const auto& vertexStr : triangle) {
                        std::istringstream viss(vertexStr);
                        std::string vPart, tPart, nPart;
                        std::getline(viss, vPart, '/');
                        std::getline(viss, tPart, '/');
                        std::getline(viss, nPart, '/');

                        int vIdx = 0, vtIdx = 0, vnIdx = 0;

                        if (!vPart.empty()) vIdx = std::stoi(vPart);
                        if (!tPart.empty()) vtIdx = std::stoi(tPart);
                        if (!nPart.empty()) vnIdx = std::stoi(nPart);

                        auto key = std::make_tuple(vIdx, vtIdx, vnIdx);
                        auto it = vertexMap.find(key);
                        if (it != vertexMap.end()) {
                            finalIndices.push_back(it->second);
                        }
                        else {
                            if (vIdx > 0 && vIdx <= static_cast<int>(tempVertices.size())) {
                                finalVertices.push_back(tempVertices[vIdx - 1]);
                            }
                            else {
                                finalVertices.push_back({ 0.0f, 0.0f, 0.0f });
                            }

                            if (vtIdx > 0 && vtIdx <= static_cast<int>(tempTexCoords.size())) {
                                finalTexCoords.push_back(tempTexCoords[vtIdx - 1]);
                            }
                            else {
                                finalTexCoords.push_back({ 0.0f, 0.0f });
                            }

                            if (vnIdx > 0 && vnIdx <= static_cast<int>(tempNormals.size())) {
                                finalNormals.push_back(tempNormals[vnIdx - 1]);
                            }
                            else {
                                finalNormals.push_back({ 0.0f, 1.0f, 0.0f });
                            }

                            vertexMap[key] = currentIndex;
                            finalIndices.push_back(currentIndex);
                            currentIndex++;
                        }
                    }
                }
            }
        }
    }

    file.close();

    if (finalVertices.empty()) {
        std::cerr << "No vertices loaded from: " << filename << std::endl;
        return false;
    }
    vertices = std::move(finalVertices);
    texCoords = std::move(finalTexCoords);
    normals = std::move(finalNormals);
    indices = std::move(finalIndices);

    if (texCoords.size() != vertices.size()) {
        if (texCoords.empty()) {
            texCoords.resize(vertices.size(), { 0.0f, 0.0f });
        }
        else {
            std::cerr << "Warning: Texture coordinates count mismatch!" << std::endl;
        }
    }

    if (normals.size() != vertices.size()) {
        if (normals.empty()) {
            normals.resize(vertices.size(), { 0.0f, 1.0f, 0.0f });
        }
        else {
            std::cerr << "Warning: Normals count mismatch!" << std::endl;
        }
    }

    SetupBuffers();
    return isInitialized;
}

void Model::SetData(const std::vector<Vertex>& verts,
    const std::vector<TexCoord>& texs,
    const std::vector<Normal>& norms,
    const std::vector<unsigned int>& inds) {
    vertices = verts;
    texCoords = texs;
    normals = norms;
    indices = inds;
    SetupBuffers();
}

void Model::Draw() const {
    if (!isInitialized) {
        std::cerr << "Cannot draw: model not initialized!" << std::endl;
        return;
    }

    glBindVertexArray(VAO);
    glDrawElements(GL_TRIANGLES, static_cast<GLsizei>(indices.size()), GL_UNSIGNED_INT, 0);
    glBindVertexArray(0);
}

glm::vec3 Model::GetMin() const {
    if (vertices.empty()) return glm::vec3(0.0f);

    float minX = vertices[0].x;
    float minY = vertices[0].y;
    float minZ = vertices[0].z;

    for (const auto& v : vertices) {
        if (v.x < minX) minX = v.x;
        if (v.y < minY) minY = v.y;
        if (v.z < minZ) minZ = v.z;
    }
    return glm::vec3(minX, minY, minZ);
}

glm::vec3 Model::GetMax() const {
    if (vertices.empty()) return glm::vec3(0.0f);

    float maxX = vertices[0].x;
    float maxY = vertices[0].y;
    float maxZ = vertices[0].z;

    for (const auto& v : vertices) {
        if (v.x > maxX) maxX = v.x;
        if (v.y > maxY) maxY = v.y;
        if (v.z > maxZ) maxZ = v.z;
    }
    return glm::vec3(maxX, maxY, maxZ);
}

glm::vec3 Model::GetSize() const {
    glm::vec3 min = GetMin();
    glm::vec3 max = GetMax();
    return max - min;
}


void Model::InitPlanets()
{
    planets.clear();

    if (!IsInitialized()) return;

    glm::vec3 size = GetSize();
    float maxDim = std::max(size.x, std::max(size.y, size.z));
    if (maxDim == 0.0f) maxDim = 1.0f;

    float desiredSize = 1.0f;
    float modelScale = desiredSize / maxDim;

    planets.push_back({
        glm::vec3(0.0f),    // position
        modelScale * 2.0f,  // scale
        0.0f,               // rotation
        0.0f,               // orbitRadius
        0.0f,               // orbitSpeed
        0.0f,               // orbitAngle
        20.0f               // selfRotationSpeed
        });

    int numObjects = 5;
    float baseOrbit = 3.0f;
    float orbitStep = 2.5f;

    for (int i = 0; i < numObjects; i++) {
        float angle = i * 360.0f / numObjects;
        float rad = glm::radians(angle);
        float orbitRadius = baseOrbit + i * orbitStep;

        glm::vec3 pos;
        pos.x = orbitRadius * cos(rad);
        pos.y = 0.0f;
        pos.z = orbitRadius * sin(rad);

        planets.push_back({
            pos,              // position
            modelScale,       // scale
            0.0f,             // rotation
            orbitRadius,      // orbitRadius
            5.0f + i * 2.0f,  // orbitSpeed
            i * 90.0f,        // orbitAngle
            30.0f             // selfRotationSpeed
            });
    }
}
