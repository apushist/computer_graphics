#pragma once

#include <GL/glew.h>

void InitQuad();
void InitFan();
void InitPentagon();
void Draw(GLenum mode, int count);

extern GLuint VBO;
extern int currentFigure;