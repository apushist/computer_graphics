#pragma once

#include <GL/glew.h>

void InitQuad();
void InitFan();
void InitPentagon();
void Draw(GLenum mode, int count);
void DrawUniform(GLenum mode, int count);
void SetColor(float r, float g, float b, float a = 1.0f);

extern GLuint VBO;
extern int currentFigure;