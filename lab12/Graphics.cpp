#include "Graphics.h"
#include "Shaders.h"

#include <SFML/Graphics.hpp>
#include <vector>
#include <cmath>
#include <iostream>

using namespace std;

GLuint VBO;
GLuint VBO_Colors;
GLuint VBO_TexCoords = 0;
GLuint IBO; 

int currentIndexCount = 0;
float figurePos[3] = { 0.0f, 0.0f, 0.0f };

GLuint Texture1 = 0;
GLuint Texture2 = 0;

float colorMix = 1.0f;
float textureMix = 0.5f;

float cubeAngleX = 0.5f;
float cubeAngleY = 0.8f;

float circleScale[3] = { 0.5f, 0.5f, 1.0f };

struct Vertex3D {
    float x, y, z;
};

struct Color {
    float r, g, b;
};

// Загрузчик текстур
GLuint LoadTexture(const char* filename)
{
    sf::Image img;
    if (!img.loadFromFile(filename))
    {
        cout << "Failed to load texture: " << filename << endl;
        return 0;
    }

    img.flipVertically();

    GLuint texID;
    glGenTextures(1, &texID);
    glBindTexture(GL_TEXTURE_2D, texID);

    glTexImage2D(
        GL_TEXTURE_2D,
        0,
        GL_RGBA,
        img.getSize().x,
        img.getSize().y,
        0,
        GL_RGBA,
        GL_UNSIGNED_BYTE,
        img.getPixelsPtr()
    );

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

    return texID;
}


void InitTetrahedron()
{
    Vertex3D vertices[4] =
    {
        { 0.0f,  0.5f,  0.0f},  
        {-0.5f, -0.5f,  0.3f},  
        { 0.5f, -0.5f,  0.3f},  
        { 0.0f, -0.2f, -0.5f}   
    };

    GLuint indices[12] =
    {
        0, 1, 2,  
        0, 2, 3,  
        0, 3, 1,  
        1, 3, 2   
    };

    Color colors[4] =
    {
        {1.0f, 0.0f, 0.0f}, 
        {0.0f, 1.0f, 0.0f}, 
        {0.0f, 0.0f, 1.0f}, 
        {1.0f, 1.0f, 1.0f} 
    };

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

    glGenBuffers(1, &VBO_Colors);
    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

    glGenBuffers(1, &IBO);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

    currentIndexCount = 12;
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

}

void InitCube()
{
    Vertex3D vertices[24] =
    {
        {-0.5f, -0.5f,  0.5f}, {0.5f, -0.5f,  0.5f}, {0.5f,  0.5f,  0.5f}, {-0.5f,  0.5f,  0.5f},
        {-0.5f, -0.5f, -0.5f}, {-0.5f,  0.5f, -0.5f}, {0.5f,  0.5f, -0.5f}, {0.5f, -0.5f, -0.5f},
        {-0.5f, -0.5f, -0.5f}, {-0.5f, -0.5f,  0.5f}, {-0.5f,  0.5f,  0.5f}, {-0.5f,  0.5f, -0.5f},
        {0.5f, -0.5f, -0.5f}, {0.5f,  0.5f, -0.5f}, {0.5f,  0.5f,  0.5f}, {0.5f, -0.5f,  0.5f},
        {-0.5f,  0.5f, -0.5f}, {-0.5f,  0.5f,  0.5f}, {0.5f,  0.5f,  0.5f}, {0.5f,  0.5f, -0.5f},
        {-0.5f, -0.5f, -0.5f}, {0.5f, -0.5f, -0.5f}, {0.5f, -0.5f,  0.5f}, {-0.5f, -0.5f,  0.5f}
    };

    Color colors[24];
    for (int i = 0; i < 24; i++)
    {
        float r = (vertices[i].x + 0.5f);
        float g = (vertices[i].y + 0.5f);
        float b = (vertices[i].z + 0.5f);
        colors[i] = { r, g, b };
    }

    float texCoords[24][2] =
    {
        {0,0},{1,0},{1,1},{0,1},
        {0,0},{1,0},{1,1},{0,1},
        {0,0},{1,0},{1,1},{0,1},
        {0,0},{1,0},{1,1},{0,1},
        {0,0},{1,0},{1,1},{0,1},
        {0,0},{1,0},{1,1},{0,1}
    };

    GLuint indices[36] =
    {
        0,1,2, 0,2,3,       
        4,5,6, 4,6,7,       
        8,9,10, 8,10,11,    
        12,13,14, 12,14,15, 
        16,17,18, 16,18,19, 
        20,21,22, 20,22,23 
    };

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

    glGenBuffers(1, &VBO_Colors);
    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

    glGenBuffers(1, &VBO_TexCoords);
    glBindBuffer(GL_ARRAY_BUFFER, VBO_TexCoords);
    glBufferData(GL_ARRAY_BUFFER, sizeof(texCoords), texCoords, GL_STATIC_DRAW);

    glGenBuffers(1, &IBO);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

    currentIndexCount = 36;
}

void InitCircle()
{
    const int segments = 64;
    const int verticesCount = segments + 1; 

    vector<Vertex3D> vertices(verticesCount);
    vector<Color> colors(verticesCount);

    vertices[0] = { 0.0f, 0.0f, 0.0f };
    colors[0] = { 0.0f, 0.0f, 0.0f }; 

    for (int i = 1; i < verticesCount; i++) {
        float angle = 2.0f * 3.14159265f * (i - 1) / (verticesCount - 1);
        vertices[i] = { cos(angle), sin(angle), 0.0f };

        colors[i] = { vertices[i].x, vertices[i].y, 0.0f };
    }

    vector<GLuint> indices;
    for (int i = 1; i < verticesCount; i++) {
        indices.push_back(0);
        indices.push_back(i);
        indices.push_back((i % (verticesCount - 1)) + 1);
    }

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex3D), vertices.data(), GL_STATIC_DRAW);

    glGenBuffers(1, &VBO_Colors);
    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glBufferData(GL_ARRAY_BUFFER, colors.size() * sizeof(Color), colors.data(), GL_STATIC_DRAW);

    glGenBuffers(1, &IBO);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size() * sizeof(GLuint), indices.data(), GL_STATIC_DRAW);

    currentIndexCount = indices.size();

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
}


void MoveFigure(float dx, float dy, float dz)
{
    figurePos[0] += dx;
    figurePos[1] += dy;
    figurePos[2] += dz;
}

void DrawGradient3D(int vertexCount)
{
    glUseProgram(Program);

    glUniform3f(Uniform_offset, figurePos[0], figurePos[1], figurePos[2]);

    float scale = 1.0f / (1.0f + figurePos[2] * 0.5f);
    glUniform1f(Uniform_scale, scale);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glEnableVertexAttribArray(Attrib_coord);
    glVertexAttribPointer(Attrib_coord, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex3D), (void*)0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glEnableVertexAttribArray(Attrib_color);
    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Color), (void*)0);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
    glDrawElements(GL_TRIANGLES, currentIndexCount, GL_UNSIGNED_INT, 0);


    glDisableVertexAttribArray(Attrib_coord);
    glDisableVertexAttribArray(Attrib_color);
    glUseProgram(0);
}

// Отрисовка куба: текстура + цвет
void DrawTextureCube1()
{
    glUseProgram(ProgramTexture1);

    GLuint angleXLoc = glGetUniformLocation(ProgramTexture1, "angleX");
    GLuint angleYLoc = glGetUniformLocation(ProgramTexture1, "angleY");
    glUniform1f(angleXLoc, cubeAngleX);
    glUniform1f(angleYLoc, cubeAngleY);

    glUniform3f(Uniform_offset, figurePos[0], figurePos[1], figurePos[2]);

    float scale = 1.0f / (1.0f + figurePos[2] * 0.5f);
    glUniform1f(Uniform_scale, scale);

    glUniform1f(Uniform_colorMix, colorMix);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glEnableVertexAttribArray(Attrib_coord);
    glVertexAttribPointer(Attrib_coord, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glEnableVertexAttribArray(Attrib_color);
    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_TexCoords);
    glEnableVertexAttribArray(Attrib_texCoord);
    glVertexAttribPointer(Attrib_texCoord, 2, GL_FLOAT, GL_FALSE, 0, 0);

    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, Texture1);
    glUniform1i(glGetUniformLocation(ProgramTexture1, "texture1"), 0);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
    glDrawElements(GL_TRIANGLES, currentIndexCount, GL_UNSIGNED_INT, 0);
}

// Отрисовка куба: 2 текстуры
void DrawTextureCube2()
{
    glUseProgram(ProgramTexture2);

    GLuint angleXLoc = glGetUniformLocation(ProgramTexture1, "angleX");
    GLuint angleYLoc = glGetUniformLocation(ProgramTexture1, "angleY");
    glUniform1f(angleXLoc, cubeAngleX);
    glUniform1f(angleYLoc, cubeAngleY);

    glUniform3f(Uniform_offset, figurePos[0], figurePos[1], figurePos[2]);

    float scale = 1.0f / (1.0f + figurePos[2] * 0.5f);
    glUniform1f(Uniform_scale, scale);

    glUniform1f(Uniform_textureMix, textureMix);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glEnableVertexAttribArray(Attrib_coord);
    glVertexAttribPointer(Attrib_coord, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glEnableVertexAttribArray(Attrib_color);
    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_TexCoords);
    glEnableVertexAttribArray(Attrib_texCoord);
    glVertexAttribPointer(Attrib_texCoord, 2, GL_FLOAT, GL_FALSE, 0, 0);

    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, Texture1);
    glUniform1i(glGetUniformLocation(ProgramTexture2, "texture1"), 0);

    glActiveTexture(GL_TEXTURE1);
    glBindTexture(GL_TEXTURE_2D, Texture2);
    glUniform1i(glGetUniformLocation(ProgramTexture2, "texture2"), 1);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
    glDrawElements(GL_TRIANGLES, currentIndexCount, GL_UNSIGNED_INT, 0);
}

void InitTextures()
{
    Texture1 = LoadTexture("texture1.png");
    Texture2 = LoadTexture("texture2.png");
}

void DrawCircle()
{
    glUseProgram(ProgramCircle);

    glUniform3f(glGetUniformLocation(ProgramCircle, "offset"),
        figurePos[0], figurePos[1], figurePos[2]);

    glUniform3f(Uniform_circleScale,
        circleScale[0], circleScale[1], circleScale[2]);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glEnableVertexAttribArray(Attrib_coord);
    glVertexAttribPointer(Attrib_coord, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glEnableVertexAttribArray(Attrib_color);
    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, 0, 0);

    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
    glDrawElements(GL_TRIANGLES, currentIndexCount, GL_UNSIGNED_INT, 0);

    glDisableVertexAttribArray(Attrib_coord);
    glDisableVertexAttribArray(Attrib_color);
    glUseProgram(0);
}