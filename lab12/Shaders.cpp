#include "Shaders.h"
#include <vector>
#include <iostream>

using namespace std;

GLuint Program;
GLint Attrib_coord;
GLint Attrib_color;
GLint Uniform_offset;
GLint Uniform_scale;

const char* VertexShaderSourceGradient = R"(
#version 330 core
in vec3 coord;      
in vec3 color;      
out vec3 fragColor;
uniform vec3 offset;
uniform float scale;

void main() {
    vec3 pos = coord  * scale + offset;

    float perspective = 1.0 / (1.0 + max(0.0, pos.z) * 0.5);
    
    vec3 rotatedPos;
    rotatedPos.x = pos.x * perspective;
    rotatedPos.y = pos.y * perspective;
    rotatedPos.z = pos.z;

    gl_Position = vec4(rotatedPos, 1.0);
    fragColor = color;
}
)";

const char* FragShaderSourceGradient = R"(
#version 330 core
in vec3 fragColor;
out vec4 color;
void main() {
    color = vec4(fragColor, 1.0);
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
    GLuint vs_gradient = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vs_gradient, 1, &VertexShaderSourceGradient, NULL);
    glCompileShader(vs_gradient);
    CheckShader(vs_gradient);

    GLuint fs_gradient = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fs_gradient, 1, &FragShaderSourceGradient, NULL);
    glCompileShader(fs_gradient);
    CheckShader(fs_gradient);

    Program = glCreateProgram();
    glAttachShader(Program, vs_gradient);
    glAttachShader(Program, fs_gradient);
    glLinkProgram(Program);

    Attrib_coord = glGetAttribLocation(Program, "coord");
    Attrib_color = glGetAttribLocation(Program, "color");
    Uniform_offset = glGetUniformLocation(Program, "offset");
    Uniform_scale = glGetUniformLocation(Program, "scale");
}