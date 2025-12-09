#include "Model.h"
#include <fstream>
#include <sstream>
#include <iostream>

Model::Model() : isInitialized(false) {
    VAO = 0;
    VBO_Vertices = 0;
    VBO_TexCoords = 0;
    VBO_Normals = 0;
    EBO = 0;
}

Model::~Model() {
    if (VAO != 0) glDeleteVertexArrays(1, &VAO);
    if (VBO_Vertices != 0) glDeleteBuffers(1, &VBO_Vertices);
    if (VBO_TexCoords != 0) glDeleteBuffers(1, &VBO_TexCoords);
    if (VBO_Normals != 0) glDeleteBuffers(1, &VBO_Normals);
    if (EBO != 0) glDeleteBuffers(1, &EBO);
}

void Model::SetupBuffers() {
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
}

bool Model::LoadFromOBJ(const std::string& filename) {
    std::ifstream file(filename);
    if (!file.is_open()) {
        return false;
    }

    std::vector<Vertex> tempVertices;
    std::vector<TexCoord> tempTexCoords;
    std::vector<Normal> tempNormals;

    vertices.clear();
    texCoords.clear();
    normals.clear();
    indices.clear();

    std::string line;
    while (std::getline(file, line)) {
        if (line.empty() || line[0] == '#') continue;

        std::istringstream iss(line);
        std::string type;
        iss >> type;

        if (type == "v") {
            Vertex v;
            iss >> v.x >> v.y >> v.z;
            tempVertices.push_back(v);
        }
        else if (type == "vt") {
            TexCoord tc;
            iss >> tc.u >> tc.v;
            tempTexCoords.push_back(tc);
        }
        else if (type == "vn") {
            Normal n;
            iss >> n.x >> n.y >> n.z;
            tempNormals.push_back(n);
        }
        else if (type == "f") {
            std::string v1, v2, v3;
            iss >> v1 >> v2 >> v3;

            auto parseVertex = [&](const std::string& vertexStr) -> unsigned int {
                std::string str = vertexStr;
                for (char& c : str) if (c == '/') c = ' ';

                std::istringstream viss(str);
                int vIdx = 0, vtIdx = 0, vnIdx = 0;

                viss >> vIdx;
                if (!viss.eof()) viss >> vtIdx;
                if (!viss.eof()) viss >> vnIdx;

                if (vIdx > 0) vertices.push_back(tempVertices[vIdx - 1]);
                if (vtIdx > 0) texCoords.push_back(tempTexCoords[vtIdx - 1]);
                if (vnIdx > 0) normals.push_back(tempNormals[vnIdx - 1]);

                return vertices.size() - 1;
                };

            unsigned int idx1 = parseVertex(v1);
            unsigned int idx2 = parseVertex(v2);
            unsigned int idx3 = parseVertex(v3);

            indices.push_back(idx1);
            indices.push_back(idx2);
            indices.push_back(idx3);
        }
    }

    file.close();

    if (texCoords.empty() && !vertices.empty()) {
        texCoords.resize(vertices.size(), { 0.0f, 0.0f });
    }

    if (normals.empty() && !vertices.empty()) {
        normals.resize(vertices.size(), { 0.0f, 1.0f, 0.0f });
    }

    SetupBuffers();
    return isInitialized;
}

void Model::Draw() const {
    if (!isInitialized) return;

    glBindVertexArray(VAO);
    glDrawElements(GL_TRIANGLES, static_cast<GLsizei>(indices.size()), GL_UNSIGNED_INT, 0);
    glBindVertexArray(0);
}