#include "Shaders.h"
#include <vector>
#include <iostream>

using namespace std;

GLuint Program;
GLint Attrib_coord;

const char* VertexShaderSource = R"(
#version 330 core
in vec2 coord;
void main() {
    gl_Position = vec4(coord, 0.0, 1.0);
}
)";

const char* FragShaderSource = R"(
#version 330 core
out vec4 color;
void main() {
    color = vec4(0.0, 0.7, 1.0, 1.0);
}
)";

static void CheckShader(GLuint shader)
{
    GLint status;
    glGetShaderiv(shader, GL_COMPILE_STATUS, &status);
    if (!status)
    {
        GLint len;
        glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &len);
        vector<char> buf(len);
        glGetShaderInfoLog(shader, len, NULL, buf.data());
        cout << "Shader error:\n" << buf.data() << endl;
    }
}

void InitShader()
{
    GLuint vs = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vs, 1, &VertexShaderSource, NULL);
    glCompileShader(vs);
    CheckShader(vs);

    GLuint fs = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fs, 1, &FragShaderSource, NULL);
    glCompileShader(fs);
    CheckShader(fs);

    Program = glCreateProgram();
    glAttachShader(Program, vs);
    glAttachShader(Program, fs);
    glLinkProgram(Program);

    Attrib_coord = glGetAttribLocation(Program, "coord");
}