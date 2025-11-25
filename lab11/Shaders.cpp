#include "Shaders.h"
#include <vector>
#include <iostream>

using namespace std;

GLuint Program;     
GLuint ProgramUniform; 
GLint Attrib_coord;
GLint Attrib_coord_uniform;
GLint Uniform_color;

const char* VertexShaderSource = R"(
#version 330 core
in vec2 coord;
void main() {
    gl_Position = vec4(coord, 0.0, 1.0);
}
)";

const char* FragShaderSourceConst = R"(
#version 330 core
out vec4 color;
void main() {
    color = vec4(0.0, 0.7, 1.0, 1.0); 
}
)";

const char* FragShaderSourceUniform = R"(
#version 330 core
uniform vec4 u_color;
out vec4 color;
void main() {
    color = u_color; 
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

    GLuint fs_const = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fs_const, 1, &FragShaderSourceConst, NULL);
    glCompileShader(fs_const);
    CheckShader(fs_const);

    Program = glCreateProgram();
    glAttachShader(Program, vs);
    glAttachShader(Program, fs_const);
    glLinkProgram(Program);
    Attrib_coord = glGetAttribLocation(Program, "coord");

    GLuint fs_uniform = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fs_uniform, 1, &FragShaderSourceUniform, NULL);
    glCompileShader(fs_uniform);
    CheckShader(fs_uniform);

    ProgramUniform = glCreateProgram();
    glAttachShader(ProgramUniform, vs);
    glAttachShader(ProgramUniform, fs_uniform);
    glLinkProgram(ProgramUniform);
    Attrib_coord_uniform = glGetAttribLocation(ProgramUniform, "coord");
    Uniform_color = glGetUniformLocation(ProgramUniform, "u_color");
}