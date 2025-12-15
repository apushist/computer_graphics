#include "Shaders.h"
#include <iostream>

const char* vertexShaderSource = R"(
    #version 330 core
    layout(location = 0) in vec3 aPos;
    layout(location = 1) in vec2 aTexCoord;
    layout(location = 2) in vec3 aNormal;
    
    out vec2 TexCoord;
    out vec3 Normal;
    out vec3 FragPos;
    
    uniform mat4 model;
    uniform mat4 view;
    uniform mat4 projection;
    
    void main() {
        gl_Position = projection * view * model * vec4(aPos, 1.0);
        FragPos = vec3(model * vec4(aPos, 1.0));
        TexCoord = aTexCoord;
        Normal = mat3(transpose(inverse(model))) * aNormal;
    }
)";

const char* fragmentShaderSource = R"(
    #version 330 core
    in vec2 TexCoord;
    in vec3 Normal;
    in vec3 FragPos;
    
    out vec4 FragColor;
    
    uniform sampler2D texture1;
    uniform vec3 viewPos;

    // Точечный источник света
    struct PointLight {
        vec3 position;
        vec3 ambient;
        vec3 diffuse;
        vec3 specular;
        bool enabled;
        float constant;
        float linear;    //коэффициент линейного затухания
        float quadratic; //коэффициент квадратичного затухания
    };
    
    uniform PointLight pointLight;
    
    vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec3 materialDiffuse) {
        if (!light.enabled) return vec3(0.0);
        
        // Фоновое освещение
        vec3 ambient = light.ambient * materialDiffuse;
        
        // Рассеянное освещение
        vec3 lightDir = normalize(light.position - fragPos);
        float diff = max(dot(normal, lightDir), 0.0);
        vec3 diffuse = light.diffuse * diff * materialDiffuse;
        
        // Отраженное освещение (specular)
        vec3 reflectDir = reflect(-lightDir, normal);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
        vec3 specular = light.specular * spec * materialDiffuse;
        
        // Затухание
        float distance = length(light.position - fragPos);
        float attenuation = 1.0 / (light.constant + light.linear * distance + 
                                 light.quadratic * (distance * distance));
        
        ambient *= attenuation;
        diffuse *= attenuation;
        specular *= attenuation;
        
        return (ambient + diffuse + specular);
    }
    
    void main() {
        vec3 norm = normalize(Normal);
        vec3 viewDir = normalize(viewPos - FragPos);
        vec4 texColor = texture(texture1, TexCoord);
        vec3 materialDiffuse = texColor.rgb;
        
        // Освещение от точечного источника
        vec3 result = CalculatePointLight(pointLight, norm, FragPos, viewDir, materialDiffuse);
        
        FragColor = vec4(result, texColor.a);
    }
)";

bool InitShader(GLuint& shaderProgram)
{
    GLuint vertexShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertexShader, 1, &vertexShaderSource, nullptr);
    glCompileShader(vertexShader);

    GLint success;
    char infoLog[512];
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success) {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infoLog);
        std::cerr << "Vertex shader error: " << infoLog << std::endl;
        return false;
    }

    GLuint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragmentShader, 1, &fragmentShaderSource, nullptr);
    glCompileShader(fragmentShader);

    glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
    if (!success) {
        glGetShaderInfoLog(fragmentShader, 512, nullptr, infoLog);
        std::cerr << "Fragment shader error: " << infoLog << std::endl;
        return false;
    }

    shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
    if (!success) {
        glGetProgramInfoLog(shaderProgram, 512, nullptr, infoLog);
        std::cerr << "Shader program error: " << infoLog << std::endl;
        return false;
    }

    glDeleteShader(vertexShader);
    glDeleteShader(fragmentShader);

    return true;
}