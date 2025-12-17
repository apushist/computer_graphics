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
    uniform int uShadingModel;
    
    const float minnaertK = 0.8;
    
    const float PI = 3.1415926;

    // параметры Ashikhmin-Shirley
    const vec3 specColor = vec3(1.0);
    const float kd = 0.3;  // коэффициент диффузии
    const float ks = 0.7;  // коэффициент зеркала
    const float mx = 10.0; // грубость по биномали
    const float my = 10.0; // грубость по тангенсу
    const float r0 = 0.8; // коэффициент ‘ренел€

    struct PointLight {
        vec3 position;
        vec3 ambient;
        vec3 diffuse;
        vec3 specular;
        vec3 attenuation;
        int enabled;
    };

    struct DirectionalLight {
        vec3 direction;
        vec3 ambient;
        vec3 diffuse;
        vec3 specular;
        int enabled;
    };

    struct SpotLight {
        vec3 position;
        vec3 direction;
        vec3 ambient;
        vec3 diffuse;
        vec3 specular;
        vec3 attenuation;
        float cutoff;
        float outerCutoff;
        int enabled;
    };
    
    uniform PointLight pointl;
    uniform DirectionalLight dirl;
    uniform SpotLight spotl;
    
    float ToonDiffuse(float d)
    {
        if (d < 0.4) return 0.3;
        else if (d < 0.7) return 1.0;
        else return 1.3;
    }
    
    vec3 AshikhminShirleyBRDF(vec3 normal, vec3 viewDir, vec3 lightDir, vec3 tangent, vec3 bitangent, vec3 texColor)
    {
        vec3 n2 = normalize(normal);
        vec3 t2 = normalize(tangent);
        vec3 b2 = normalize(bitangent);
        vec3 l2 = normalize(lightDir);
        vec3 v2 = normalize(viewDir);
        vec3 h2 = normalize(l2 + v2);

        float nv = max(dot(n2, v2), 0.0);
        float nl = max(dot(n2, l2), 0.0);
        float nh = max(dot(n2, h2), 0.0);
        float hl = max(dot(h2, l2), 0.0);
        float t1h = dot(b2, h2);
        float t2h = dot(t2, h2);

        // ƒиффузна€ составл€юща€
        float rd = (28.0 / (23.0 * PI)) * (1.0 - pow(1.0 - 0.5 * nv, 5.0)) * (1.0 - pow(1.0 - 0.5 * nl, 5.0));

        // —пекул€рна€ составл€юща€
        float B = pow(nh, (mx * t1h * t1h + my * t2h * t2h) / max(1e-4, 1.0 - nh*nh));
        float F = (r0 + (1.0 - r0) * pow(1.0 - hl, 5.0)) / max(1e-4, hl * max(nv, nl));
        float rs = ks * B * F;

        // »спользуем цвет из текстуры
        vec3 diffuse = texColor * kd * (1.0 - ks) * rd;
        vec3 specular = specColor * rs;

        return nl * (diffuse + specular);
    }


    vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec3 texColor)
    {
        if (light.enabled == 0) return vec3(0.0);
        
        vec3 lightDir = normalize(light.position - fragPos);
        
        // Ambient - фоновое освещение
        vec3 ambient = light.ambient * texColor;
        
        // Diffuse - рассе€нное освещение
        float diff = max(dot(normal, lightDir), 0.0);

        if (uShadingModel == 1)
        {
            diff = ToonDiffuse(diff);
        }
        else if (uShadingModel == 2)
        {
            vec3 n2 = normalize(normal);
            vec3 l2 = normalize(lightDir);
            vec3 v2 = normalize(viewDir);
            
            float d1 = pow(max(dot(n2, l2), 0.0), 1.0 + minnaertK);
            float d2 = pow(1.0 - max(dot(n2, v2), 0.0), 1.0 - minnaertK);

            diff = d1 * d2;
        }

        vec3 diffuse = light.diffuse * diff * texColor;
        if (uShadingModel == 3)
        {
            vec3 tangent = normalize(vec3(0.0, 1.0, 0.0));
            vec3 bitangent = normalize(cross(normal, tangent));
            diffuse = AshikhminShirleyBRDF(normal, viewDir, lightDir, tangent, bitangent, texColor);
        }

        // Specular - отраженное освещение
        vec3 specular = vec3(0.0);
        if (uShadingModel == 0)
        {
            vec3 reflectDir = reflect(-lightDir, normal); 
            float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
            specular = light.specular * spec * texColor;
        }
        
        // Attenuation - затухание
        float distance = length(light.position - fragPos);
        float attenuation = 1.0 / (light.attenuation.x + 
                                 light.attenuation.y * distance + 
                                 light.attenuation.z * (distance * distance));
        
        return (ambient + diffuse + specular) * attenuation;
    }
    
    vec3 CalculateDirectionalLight(DirectionalLight light, vec3 normal, vec3 viewDir, vec3 texColor) {
        if (light.enabled == 0) return vec3(0.0);
        
        vec3 lightDir = normalize(-light.direction);
        
        // Ambient
        vec3 ambient = light.ambient * texColor;
        
        // Diffuse
        float diff = max(dot(normal, lightDir), 0.0);
        
        if (uShadingModel == 1)
        {
            diff = ToonDiffuse(diff);
        }
        else if (uShadingModel == 2)
        {
            vec3 n2 = normalize(normal);
            vec3 l2 = normalize(lightDir);
            vec3 v2 = normalize(viewDir);
            
            float d1 = pow(max(dot(n2, l2), 0.0), 1.0 + minnaertK);
            float d2 = pow(1.0 - max(dot(n2, v2), 0.0), 1.0 - minnaertK);

            diff = d1 * d2;
        }

        vec3 diffuse = light.diffuse * diff * texColor;
        if (uShadingModel == 3)
        {
            vec3 tangent = normalize(vec3(0.0, 1.0, 0.0));
            vec3 bitangent = normalize(cross(normal, tangent));
            diffuse = AshikhminShirleyBRDF(normal, viewDir, lightDir, tangent, bitangent, texColor);
        }

        // Specular
        vec3 specular = vec3(0.0);
        if (uShadingModel == 0)
        {
            vec3 reflectDir = reflect(-lightDir, normal);
            float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
            specular = light.specular * spec * texColor;
        }
        
        return ambient + diffuse + specular;
    }
    
    vec3 CalculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec3 texColor) {
        if (light.enabled == 0) return vec3(0.0);
        
        vec3 lightDir = normalize(light.position - fragPos);
        float theta = dot(lightDir, normalize(-light.direction));
        
        // ћ€гкие границы конуса
        float epsilon = light.cutoff - light.outerCutoff;
        float intensity = clamp((theta - light.outerCutoff) / epsilon, 0.0, 1.0);
        
        if (intensity > 0.0) {
            // Ambient
            vec3 ambient = light.ambient * texColor;
            
            // Diffuse
            float diff = max(dot(normal, lightDir), 0.0);

            if (uShadingModel == 1)
            {
                diff = ToonDiffuse(diff);
            }
            else if (uShadingModel == 2)
            {
                vec3 n2 = normalize(normal);
                vec3 l2 = normalize(lightDir);
                vec3 v2 = normalize(viewDir);
            
                float d1 = pow(max(dot(n2, l2), 0.0), 1.0 + minnaertK);
                float d2 = pow(1.0 - max(dot(n2, v2), 0.0), 1.0 - minnaertK);

                diff = d1 * d2;
            }

            vec3 diffuse = light.diffuse * diff * texColor;
            if (uShadingModel == 3)
            {
                vec3 tangent = normalize(vec3(0.0, 1.0, 0.0));
                vec3 bitangent = normalize(cross(normal, tangent));
                diffuse = AshikhminShirleyBRDF(normal, viewDir, lightDir, tangent, bitangent, texColor);
            }

            // Specular
            vec3 specular = vec3(0.0);
            if (uShadingModel == 0)
            {
                vec3 reflectDir = reflect(-lightDir, normal);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
                specular = light.specular * spec * texColor;
            }
            
            // Attenuation
            float distance = length(light.position - fragPos);
            float attenuation = 1.0 / (light.attenuation.x + 
                                     light.attenuation.y * distance + 
                                     light.attenuation.z * (distance * distance));
            
            return (ambient + diffuse + specular) * attenuation * intensity;
        }
        else {
            return vec3(0.0);
        }
    }
    
    void main() {
        vec3 norm = normalize(Normal);
        vec3 viewDir = normalize(viewPos - FragPos);
        vec3 texColor = texture(texture1, TexCoord).rgb;
        
        vec3 result = vec3(0.0);
        
        // —кладываем освещение от всех источников
        result += CalculateDirectionalLight(dirl, norm, viewDir, texColor);
        result += CalculatePointLight(pointl, norm, FragPos, viewDir, texColor);
        result += CalculateSpotLight(spotl, norm, FragPos, viewDir, texColor);
        
        FragColor = vec4(result, 1.0);
    }
)";

// ƒл€ отрисовки контура текущего объекта
const char* outlineVertexShaderSource = R"(
    #version 330 core
    layout(location = 0) in vec3 aPos;
    layout(location = 2) in vec3 aNormal;

    uniform mat4 model;
    uniform mat4 view;
    uniform mat4 projection;
    uniform float outlineWidth;

    void main()
    {
        vec3 pos = aPos + aNormal * outlineWidth;
        gl_Position = projection * view * model * vec4(pos, 1.0);
    }
)";

const char* outlineFragmentShaderSource = R"(
    #version 330 core
    out vec4 FragColor;
    uniform vec3 outlineColor;

    void main()
    {
        FragColor = vec4(outlineColor, 1.0);
    }
)";

bool InitOutlineShader(GLuint& shaderProgram)
{
    GLuint v = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(v, 1, &outlineVertexShaderSource, nullptr);
    glCompileShader(v);

    GLuint f = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(f, 1, &outlineFragmentShaderSource, nullptr);
    glCompileShader(f);

    shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, v);
    glAttachShader(shaderProgram, f);
    glLinkProgram(shaderProgram);

    glDeleteShader(v);
    glDeleteShader(f);

    return true;
}

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