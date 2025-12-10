#include "Camera.h"
#include <iostream>

Camera::Camera(glm::vec3 position, glm::vec3 up, float yaw, float pitch)
    : position(position), worldUp(up), yaw(yaw), pitch(pitch),
      movementSpeed(3.0f), mouseSensitivity(0.1f), zoom(45.0f) {
    front = glm::vec3(0.0f, 0.0f, -1.0f);
    updateCameraVectors();
}

glm::mat4 Camera::GetViewMatrix() const {
    return glm::lookAt(position, position + front, up);
}

glm::mat4 Camera::GetProjectionMatrix(float aspectRatio, 
                                     float nearPlane, 
                                     float farPlane) const {
    return glm::perspective(glm::radians(zoom), aspectRatio, nearPlane, farPlane);
}

void Camera::ProcessMouseMovement(float xoffset, float yoffset, bool constrainPitch) {
    xoffset *= mouseSensitivity;
    yoffset *= mouseSensitivity;
    
    yaw += xoffset;
    pitch += yoffset;
    
    if (constrainPitch) {
        if (pitch > 89.0f) pitch = 89.0f;
        if (pitch < -89.0f) pitch = -89.0f;
    }
    
    updateCameraVectors();
}

void Camera::ProcessMouseScroll(float yoffset) {
    zoom -= yoffset;
    if (zoom < 1.0f) zoom = 1.0f;
    if (zoom > 90.0f) zoom = 90.0f;
}

void Camera::MoveForward(float velocity) {
    position += front * velocity;
}

void Camera::MoveBackward(float velocity) {
    position -= front * velocity;
}

void Camera::MoveLeft(float velocity) {
    position -= right * velocity;
}

void Camera::MoveRight(float velocity) {
    position += right * velocity;
}

void Camera::MoveUp(float velocity) {
    position += up * velocity;
}

void Camera::MoveDown(float velocity) {
    position -= up * velocity;
}

void Camera::updateCameraVectors() {
    glm::vec3 newFront;
    newFront.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
    newFront.y = sin(glm::radians(pitch));
    newFront.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
    front = glm::normalize(newFront);
    
    right = glm::normalize(glm::cross(front, worldUp));
    up = glm::normalize(glm::cross(right, front));
}

void Camera::LookAt(const glm::vec3& target) {
    front = glm::normalize(target - position);
    right = glm::normalize(glm::cross(front, worldUp));
    up = glm::normalize(glm::cross(right, front));
}

void Camera::Rotate(float yawOffset, float pitchOffset) {
    yaw += yawOffset;
    pitch += pitchOffset;

    if (pitch > 89.0f) pitch = 89.0f;
    if (pitch < -89.0f) pitch = -89.0f;

    updateCameraVectors();
}