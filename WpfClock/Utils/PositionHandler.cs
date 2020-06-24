using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClockApplication.Utils
{
    /// <summary>
    /// Changes position of window via mouse actions
    /// </summary>
    class PositionHandler
    {
        readonly Window window;
        Point mouseDownPos;
        bool leftMouseButtonPressed, positionChanged;

        public PositionHandler(Window window)
        {
            this.window = window;
        }

        public bool PositionChanged => positionChanged;

        public void MouseUp(System.Windows.Input.MouseEventArgs e)
        {
            leftMouseButtonPressed = false;
        }

        public void MouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            MouseUp(e);
        }

        public void MouseMove(System.Windows.Input.MouseEventArgs e)
        {
            //если кнопка мыши нажата
            if (leftMouseButtonPressed)
            {
                Point p = e.GetPosition(window);

                //узнать разницу в координатах мыши. Изменить координаты окна в соответствии с этой разницей
                window.Left -=
                    mouseDownPos.X - p.X;//на сколько изменилась позиция мыши
                window.Top -=
                    mouseDownPos.Y - p.Y;//на сколько изменилась позиция мыши

                positionChanged = true;
            }
        }

        public void MouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            //позиция мыши относительно окна
            mouseDownPos = e.GetPosition(window);

            leftMouseButtonPressed = true;
        }
    }
}
