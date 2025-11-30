#pragma once

#include <GL/glew.h>

void InitTetrahedron();
void InitCube();
void DrawGradient3D(int vertexCount);
void MoveFigure(float dx, float dy, float dz);

extern GLuint VBO;
extern GLuint VBO_Colors;
extern GLuint IBO;
extern float figurePos[3];
extern int currentIndexCount;