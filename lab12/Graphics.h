#pragma once

#include <GL/glew.h>

GLuint LoadTexture(const char* filename);
void InitTextures();

void InitTetrahedron();
void InitCube();

void DrawGradient3D(int vertexCount);
void DrawTextureCube1();
void DrawTextureCube2();

void MoveFigure(float dx, float dy, float dz);

extern GLuint VBO;
extern GLuint VBO_Colors;
extern GLuint VBO_Tex;
extern GLuint IBO;

extern float figurePos[3];
extern int currentIndexCount;

extern float colorMix;    
extern float textureMix;   

extern GLuint textureID1;
extern GLuint textureID2;

extern float cubeAngleX;
extern float cubeAngleY;