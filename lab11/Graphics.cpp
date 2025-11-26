#include "Graphics.h"
#include "Shaders.h"

#include <vector>
#include <cmath>

using namespace std;

GLuint VBO;
GLuint VBO_Colors;
int currentFigure = 0;

struct Vertex { 
    float x, y;
};

struct Color {
    float r, g, b;
};

void SetColor(float r, float g, float b, float a)
{
    glUseProgram(ProgramUniform);
    glUniform4f(Uniform_color, r, g, b, a);
    glUseProgram(0);
}

void InitQuad()
{
    Vertex quad[4] =
    {
        {-0.5f, -0.5f},
        { 0.5f, -0.5f},
        { 0.5f,  0.5f},
        {-0.5f,  0.5f}
    };

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(quad), quad, GL_STATIC_DRAW);

    Color colors[4] =
    {
        {1.0f, 0.0f, 0.0f},
        {0.0f, 1.0f, 0.0f}, 
        {0.0f, 0.0f, 1.0f}, 
        {1.0f, 1.0f, 1.0f}  
    };

    glGenBuffers(1, &VBO_Colors);
    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void InitFan()
{
    Vertex fan[6] =
    {
        { 0.0f,  0.0f},  // центр
        {-0.5f, -0.2f},
        {-0.2f,  0.4f},
        { 0.4f,  0.3f},
        { 0.6f, -0.1f},
        {-0.5f, -0.2f}   
    };

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(fan), fan, GL_STATIC_DRAW);

    Color colors[6] =
    {
        {1.0f, 1.0f, 1.0f}, 
        {1.0f, 0.0f, 0.0f}, 
        {0.0f, 1.0f, 0.0f},
        {0.0f, 0.0f, 1.0f}, 
        {1.0f, 0.0f, 1.0f}, 
        {1.0f, 1.0f, 0.0f}  
    };

    glGenBuffers(1, &VBO_Colors);
    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
}
void InitPentagon()
{
    const int N = 5;
    const float R = 0.6f;

    vector<Vertex> pts;
    for (int i = 0; i < N; i++)
    {
        float angle = 2.0f * 3.14159265f * i / N;
        pts.push_back({ R * cos(angle), R * sin(angle) });
    }

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, pts.size() * sizeof(Vertex), pts.data(), GL_STATIC_DRAW);

    vector<Color> colors;
    for (int i = 0; i < N; i++)
    {
        float r = (i == 0) ? 1.0f : (i == 1 || i == 4) ? 0.5f : 0.0f;
        float g = (i == 1 || i == 2) ? 1.0f : (i == 0 || i == 3) ? 0.5f : 0.0f;
        float b = (i == 3 || i == 4) ? 1.0f : (i == 2) ? 0.5f : 0.0f;
        colors.push_back({ r, g, b });
    }

    glGenBuffers(1, &VBO_Colors);
    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glBufferData(GL_ARRAY_BUFFER, colors.size() * sizeof(Color), colors.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void Draw(GLenum mode, int count)
{
    glUseProgram(Program);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glEnableVertexAttribArray(Attrib_coord);
    glVertexAttribPointer(Attrib_coord, 2, GL_FLOAT, GL_FALSE, 0, (void*)0);
    glDrawArrays(mode, 0, count);
    glDisableVertexAttribArray(Attrib_coord);
    glUseProgram(0);
}

void DrawUniform(GLenum mode, int count)
{
    glUseProgram(ProgramUniform);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glEnableVertexAttribArray(Attrib_coord_uniform);
    glVertexAttribPointer(Attrib_coord_uniform, 2, GL_FLOAT, GL_FALSE, 0, (void*)0);
    glDrawArrays(mode, 0, count);
    glDisableVertexAttribArray(Attrib_coord_uniform);
    glUseProgram(0);
}

void DrawGradient(GLenum mode, int count)
{
    glUseProgram(ProgramGradient);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glEnableVertexAttribArray(Attrib_coord);
    glVertexAttribPointer(Attrib_coord, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)0);

    glBindBuffer(GL_ARRAY_BUFFER, VBO_Colors);
    glEnableVertexAttribArray(Attrib_color);
    glVertexAttribPointer(Attrib_color, 3, GL_FLOAT, GL_FALSE, sizeof(Color), (void*)0);

    glDrawArrays(mode, 0, count);

    glDisableVertexAttribArray(Attrib_coord);
    glDisableVertexAttribArray(Attrib_color);
    glUseProgram(0);
}
