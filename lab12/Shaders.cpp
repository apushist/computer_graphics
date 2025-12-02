#include "Shaders.h"
#include <vector>
#include <iostream>

using namespace std;

GLuint Program;
GLuint ProgramTexture1;
GLuint ProgramTexture2;
GLuint ProgramCircle;

GLint Attrib_coord;
GLint Attrib_color;
GLint Attrib_texCoord;

GLint Uniform_offset;
GLint Uniform_scale;
GLint Uniform_colorMix;
GLint Uniform_textureMix;
GLint Uniform_circleScale;

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

const char* VertexShaderSourceTexture = R"(
#version 330 core

in vec3 coord;
in vec3 color;
in vec2 texCoord;

out vec3 fColor;
out vec2 fTexCoord;

uniform vec3 offset;
uniform float scale;
uniform float angleX;
uniform float angleY;

void main()
{
    vec3 pos = coord * scale + offset;

    float cosY = cos(angleY);
    float sinY = sin(angleY);
    vec3 rotatedPosY;
    rotatedPosY.x = pos.x * cosY - pos.z * sinY;
    rotatedPosY.z = pos.x * sinY + pos.z * cosY;
    rotatedPosY.y = pos.y;

    float cosX = cos(angleX);
    float sinX = sin(angleX);
    vec3 rotatedPos;
    rotatedPos.x = rotatedPosY.x;
    rotatedPos.y = rotatedPosY.y * cosX - rotatedPosY.z * sinX;
    rotatedPos.z = rotatedPosY.y * sinX + rotatedPosY.z * cosX;

    float p = 1.0 / (1.0 + max(0.0, rotatedPos.z) * 0.5);

    gl_Position = vec4(rotatedPos.x * p, rotatedPos.y * p, rotatedPos.z, 1.0);

    fColor = color;
    fTexCoord = texCoord;
}
)";


const char* FragShaderSourceTexture = R"(
#version 330 core

in vec3 fColor;
in vec2 fTexCoord;

uniform sampler2D texture1;
uniform float colorMix;

out vec4 outColor;

void main()
{
    vec4 tex = texture(texture1, fTexCoord);
    vec4 col = vec4(fColor, 1.0);
    outColor = mix(tex, col, colorMix);
}
)";

const char* FragShaderSource2Textures = R"(
#version 330 core

in vec3 fColor;
in vec2 fTexCoord;

uniform sampler2D texture1;
uniform sampler2D texture2;
uniform float textureMix;

out vec4 outColor;

void main()
{
    vec4 t1 = texture(texture1, fTexCoord);
    vec4 t2 = texture(texture2, fTexCoord);

    outColor = mix(t1, t2, textureMix);
}
)";

const char* VertexShaderSourceCircle = R"(
#version 330 core
in vec3 coord;
in vec3 color;
out vec3 fragColor;
uniform vec3 offset;
uniform vec3 circleScale;  // Масштаб по осям

void main() {
    vec3 pos = coord * circleScale + offset;
    gl_Position = vec4(pos, 1.0);
    fragColor = color;
}
)";

const char* FragShaderSourceCircle = R"(
#version 330 core
in vec3 fragColor;
out vec4 outColor;

vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
    float dist = length(fragColor.xy);
    if (dist > 1.0) {
        discard;
    }
    
    // Центр - белый (1,1,1), по краям - полный Hue круг
    float hue = atan(fragColor.y, fragColor.x) / (2.0 * 3.14159265);
    if (hue < 0.0) hue += 1.0;
    
    // Интерполяция от белого в центре к цвету на краю
    vec3 centerColor = vec3(1.0, 1.0, 1.0);
    vec3 edgeColor = hsv2rgb(vec3(hue, 1.0, 1.0));
    vec3 finalColor = mix(centerColor, edgeColor, dist);
    
    outColor = vec4(finalColor, 1.0);
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

    // Программа 1: текстура + цвет
    GLuint vs = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vs, 1, &VertexShaderSourceTexture, NULL);
    glCompileShader(vs);
    CheckShader(vs);

    GLuint fs1 = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fs1, 1, &FragShaderSourceTexture, NULL);
    glCompileShader(fs1);
    CheckShader(fs1);

    ProgramTexture1 = glCreateProgram();
    glAttachShader(ProgramTexture1, vs);
    glAttachShader(ProgramTexture1, fs1);
    glLinkProgram(ProgramTexture1);

    // Программа 2: 2 текстуры
    GLuint fs2 = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fs2, 1, &FragShaderSource2Textures, NULL);
    glCompileShader(fs2);
    CheckShader(fs2);

    ProgramTexture2 = glCreateProgram();
    glAttachShader(ProgramTexture2, vs);
    glAttachShader(ProgramTexture2, fs2);
    glLinkProgram(ProgramTexture2);

    //круг
    GLuint vs_circle = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vs_circle, 1, &VertexShaderSourceCircle, NULL);
    glCompileShader(vs_circle);
    CheckShader(vs_circle);

    GLuint fs_circle = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fs_circle, 1, &FragShaderSourceCircle, NULL);
    glCompileShader(fs_circle);
    CheckShader(fs_circle);

    ProgramCircle = glCreateProgram();
    glAttachShader(ProgramCircle, vs_circle);
    glAttachShader(ProgramCircle, fs_circle);
    glLinkProgram(ProgramCircle);

    Attrib_coord = 0;
    Attrib_color = 1;
    Attrib_texCoord = 2;

    Uniform_offset = glGetUniformLocation(ProgramTexture1, "offset");
    Uniform_scale = glGetUniformLocation(ProgramTexture1, "scale");
    Uniform_colorMix = glGetUniformLocation(ProgramTexture1, "colorMix");

    Uniform_textureMix = glGetUniformLocation(ProgramTexture2, "textureMix");
    Uniform_circleScale = glGetUniformLocation(ProgramCircle, "circleScale");
}