#include <GL/glew.h>
#include <SFML/Window.hpp>
#include <SFML/OpenGL.hpp>

#include "Shaders.h"
#include "Graphics.h"

int main()
{
    sf::Window window(sf::VideoMode({ 600, 600 }), "Lab11 - OpenGL");
    glewInit();

    InitShader();
    InitQuad();

    bool useUniformColor = false; 
    SetColor(1.0f, 0.0f, 0.0f); 

    while (window.isOpen())
    {
        while (auto event = window.pollEvent())
        {
            if (event->is<sf::Event::Closed>())
                window.close();

            if (event->is<sf::Event::KeyPressed>())
            {
                auto key = event->getIf<sf::Event::KeyPressed>()->scancode;

                if (key == sf::Keyboard::Scan::Num1)
                {
                    InitQuad();
                    currentFigure = 0;
                }
                if (key == sf::Keyboard::Scan::Num2)
                {
                    InitFan();
                    currentFigure = 1;
                }
                if (key == sf::Keyboard::Scan::Num3)
                {
                    InitPentagon();
                    currentFigure = 2;
                }

                if (key == sf::Keyboard::Scan::Num4)
                {
                    useUniformColor = false;
                }
                if (key == sf::Keyboard::Scan::Num5)
                {
                    useUniformColor = true; 
                    SetColor(1.0f, 0.0f, 0.0f); 
                }
                if (key == sf::Keyboard::Scan::Num6)
                {
                    useUniformColor = true; 
                    SetColor(0.0f, 1.0f, 0.0f); 
                }
            }
        }

        glClear(GL_COLOR_BUFFER_BIT);

        if (currentFigure == 0)
        {
            if (useUniformColor) DrawUniform(GL_TRIANGLE_FAN, 4);
            else Draw(GL_TRIANGLE_FAN, 4);
        }
        if (currentFigure == 1)
        {
            if (useUniformColor) DrawUniform(GL_TRIANGLE_FAN, 6);
            else Draw(GL_TRIANGLE_FAN, 6);
        }
        if (currentFigure == 2)
        {
            if (useUniformColor) DrawUniform(GL_TRIANGLE_FAN, 5);
            else Draw(GL_TRIANGLE_FAN, 5);
        }

        window.display();
    }

    return 0;
}