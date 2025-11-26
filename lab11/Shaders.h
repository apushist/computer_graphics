#pragma once

#include <GL/glew.h>

extern GLuint Program; 
extern GLuint ProgramUniform;
extern GLuint ProgramGradient;
extern GLint Attrib_coord;
extern GLint Attrib_coord_uniform;
extern GLint Attrib_color;
extern GLint Uniform_color;

void InitShader();