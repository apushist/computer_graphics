#pragma once
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

class Camera {
private:
    glm::vec3 position;
    glm::vec3 front;
    glm::vec3 up;
    glm::vec3 right;
    glm::vec3 worldUp;

    float yaw;
    float pitch;

    float movementSpeed;
    float mouseSensitivity;
    float zoom;

public:
    Camera(glm::vec3 position = glm::vec3(0.0f, 0.0f, 0.0f),
        glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f),
        float yaw = -90.0f, float pitch = 0.0f);

    glm::mat4 GetViewMatrix() const;
    glm::mat4 GetProjectionMatrix(float aspectRatio,
        float nearPlane = 0.1f,
        float farPlane = 100.0f) const;

    void ProcessMouseMovement(float xoffset, float yoffset,
        bool constrainPitch = true);
    void ProcessMouseScroll(float yoffset);

    void MoveForward(float velocity);
    void MoveBackward(float velocity);
    void MoveLeft(float velocity);
    void MoveRight(float velocity);
    void MoveUp(float velocity);
    void MoveDown(float velocity);

    glm::vec3 GetPosition() const { return position; }
    void SetPosition(const glm::vec3& newPos) { position = newPos; }

private:
    void updateCameraVectors();
};