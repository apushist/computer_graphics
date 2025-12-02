#pragma once

#include <GL/glew.h>

extern GLuint Program;
extern GLint Attrib_coord;
extern GLint Attrib_color;
extern GLint Uniform_offset;
extern GLint Uniform_scale;

extern GLuint ProgramTexture1;
extern GLuint ProgramTexture2;
extern GLuint ProgramCircle;

extern GLint Attrib_coord;
extern GLint Attrib_color;
extern GLint Attrib_texCoord;

extern GLint Uniform_offset;
extern GLint Uniform_scale;
extern GLint Uniform_colorMix;
extern GLint Uniform_textureMix;
extern GLint Uniform_circleScale;

void InitShader();