#include "Graphics.h"
#include "Shaders.h"

#include <vector>
#include <cmath>

using namespace std;

GLuint VBO;
int currentFigure = 0;

struct Vertex { float x, y; };

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
    glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void InitFan()
{
    vector<Vertex> fan;

    fan.push_back({ 0.0f, 0.0f });

    fan.push_back({ -0.5f, -0.2f }); 
    fan.push_back({ -0.2f,  0.4f }); 
    fan.push_back({ 0.4f,  0.3f }); 
    fan.push_back({ 0.6f, -0.1f }); 
    fan.push_back({ -0.5f, -0.2f });

    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, fan.size() * sizeof(Vertex), fan.data(), GL_STATIC_DRAW);
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