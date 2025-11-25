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
            }
        }

        glClear(GL_COLOR_BUFFER_BIT);

        if (currentFigure == 0) Draw(GL_TRIANGLE_FAN, 4);
        if (currentFigure == 1) Draw(GL_TRIANGLE_FAN, 6);
        if (currentFigure == 2) Draw(GL_TRIANGLE_FAN, 5);

        window.display();
    }

    return 0;
}
