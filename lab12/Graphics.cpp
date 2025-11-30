#include "Graphics.h"
#include "Shaders.h"

#include <vector>
#include <cmath>

using namespace std;

GLuint VBO;
GLuint VBO_Colors;
GLuint IBO; 
int currentIndexCount = 0;
float figurePos[3] = { 0.0f, 0.0f, 0.0f };

struct Vertex3D {
    float x, y, z;
};

struct Color {
    float r, g, b;
};

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
    Vertex3D vertices[8] =
    {
        {-0.5f, -0.5f,  0.5f}, // 0 - передний нижний левый
        { 0.5f, -0.5f,  0.5f}, // 1 - передний нижний правый
        { 0.5f,  0.5f,  0.5f}, // 2 - передний верхний правый
        {-0.5f,  0.5f,  0.5f}, // 3 - передний верхний левый
        {-0.5f, -0.5f, -0.5f}, // 4 - задний нижний левый
        { 0.5f, -0.5f, -0.5f}, // 5 - задний нижний правый
        { 0.5f,  0.5f, -0.5f}, // 6 - задний верхний правый
        {-0.5f,  0.5f, -0.5f}  // 7 - задний верхний левый
    };

    GLuint indices[36] =
    {
        // Передняя грань
        0, 1, 2,  0, 2, 3,
        // Задняя грань  
        4, 6, 5,  4, 7, 6,
        // Верхняя грань
        3, 2, 6,  3, 6, 7,
        // Нижняя грань
        0, 5, 1,  0, 4, 5,
        // Левая грань
        0, 3, 7,  0, 7, 4,
        // Правая грань
        1, 5, 6,  1, 6, 2
    };

    Color colors[8] =
    {
        {1.0f, 0.0f, 0.0f},
        {0.0f, 1.0f, 0.0f}, 
        {0.0f, 0.0f, 1.0f}, 
        {1.0f, 1.0f, 0.0f},
        {1.0f, 0.0f, 1.0f}, 
        {0.0f, 1.0f, 1.0f}, 
        {0.5f, 0.5f, 1.0f}, 
        {1.0f, 0.5f, 0.0f}  
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

    currentIndexCount = 36;
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